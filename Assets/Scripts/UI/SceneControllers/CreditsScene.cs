using UnityEngine;
using UnityEngine.UI;

public class CreditsScene : SceneController
{
    public float creditsScrollTarget = 7700;
    public float totalTime = 118;
    public RectTransform creditsPane;
    public Image riko;
    public Sprite targetSprite;

    float creditsScrollSpeed;
    float elapsedTime = 0;
    bool animatingCredits;

    public override void Start()
    {
        base.Start();

        float distance = creditsScrollTarget - transform.localPosition.y;
        creditsScrollSpeed = (distance / totalTime);
    }

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
        
        if(animatingCredits) { 
            float delta = Time.deltaTime;
            elapsedTime += delta;

            creditsPane.localPosition += Vector3.up * creditsScrollSpeed * delta;
            if(elapsedTime >= totalTime) {
                animatingCredits = false;
                blackPanel2 = null;
                AnimateDown();
            }    
        }
    }
}
