using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTesting : MonoBehaviour
{
    public float maxDistance = 10f;

    void Update()
    {
        // �q�ثe�����m���e�� ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // �e�X debug �u
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // �p�G�g�u����F��
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
        }
    }
}
