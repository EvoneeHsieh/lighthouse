using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameManager gameManager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gate"))
        {
            Debug.Log("Hi");//it work
            if (gameManager.gateChargeMax)
            {
                Debug.Log("Why");//work
                SceneManager.LoadScene("2_Hold2");
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        ClimbProvider climbProvider = GetComponent<ClimbProvider>();
        if (climbProvider != null && climbProvider.locomotionPhase != LocomotionPhase.Moving)
        {
            playerRb.useGravity = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            GameManager.instance.PlayerTouchWater();
        }
    }
}
