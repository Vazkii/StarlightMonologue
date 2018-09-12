using UnityEngine;
using System.Collections;

public class TestBoss : EnemyController
{
    [Header("Descend")]
    public float descendSpeed = -0.5F;
    public float descendTarget = 3.5F;

    [Header("Phase Data")]
    public PhaseInfo phase1;
    public PhaseInfo phase2;
    public PhaseInfo phase3;

    // ======================================================================================================

    private float startPos;

    void Start()
    {
        startPos = transform.localPosition.y;
    }

    protected override void StepIntoPhase()
    {
        base.StepIntoPhase();

        switch(phase) {
            case 1: Wait(phase1.wait); Expect(phase1.expected); break;
            case 2: Wait(phase2.wait); Expect(phase2.expected); break;
            case 3: Wait(phase3.wait); Expect(phase3.expected); break;
        }
    }

    protected override void PhaseUpdate()
    {
        switch(phase) {
            case 0:
                float y = startPos + elapsedTime * descendSpeed;
                if(y < descendTarget) {
                    y = descendTarget;
                    SetPhase(1);
                }

                transform.localPosition = new Vector3(0, y, 0);
                break;
            case 1:
                Deduct(phase1.delay, i => SpawnBullet((float) i * phase1.rotation, phase1.bulletSpeed, phase1.color, phase1.size));
                break;
            case 2:
                Deduct(phase2.delay, i => SpawnBullet((float)i * phase2.rotation, phase2.bulletSpeed, phase2.color, phase2.size));
                break;
            case 3:
                Deduct(phase3.delay, i => SpawnPlayerTrackingBullet(phase3.bulletSpeed, phase3.color, phase3.size));
                break;
        }
    }



}
