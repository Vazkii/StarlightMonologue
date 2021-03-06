﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public const float TAU = Mathf.PI * 2;

    public delegate void DeductCallback(int i);

    [Header("Generic Boss Parameters")]
    public Transform playfield;
    public GameObject bulletPrefab;
    public Transform playerEntity;
    public Text debugText;
    public GameplayScene scene;

    [Header("Phase Data")]
    public int startingPhase = 0;
    public PhaseInfo[] phases;

    protected int phase = 0;
    protected bool waiting = false;
    protected float waitTime = -1;
    protected float expectedPhaseTime = -1;

    protected float totalTime;
    protected float phaseTime;
    protected AttackTime[] times;
    protected SpriteRenderer sprite;
    protected ParticleSystem particles;

    protected List<GameObject> targets = new List<GameObject>();

    public virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        SetPhase(startingPhase);
        targets.Add(gameObject);

        UnityEngine.Random.InitState(0);
    }

    void Update()
    {
        float delta = Time.deltaTime;

        if(!waiting && times != null)
            foreach(AttackTime time in times)
                time.Step(delta);

        phaseTime += delta;
        totalTime += delta;

        if(expectedPhaseTime > -1 && phaseTime >= expectedPhaseTime)
            SetPhase(phase + 1);

        if(waitTime > -1 && phaseTime >= waitTime)
            waiting = false;

        if(debugText != null)
            debugText.text = string.Format("P{0}: {1:N2} (Total: {2:N2})", phase, phaseTime, totalTime);

        PhaseUpdate();    
    }

    protected virtual void PhaseUpdate()
    {
        if(phase >= phases.Length)
            return;

        PhaseInfo info = phases[phase];
        Attack[] attacks = info.attacks;
        for(int i = 0; i < attacks.Length; i++)
            RunAttack(times[i], attacks[i]);
    }

    protected void Expect(float time)
    {
        expectedPhaseTime = time == -1 ? -1 : time + waitTime;
    }

    protected void Wait(float time)
    {
        if(time > 0) {
            waiting = true;
            waitTime = time;
        } else
            foreach(AttackTime atime in times)
                atime.Step(-time);
            
    }

    protected void SetPhase(int phase)
    {
        this.phase = phase;

        StepIntoPhase();
    }

    protected virtual void StepIntoPhase()
    {
        expectedPhaseTime = -1;
        waitTime = -1;
        phaseTime = 0;

        if(phase >= phases.Length)
            return;

        PhaseInfo info = phases[phase];

        times = new AttackTime[info.attacks.Length];
        for(int i = 0; i < times.Length; i++)
            times[i] = new AttackTime();

        Wait(info.wait);
        Expect(info.expected);
    }

    public GameObject SpawnBullet(float angle, float speed, Vector3 pos, Color color, float scale = 1F)
    {
        Vector2 force = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);

        GameObject obj = Instantiate(bulletPrefab, pos, Quaternion.identity);
        obj.transform.parent = playfield;
        obj.transform.localScale = Vector3.one * scale;

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        body.AddForce(force);

        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        sprite.color = color;

        return obj;
    }

    public void SpawnPlayerTrackingBullet(float speed, Vector3 pos, Color color, float scale = 1F)
    {
        SpawnBulletTowardsPoint(speed, pos, playerEntity.position, color, scale);
    }

    public void SpawnBulletTowardsPoint(float speed, Vector3 pos, Vector3 target, Color color, float scale = 1F)
    {
        float angle = TrueAngle(pos, target);
        SpawnBullet(angle, speed, pos, color, scale);
    }

    protected void SpawnCircleBurst(float speed, Vector3 pos, float offset, float spread, Color color, float scale = 1F)
    {
        if(spread == 0)
            return;

        float angle = 0F;
        float realSpread = Mathf.Deg2Rad * spread;

        while(angle < TAU) {
            float offAngle = angle + offset;
            SpawnBullet(offAngle, speed, pos, color, scale);

            angle += realSpread;
        }
    }

    protected void SpawnDirectionalBurst(float speed, Vector3 pos, float start, float length, Color color, float scale = 1F)
    {
        float angle = UnityEngine.Random.Range(start, start + length);    
        SpawnBullet(angle, speed, pos, color, scale);
    }

    protected static float TrueAngle(Vector2 a, Vector2 b)
    {
        return Mathf.Deg2Rad * (Vector2.Angle(a, b) * Mathf.Sign(a.x * b.y - a.y * b.x) + 90F);
    }

    // ================== Generic Attacks ==================

    protected void RunAttack(AttackTime time, Attack a)
    {
        switch(a.type) {
            case AttackType.SPIN:
                SpinAttack(time, a);
                break;
            case AttackType.TARGET_PLAYER:
                TargetPlayerAttack(time, a);
                break;
            case AttackType.CIRCLE_BURST:
                CircleBurstAttack(time, a);
                break;
            case AttackType.DIRECTIONAL_BURST:
                DirectionalBurstAttack(time, a);
                break;
            case AttackType.CUSTOM:
                CustomAttack(time, a);
                break;
        }
    }

    protected void SpinAttack(AttackTime time, Attack a)
    {
        time.Deduct(a.delay, i => SpawnBullet((float)i * a.rotation + a.offset, a.bulletSpeed, targets[a.start].transform.position, a.color, a.size));
    }

    protected void TargetPlayerAttack(AttackTime time, Attack a)
    {
        time.Deduct(a.delay, i => SpawnPlayerTrackingBullet(a.bulletSpeed, targets[a.start].transform.position, a.color, a.size));
    }

    protected void CircleBurstAttack(AttackTime time, Attack a)
    {
        time.Deduct(a.delay, i => SpawnCircleBurst(a.bulletSpeed, targets[a.start].transform.position, (float) i * a.rotation + (i % 2) * a.offset, a.spread, a.color, a.size));
    }

    protected void DirectionalBurstAttack(AttackTime time, Attack a)
    {
        time.Deduct(a.delay, i => SpawnDirectionalBurst(a.bulletSpeed, targets[a.start].transform.position, (a.offset + (float) i * a.rotation * a.delay) * Mathf.Deg2Rad, a.spread * Mathf.Deg2Rad, a.color, a.size));
    }

    protected virtual void CustomAttack(AttackTime time, Attack a) { }

    // ================== Serializable Stuff ==================

    [Serializable]
    public class PhaseInfo
    {
        public float wait = 0F;
        public float expected = -1F;
        public Attack[] attacks = new Attack[0];
    }

    [Serializable]
    public class Attack
    {
        public String optionalName = "";
        public float delay = 1F;
        public float bulletSpeed = 100F;
        public float rotation = 900F;
        public float offset = 0F;
        public float spread = 0F;
        public float size = 1F;
        public int start = 0;
        public Color color = Color.white;
        public AttackType type = AttackType.SPIN;
    }

    [Serializable]
    public enum AttackType
    {
        SPIN, TARGET_PLAYER, CIRCLE_BURST, DIRECTIONAL_BURST, CUSTOM

    }

    protected class AttackTime
    {
        protected float elapsedTime;
        protected int timesCalled;

        public void Step(float delta)
        {
            elapsedTime += delta;
        }

        public void Deduct(float time, DeductCallback cb)
        {
            while(time > 0 && elapsedTime > time) {
                elapsedTime -= time;
                timesCalled++;
                cb(timesCalled);
            }
        }

    }

}
