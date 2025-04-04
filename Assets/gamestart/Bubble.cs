using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed; // 每顆自己的速度

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.localPosition.y > 1200f)
        {
            Destroy(gameObject);
        }
    }
}
