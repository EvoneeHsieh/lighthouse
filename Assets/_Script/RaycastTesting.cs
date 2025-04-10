using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTesting : MonoBehaviour
{
    public float maxDistance = 10f;

    void Update()
    {
        // 從目前物件位置往前打 ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 畫出 debug 線
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // 如果射線打到東西
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
        }
    }
}
