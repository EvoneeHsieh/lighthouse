using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a "Player" tag
        {
            if (gameManager.gateChargeMax)
            {
                Debug.Log("Open");
                SceneManager.LoadScene("Test1");
            }
        }
    }
}
