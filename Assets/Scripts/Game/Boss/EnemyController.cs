using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class EnemyController : MonoBehaviour
{

    public delegate void DeductCallback(int i);

    public Transform playfield;
    public GameObject bullet;

    public Text text;

    protected float ticksSinceReset;
    protected float ticksElapsed;
    protected int timesCalled;

    public virtual void Update()
    {
        ticksElapsed += Time.deltaTime;
        ticksSinceReset += Time.deltaTime;

        if(text != null) {
            text.text = "Phase Time: " + ticksSinceReset;
        }
    }

    public void Deduct(float time, DeductCallback cb)
    {
        while(time > 0 && ticksElapsed > time) {
            ticksElapsed -= time;
            timesCalled++;
            cb(timesCalled);
        }   
    }

    public void ResetCounter()
    {
        ticksElapsed = 0;
        ticksSinceReset = 0;
        timesCalled = 0;
    }

    public void SpawnBullet(float angle, float speed, Color color, float scale = 1F)
    {
        Vector2 force = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
        SpawnBullet(force, transform.position, color, scale);
    }

    public void SpawnBullet(Vector2 force, Vector3 pos, Color color, float scale = 1F)
    {
        GameObject obj = Instantiate(bullet, pos, Quaternion.identity);
        obj.transform.parent = playfield;
        obj.transform.localScale = Vector3.one * scale;

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        body.AddForce(force);

        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        sprite.color = color;
    }
}
