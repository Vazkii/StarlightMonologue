using UnityEngine;
using System.Collections;

public class TestBoss : BossController
{
    [Header("Descend")]
    public float descendSpeed = -0.5F;
    public float descendTarget = 3.5F;

    // ======================================================================================================

    private float startPos;

    void Start()
    {
        startPos = transform.localPosition.y;
    }

    protected override void PhaseUpdate()
    {
        base.PhaseUpdate();

        if(phase == 0) {
                float y = startPos + phaseTime * descendSpeed;
                if(y < descendTarget) {
                    y = descendTarget;
                    SetPhase(1);
                }

                transform.localPosition = new Vector3(0, y, 0);
        }
    }

}
