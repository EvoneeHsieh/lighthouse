using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f; // 跳躍力道
    public float jumpCooldown = 1f; // 跳躍冷卻時間
    private bool canJump = true;

    private Rigidbody playerRb;
    public InputActionReference inputAction; // VR 控制器按鍵輸入

    public GameManager gameManager;
    private ClimbProvider climbProvider;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        climbProvider = GetComponent<ClimbProvider>(); // 取得 ClimbProvider

        if (playerRb == null)
        {
            Debug.LogError("找不到 Rigidbody，請確保此腳本掛在有 Rigidbody 的物件上！");
        }
    }

    private void Update()
    {
        // VR 控制器按鈕輸入
        if (inputAction.action.WasPressedThisFrame() && canJump && !IsClimbing())
        {
            Jump();
        }

        // 鍵盤按鍵輸入 (Q 鍵)
        if (Input.GetKeyDown(KeyCode.Q) && canJump && !IsClimbing())
        {
            Jump();
        }

        // 自動檢測是否在攀爬，根據狀態控制重力
        HandleGravity();
    }

    private void Jump()
    {
        if (playerRb == null) return;

        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 施加向上的力
        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown); // 設定冷卻時間
    }

    private void ResetJump()
    {
        canJump = true;
    }

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
        // 確保 ClimbProvider 存在，並且當玩家離開攀爬時開啟重力
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

    // **檢查是否在攀爬**
    private bool IsClimbing()
    {
        return climbProvider != null && climbProvider.locomotionPhase == LocomotionPhase.Moving;
    }

    // **動態控制重力**
    private void HandleGravity()
    {
        if (IsClimbing())
        {
            playerRb.useGravity = false;
            playerRb.velocity = Vector3.zero;
        }
        else
        {
            StartCoroutine(EnableGravitySmoothly()); // 平滑啟動重力
        }
    }
    private IEnumerator EnableGravitySmoothly()
    {
        yield return new WaitForSeconds(0.1f); // 短暫等待，確保離開攀爬狀態
        playerRb.useGravity = true; // 再開啟重力
    }

}
