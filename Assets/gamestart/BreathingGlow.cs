using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathingGlow : MonoBehaviour
{
    public float minAlpha = 0.01f;
    public float maxAlpha = 0.6f;

    private Image img;
    private float currentAlpha;
    private float targetAlpha;
    private float changeSpeed;

    void Start()
    {
        img = GetComponent<Image>();
        currentAlpha = img.color.a;
        PickNewTarget();
    }

    void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, changeSpeed * Time.deltaTime);

        Color c = img.color;
        c.a = currentAlpha;
        img.color = c;

        if (Mathf.Abs(currentAlpha - targetAlpha) < 0.01f)
        {
            PickNewTarget(); // 到達目標就換下一個目標
        }
    }

    void PickNewTarget()
    {
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        changeSpeed = Random.Range(0.1f, 0.3f); // 越小越慢
    }
}
