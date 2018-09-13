using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    [Header("Links")]
    public GameplayScene scene;
    public SpriteRenderer hitbox;
    public Image[] hearts;
    public AudioClip damageSound;

    [Header("Gameplay")]
    public float moveSpeed = 1;
    public float moveSpeedShift = 0.2F;
    public int maxHealth = 3;
    public float iframeTime = 0.5F;

    public bool allowInput { get; set; }
    private int health;
    private float iframes = 0;
    private AudioSource audioSource;
    private SpriteRenderer sprite;

    private Vector2 TOP_RIGHT = new Vector2(8.47F, 3.97F);
    private Vector2 BOTTOM_LEFT = new Vector2(-8.26F, -3.97F);

    private void Start()
    {
        this.health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        allowInput = false;
    }

    void Update () {
        if(!allowInput)
            return;

        float delta = Time.deltaTime;
        if(iframes > 0) {
            iframes -= delta;

            if(iframes < 0) { 
                gameObject.layer = 10; // Player
                sprite.color = new Color32(255, 255, 255, 255);
            }
        }

        const float hitboxFadeSpeed = 4F;

        float motionX = 0, motionY = 0;
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); 
        float speed = (shift ? moveSpeedShift : moveSpeed) * delta;

        if(Input.GetKey(KeyCode.UpArrow))
            motionY += 1;
        if(Input.GetKey(KeyCode.DownArrow))
            motionY -= 1;

        if(Input.GetKey(KeyCode.RightArrow))
            motionX += 1;
        if(Input.GetKey(KeyCode.LeftArrow))
            motionX -= 1;

        Vector3 motion = new Vector3(motionX, motionY, 0);
        motion.Normalize();
        motion.Scale(new Vector3(speed, speed, 0));

        transform.position += motion;
        ClampPosition();

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
            health--;

            audioSource.PlayOneShot(damageSound);
            Destroy(collision.collider.gameObject);

            if(health < 0) {
                allowInput = false;
                scene.OnPlayerDeath();
            } else
                hearts[health].color = new Color32(0, 0, 0, 0);
                sprite.color = new Color32(190, 190, 190, 255);
                iframes = iframeTime;
                gameObject.layer = 11; // Invulnerable
            }
        }
    }


    private void ClampPosition()
    {
        float x = Mathf.Max(BOTTOM_LEFT.x, Mathf.Min(TOP_RIGHT.x, transform.localPosition.x));
        float y = Mathf.Max(BOTTOM_LEFT.y, Mathf.Min(TOP_RIGHT.y, transform.localPosition.y));
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }
}
