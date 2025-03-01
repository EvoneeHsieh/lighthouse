using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderFollow : MonoBehaviour
{
    public Transform headTransform; // �A�� Main Camera�]�Y���^
    public Vector3 offset = new Vector3(0, -0.1f, 0); // �� Collider �b�Y���y�L���U

    void LateUpdate()
    {
        if (headTransform != null)
        {
            transform.position = headTransform.position + offset;
            transform.rotation = headTransform.rotation;
        }
    }
}
