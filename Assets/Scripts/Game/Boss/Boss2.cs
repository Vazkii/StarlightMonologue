using UnityEngine;
using System.Collections;

public class Boss2 : BossController
{
    [Header("Descend")]
    public float descendSpeed = -0.5F;
    public float descendTarget = 3.5F;

    // ======================================================================================================

    private float startPos;
    private bool didCutin;

    public override void Start()
    {
        base.Start();

        didCutin = false;
        startPos = transform.localPosition.y;
    }

    protected override void PhaseUpdate()
    {
        base.PhaseUpdate();

        int len = phases.Length;

        if(phase == 0) {
                float y = startPos + phaseTime * descendSpeed;
                if(y < descendTarget) {
                    y = descendTarget;
                    SetPhase(1);
                }

                transform.localPosition = new Vector3(0, y, 0);
        } else if(phase == len && phaseTime > 2) {
            float a = Mathf.Max(0F, 1F - ((phaseTime - 2) / 3F));
            sprite.color = new Color(1F, 1F, 1F, a);
            particles.Stop();
        }

        if(!didCutin && totalTime > 14) {
            scene.AnimateCutin("Shutdown, Shutdown");
            didCutin = true;
        }
    }

}
