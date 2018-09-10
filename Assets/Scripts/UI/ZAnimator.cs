using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAnimator : MonoBehaviour {

    public float timeOffset = 0; 
    public float rotateSpeed = 1;
    public float transparencySpeed = 1;

    float time;
    CanvasRenderer canvasRenderer;
    RectTransform targetRect;

	void Start () {
        canvasRenderer = GetComponent<CanvasRenderer>();
        targetRect = GetComponent<RectTransform>();
	}
	
	void Update () {
        time += Time.deltaTime;

        canvasRenderer.SetAlpha(Mathf.Sin(time * transparencySpeed + timeOffset) * 0.7F + 0.5F);
        targetRect.rotation = Quaternion.Euler(0, 0, Mathf.Cos(time * rotateSpeed + timeOffset) * 40);
	}
}
