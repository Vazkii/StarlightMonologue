using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayScene : SceneController
{

    public PlayerController playerController;
    public AudioSource audioSource;

    public override void FinishAnimating()
    {
        if(animDown)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else playerController.allowInput = true;
    }

    public void OnPlayerDeath()
    {
        AnimateDown();
    }

    public override void Update()
    {
        base.Update();

        if(animating && animDown && funcTicks > HALF_TICKS)
            audioSource.volume = 1F - (funcTicks - HALF_TICKS) / HALF_TICKS;
    }

}
