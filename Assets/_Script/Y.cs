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
            Debug.Log("XR 控制器按鍵被觸發！");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("玩家按下 T 鍵！");
        }
    }
}

