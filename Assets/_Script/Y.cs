using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Y : MonoBehaviour
{
    public InputActionReference talk;
    private void Update()
    {
        if (talk.action.WasPressedThisFrame())
        {
            Debug.Log("XR �������QĲ�o�I");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("���a���U T ��I");
        }
    }
}

