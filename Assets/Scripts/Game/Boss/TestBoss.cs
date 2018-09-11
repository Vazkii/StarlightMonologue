using UnityEngine;
using System.Collections;

public class TestBoss : EnemyController
{
    public float delay = 2;
    public float bulletSpeed = 120;
    public float rotation = 300;
    public float descendSpeed = -0.5F;
    public float descendTarget = 3.5F;

    private int phase = 0;
    private float startPos;

    void Start()
    {
        startPos = transform.localPosition.y;
    }

    public override void Update()
    {
        base.Update();

        switch(phase) {
            case 0:
                float y = startPos + ticksElapsed * descendSpeed;
                if(y < descendTarget) {
                    y = descendTarget;
                    SetPhase(1);
                }

                transform.localPosition = new Vector3(0, y, 0);
                break;
            case 1:
                Deduct(4.8F, i => SetPhase(2));
                break;
            case 2:
                Deduct(delay, i => SpawnBullet((float)i * rotation, bulletSpeed, Color.white));
                if(ticksSinceReset > 5)
                    SetPhase(3);
                break;
            case 3:
                Deduct(delay / 2.5F, i => SpawnBullet((float)i * rotation, bulletSpeed * 1.5F, Color.red, 0.5F));
                break;
        }
    }

    void SetPhase(int phase)
    {
        this.phase = phase;
        ResetCounter();
    }


}
