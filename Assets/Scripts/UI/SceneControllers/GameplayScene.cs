using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayScene : SceneController
{

    public PlayerController playerController;
    public AudioSource audioSource;
    public float songTime;
    public int nextScene;

    private float elapsedTime;
    private bool done;

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

    public override void Update()
    {
        base.Update();

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
