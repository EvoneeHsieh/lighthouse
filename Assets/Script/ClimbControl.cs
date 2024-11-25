using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl : MonoBehaviour
{
    public bool isClimbing = false; // �O�_���b�k��
    private GameObject climbableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            climbableObject = other.gameObject;

            // �_?��?
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
