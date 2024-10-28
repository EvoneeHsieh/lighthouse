using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    public bool isCharged = false;

    public void SetCharged(Material chargedMaterial)
    {
        isCharged = true;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = chargedMaterial;
        }
    }

    // Detects player collision and starts charging
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCharged)
        {
            GameManager.instance.StartCharging(this);
        }
    }
}
