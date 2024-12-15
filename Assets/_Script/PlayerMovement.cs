using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameManager gameManager;
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Gate"))
    //    {
    //        Debug.Log("Hi");//it work
    //        if (gameManager.gateChargeMax)
    //        {
    //            Debug.Log("Why");//work
    //            SceneManager.LoadScene("Test1");
    //            GameManager.instance.OnPlayerTouchGate();
    //        }
    //    }
    //}
    //public void OnTriggerStay(Collider other)
    //{
    //    if (gameManager.gateChargeMax)
    //    {
    //        Debug.Log("Why");
    //        SceneManager.LoadScene("Test1");
    //        //GameManager.instance.OnPlayerTouchGate();
    //    }
    //}
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
