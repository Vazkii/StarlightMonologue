using UnityEngine;
using UnityEngine.UI;

public class CreditsScene : SceneController
{
    public float creditsScrollSpeed = 1;
    public float totalTime = 118;
    public RectTransform creditsPane;
    public Image riko;
    public Sprite targetSprite;

    float elapsedTime = 0;
    bool animatingCredits;

    public override void FinishAnimating()
    {
        if(!animDown)
            animatingCredits = true;
        else {
            riko.sprite = targetSprite;
            Debug.Log("Finished!");
        }
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;
        if(animatingCredits) {
            creditsPane.localPosition += Vector3.up * creditsScrollSpeed;
            if(elapsedTime >= totalTime) {
                animatingCredits = false;
                blackPanel2 = null;
                AnimateDown();
            }
                
        }
    }
}
