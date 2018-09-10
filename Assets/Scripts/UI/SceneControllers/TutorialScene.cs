using UnityEngine.SceneManagement;

public class TutorialScene : SceneController
{
    public override void FinishAnimating()
    {
        if(animDown)
            SceneManager.LoadScene("Level1");
        else AnimateDown();
    }
}
