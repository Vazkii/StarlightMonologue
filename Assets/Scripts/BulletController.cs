using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{

    private Vector2 TOP_RIGHT = new Vector2(10.47F, 5.97F);
    private Vector2 BOTTOM_LEFT = new Vector2(-10.26F, -5.97F);

    void Update()
    {
        if(transform.localPosition.x < BOTTOM_LEFT.x ||
            transform.localPosition.y < BOTTOM_LEFT.y ||
            transform.localPosition.x > TOP_RIGHT.x ||
            transform.localPosition.y > TOP_RIGHT.y)
            Destroy(gameObject);
    }
}
