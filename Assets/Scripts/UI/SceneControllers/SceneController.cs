using UnityEngine;
using UnityEngine.UI;

public abstract class SceneController : MonoBehaviour
{
    protected const int MAX_TICKS = 512;
    protected const int HALF_TICKS = MAX_TICKS / 2;

    public Image blackPanel1, blackPanel2;
    public float animSpeed = 35;
    public float outTransitionMult = 2.5F;

    protected float funcTicks;
    protected bool animDown;
    protected bool animating;

    public virtual void Start()
    {
        AnimateUp();
    }

    public virtual void AnimateUp()
    {
        funcTicks = MAX_TICKS;
        animDown = false;
        animating = true;
    }

    public virtual void AnimateDown()
    {
        funcTicks = 0;
        animDown = true;
        animating = true;
        animSpeed *= outTransitionMult;
    }

    public abstract void FinishAnimating();

    public virtual void Update()
    {
        if(animating) {
            float old = funcTicks;
            float delta = animSpeed * Time.deltaTime * (animDown ? 1 : -1);

            funcTicks = Mathf.Min(MAX_TICKS, Mathf.Max(0, funcTicks + delta));

            if(funcTicks == old) {
                animating = false;
                FinishAnimating();
            }

            SetAlpha(blackPanel1, (int)funcTicks);

            if(blackPanel2 != null)
                SetAlpha(blackPanel2, (int)funcTicks - HALF_TICKS);
        }
    }

    private void SetAlpha(MaskableGraphic g, int a)
    {
        g.color = new Color32(0, 0, 0, (byte)Mathf.Min(255, Mathf.Max(0, a)));
    }
}
