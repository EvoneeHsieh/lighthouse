using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderFollow : MonoBehaviour
{
    public Transform headTransform; // 你的 Main Camera（頭部）
    public Vector3 offset = new Vector3(0, -0.1f, 0); // 讓 Collider 在頭部稍微往下

    void LateUpdate()
    {
        if (headTransform != null)
        {
            transform.position = headTransform.position + offset;
            transform.rotation = headTransform.rotation;
        }
    }
}
