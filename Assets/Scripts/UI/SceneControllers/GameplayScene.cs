using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayScene : SceneController
{

    public RectTransform cutinPanel;
    public Text cutinText;

    public PlayerController playerController;
    public AudioSource audioSource;
    public float songTime;
    public int nextScene;

    private float elapsedTime;
    protected bool done;

    private float cutinAnimTime;
    private float cutinPauseTime;
    private bool animatingCutin;
    private float cutinTime;

    public override void FinishAnimating()
    {
        if(animDown)
            SceneManager.LoadScene(done ? nextScene : SceneManager.GetActiveScene().buildIndex);
        else playerController.allowInput = true;
    }

    public void OnPlayerDeath()
    {
        AnimateDown();
    }

    public void AnimateCutin(string text, float cutinAnimTime = 0.3F, float cutinPauseTime = 0.6F)
    {
        cutinText.text = text;
        animatingCutin = true;
        this.cutinAnimTime = cutinAnimTime;
        this.cutinPauseTime = cutinPauseTime;
        cutinTime = 0;
        cutinPanel.localScale = new Vector3(0, 1, 1);
    }

    public override void Update()
    {
        base.Update();

        if(animatingCutin) {
            float total = cutinAnimTime * 2 + cutinPauseTime;
            cutinTime += Time.deltaTime;

            float xScale = Mathf.Min(1, (cutinTime / cutinAnimTime));
            float yScale = 1F - Mathf.Max(0, Mathf.Min(1, (cutinTime - cutinAnimTime - cutinPauseTime) / cutinAnimTime));
            cutinPanel.localScale = new Vector3(xScale, yScale, 1);

            if(cutinTime > total)
                animatingCutin = false;
        }

        if(animating && animDown && funcTicks > HALF_TICKS)
            audioSource.volume = 1F - (funcTicks - HALF_TICKS) / HALF_TICKS;

        elapsedTime += Time.deltaTime;
        if(!animating && elapsedTime > songTime) {
            done = true;
            outTransitionMult = 0.5F;
            AnimateDown();
        }
    }

}
