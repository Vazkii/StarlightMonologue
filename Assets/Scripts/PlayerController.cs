using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameplayScene scene;
    public SpriteRenderer hitbox;
    public AudioClip damageSound;
    public float moveSpeed = 1;
    public float moveSpeedShift = 0.2F;

    public bool allowInput { get; set; }
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        allowInput = false;
    }

    void Update () {
        if(!allowInput)
            return;

        const float hitboxFadeSpeed = 4F;

        float motionX = 0, motionY = 0;
        bool shift = Input.GetKey(KeyCode.LeftShift);
        float speed = (shift ? moveSpeedShift : moveSpeed) * Time.deltaTime;

        if(Input.GetKey(KeyCode.W))
            motionY += 1;
        if(Input.GetKey(KeyCode.S))
            motionY -= 1;

        if(Input.GetKey(KeyCode.D))
            motionX += 1;
        if(Input.GetKey(KeyCode.A))
            motionX -= 1;

        Vector3 motion = new Vector3(motionX, motionY, 0);
        motion.Normalize();
        motion.Scale(new Vector3(speed, speed, 0));

        transform.position += motion;

        float a = hitbox.color.a;
        if(shift)
            a = Mathf.Min(1, a + hitboxFadeSpeed * Time.deltaTime);
        else a = Mathf.Max(0, a - hitboxFadeSpeed * Time.deltaTime);

        hitbox.color = new Color(1F, 1F, 1F, a);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform target = collision.collider.transform;
        if(target.tag == "KillThing" && allowInput) {
            allowInput = false;
            audioSource.PlayOneShot(damageSound);
            scene.OnPlayerDeath();
        }
    }
}
