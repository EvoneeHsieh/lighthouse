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

    public Rigidbody playerRb;
    public InputActionReference inputAction; // VR 控制器按鍵輸入
    public GameManager gameManager;

    private bool isClimbing = false; // 是否正在攀爬
    private ClimbProvider climbProvider;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        if (playerRb == null)
        {
            Debug.LogError("找不到 Rigidbody，請確保此腳本掛在有 Rigidbody 的物件上！");
        }
    }

    private void Update()
    {
        // VR 控制器按鈕輸入
        if (inputAction.action.WasPressedThisFrame() && canJump)
        {
            Jump();
        }

        // 鍵盤按鍵輸入 (Q 鍵)
        if (Input.GetKeyDown(KeyCode.Q) && canJump)
        {
            Jump();
        }
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

        // 檢查是否進入攀爬區域
        if (other.gameObject.CompareTag("Climbable"))
        {
            StartClimbing();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Climbable"))
        {
            StopClimbing();
        }

        climbProvider = GetComponent<ClimbProvider>();
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

    // **開始攀爬：解鎖旋轉**
    private void StartClimbing()
    {
        isClimbing = true;
        playerRb.constraints = RigidbodyConstraints.None; // 解除所有限制
        Debug.Log("開始攀爬，解除旋轉鎖定");
    }

    // **停止攀爬：恢復旋轉鎖定**
    private void StopClimbing()
    {
        isClimbing = false;
        LockRotation();
        Debug.Log("停止攀爬，恢復旋轉鎖定");
    }

    // **鎖定旋轉**
    private void LockRotation()
    {
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Debug.Log("旋轉鎖定已恢復");
    }
}
