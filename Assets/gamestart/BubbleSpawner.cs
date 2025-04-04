using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public RectTransform canvas;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnBubble();
            yield return new WaitForSeconds(Random.Range(0.8f, 2f)); // 每 0.8~2 秒生成一顆
        }
    }

    void SpawnBubble()
    {
        float x = Random.Range(-500f, 500f);
        Vector3 pos = new Vector3(x, -1000f, 0f);

        GameObject bubble = Instantiate(bubblePrefab, canvas);
        bubble.GetComponent<RectTransform>().localPosition = pos;

        float scale = Random.Range(0.5f, 1.2f);
        bubble.transform.localScale = Vector3.one * scale;

        float randomSpeed = Random.Range(30f, 100f);
        bubble.GetComponent<Bubble>().speed = randomSpeed;

        // 加上隨機透明度
        Image bubbleImage = bubble.GetComponent<Image>();
        Color color = bubbleImage.color;
        color.a = Random.Range(0.1f, 1f);
        bubbleImage.color = color;
    }


}
