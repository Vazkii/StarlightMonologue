using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScene : SceneController
{

    public float pauseTime = 2;

    float elapsedTime;

    public override void FinishAnimating()
    {
        if(animDown)
            SceneManager.LoadScene("Level1");
    }

    public override void Update()
    {
        base.Update();

        if(!animating) {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > pauseTime)
                AnimateDown();
        }

    }
}
