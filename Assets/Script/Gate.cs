using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a "Player" tag
        {
            Debug.Log("weeeeeeeeee");
            GameManager.instance.OnPlayerTouchGate();
        }
    }
}
