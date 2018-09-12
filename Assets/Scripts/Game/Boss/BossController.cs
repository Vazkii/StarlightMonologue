using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyController : MonoBehaviour
{

    public delegate void DeductCallback(int i);

    [Header("Generic Boss Parameters")]
    public Transform playfield;
    public GameObject bulletPrefab;
    public Transform playerEntity;
    public Text debugText;

    protected int phase = 0;
    protected bool waiting = false;
    protected float waitTime = -1;
    protected float expectedPhaseTime = -1;

    protected float phaseTime;
    protected float elapsedTime;
    protected int timesCalled;

    void Update()
    {
        if(!waiting)
            elapsedTime += Time.deltaTime;

        phaseTime += Time.deltaTime;

        if(expectedPhaseTime > -1 && phaseTime >= expectedPhaseTime)
            SetPhase(phase + 1);
        if(waitTime > -1 && phaseTime >= waitTime)
            waiting = false;

        if(debugText != null)
            debugText.text = "Phase Time: " + phaseTime;

        PhaseUpdate();    
    }

    protected abstract void PhaseUpdate();

    protected void Deduct(float time, DeductCallback cb)
    {
        while(time > 0 && elapsedTime > time) {
            elapsedTime -= time;
            timesCalled++;
            cb(timesCalled);
        }   
    }

    protected void Expect(float time)
    {
        expectedPhaseTime = time == -1 ? -1 : time + waitTime;
    }

    protected void Wait(float time)
    {
        waiting = true;
        waitTime = time;
    }

    protected void ResetCounter()
    {
        elapsedTime = 0;
        phaseTime = 0;
        timesCalled = 0;
    }

    protected void SetPhase(int phase)
    {
        this.phase = phase;

        expectedPhaseTime = -1;
        waitTime = -1;

        StepIntoPhase();
        ResetCounter();
    }

    protected virtual void StepIntoPhase()
    {

    }

    public void SpawnBullet(float angle, float speed, Color color, float scale = 1F)
    {
        Vector2 force = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
        SpawnBullet(force, transform.position, color, scale);
    }

    public void SpawnBullet(Vector2 force, Vector3 pos, Color color, float scale = 1F)
    {
        GameObject obj = Instantiate(bulletPrefab, pos, Quaternion.identity);
        obj.transform.parent = playfield;
        obj.transform.localScale = Vector3.one * scale;

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        body.AddForce(force);

        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void SpawnPlayerTrackingBullet(float speed, Color color, float scale = 1F)
    {
        SpawnPlayerTrackingBullet(speed, transform.position, color, scale);
    }

    public void SpawnPlayerTrackingBullet(float speed, Vector3 pos, Color color, float scale = 1F)
    {
        SpawnBulletTowardsPoint(speed, pos, playerEntity.position, color, scale);
    }

    public void SpawnBulletTowardsPoint(float speed, Vector3 pos, Vector3 target, Color color, float scale = 1F)
    {
        float angle = TrueAngle(pos, target);
        Vector2 force = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
        SpawnBullet(force, pos, color, scale);
    }

    private static float TrueAngle(Vector2 a, Vector2 b) {
        return Mathf.Deg2Rad * (Vector2.Angle(a, b) * Mathf.Sign(a.x * b.y - a.y * b.x) + 90F);
    }

    [System.Serializable]
    public class PhaseInfo
    {
        public float wait = 0F;
        public float expected = -1F;
        public float delay = 1F;
        public float bulletSpeed = 100F;
        public float rotation = 900F;
        public float size = 1F;
        public Color color = Color.white;
    }

}
