using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalLevelScene : GameplayScene
{

    [Header("Final Level Settings")]
    public AudioClip finalSong;
    public ParticleSystem particles;
    public Text nowPlayingText;
    public int startTime;

    [Header("Sparkles")]
    public Transform[] sparkles;
    public Transform sparkleHolder;
    public float sparkleStart = 1F;
    public float maxSparkleDistance = 3F;
    public float sparkleDelay = 0.25F;
    public float sparkleTweenTime = 0.25F;
    public float sparkleRotateSpeed = 20F;
    public float sparkleTrailLength = 1.2F;

    [Header("Saturate")]
    public SpriteRenderer backgroundOverlay;
    public float saturateStart = 14F;
    public float saturateDuration = 4F;

    [Header("Fly Off")]
    public float flyOffStart = 20F;
    public float flyOffDuration = 2F;

    private bool skipped = false;
    private bool playing = false;
    private float internalTime = 0F;
    private Vector3 travel;

    public override void Update()
    {
        base.Update();

        float delta = Time.deltaTime;
        internalTime += delta;

        if(!skipped) {
            skipped = true;
            audioSource.time = songTime - 3;
        }

        if(internalTime > startTime) {
            if(!playing) {
                audioSource.PlayOneShot(finalSong);
                playing = true;

                playerController.allowInput = false;
                travel = playerController.transform.localPosition;

                nowPlayingText.text = nowPlayingText.text.Replace("Daisuki Dattara Daijoubu", "Kimi no Kokoro wa Kagayaiteru Kai (Instrumental)");
            }

            float animTime = internalTime - startTime;
            
            Vector3 targetPosition = (travel * Mathf.Max(0, sparkleStart - animTime) / sparkleStart);
            playerController.transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, playerController.transform.position.z);
           
            if(animTime > sparkleStart) {
                float sparkleTime = animTime - sparkleStart;
                float tweenSpeed = maxSparkleDistance / sparkleDelay;

                for(int i = 0; i < sparkles.Length; i++) {
                    Transform t = sparkles[i];
                    float y = Mathf.Max(0, Mathf.Min(maxSparkleDistance, sparkleTime * tweenSpeed - sparkleTweenTime * i));
                    if(t.transform.localPosition.y == 0) {
                        TrailRenderer trail = t.GetComponent<TrailRenderer>();
                        trail.enabled = true;
                        trail.time = sparkleTrailLength;
                    }

                    t.transform.localPosition = new Vector3(0, y, 0);
                }

                sparkleHolder.localRotation = Quaternion.Euler(0, 0, sparkleRotateSpeed * sparkleTime);
                ParticleSystem.EmissionModule emission = particles.emission;
                emission.enabled = true;
            }

            if(animTime > saturateStart) {
                float saturateTime = animTime - saturateStart;
                float saturateFract = Mathf.Min(1, Mathf.Max(0, (saturateTime / saturateDuration)));
                saturateFract = -saturateFract * (saturateFract - 2);
                backgroundOverlay.color = new Color(1, 1, 1, saturateFract);
            }

            if(animTime > flyOffStart) {
                float flyOffTime = animTime - flyOffStart;
                float flyOffFract = Mathf.Min(1F, flyOffTime / flyOffDuration);

                if(flyOffFract > 0.7 && !animating) {
                    done = true;
                    outTransitionMult = 0.5F;
                    AnimateDown();
                }                    

                flyOffFract = -flyOffFract * (flyOffFract - 2);
                float y = 9F * flyOffFract;
                playerController.transform.localPosition = new Vector3(0, y, playerController.transform.localPosition.z);
            }
        }
    }
}
