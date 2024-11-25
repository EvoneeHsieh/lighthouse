using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl : MonoBehaviour
{
    public bool isClimbing = false; // 是否正在攀爬
    private GameObject climbableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbableObject = other.gameObject;

            // 震?反?
            HapticManager.Instance.TriggerHapticFeedback(isLeftHand: true, intensity: 150, duration: 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbableObject = null;
        }
    }
}
