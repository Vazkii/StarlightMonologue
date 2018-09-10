using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : SceneController
{

    public Text text;
    public AudioSource audioSource;
    public float textAnimSpeed = 255F;

    float textFuncTicks;
    bool acceptInput;
    bool animatingText;

    public override void AnimateUp()
    {
        base.AnimateUp();
        acceptInput = false;
        animatingText = false;
    }

    public override void AnimateDown()
    {
        base.AnimateDown();
        acceptInput = false;
    }

    public override void FinishAnimating()
    {
        if(animDown)
            SceneManager.LoadScene("Tutorial");
        else {
            acceptInput = true;
            animatingText = true;
        }
    }

    public override void Update()
    {
        base.Update();

        if(animating) {
            const int audioOff = -60;
            if(animDown && (funcTicks - audioOff) > HALF_TICKS)
                audioSource.volume = 1F - ((funcTicks - audioOff) - HALF_TICKS) / HALF_TICKS;
        }

        if(animatingText) {
            textFuncTicks += Time.deltaTime * textAnimSpeed;
            float a = Mathf.Min(255, textFuncTicks);

            text.color = new Color32(255, 255, 255, (byte)a);

            if(a >= 255)
                animatingText = false;
        }

        if(acceptInput && Input.GetKeyDown(KeyCode.Return))
            AnimateDown();
    }

}
