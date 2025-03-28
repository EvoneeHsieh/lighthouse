using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameManager gameManager;
    public InputActionReference jump;
    public InputActionReference climbInput; // 合併左右手攀爬輸入

    public float jumpForce = 5f;
    public float jumpCooldown = 1f;
    public bool canJump = true;
    public bool isGrounded = true;

    private bool isClimbingZone = false;

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
        bool isClimbing = climbInput.action.IsPressed() && isClimbingZone;

        playerRb.isKinematic = isClimbing;
        playerRb.useGravity = !isClimbing;

        if ((jump.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.Space)) && canJump && isGrounded)
        {
            jumpStart();
        }
    }

    private void jumpStart()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
        isGrounded = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate") && gameManager.gateChargeMax)
        {
            SceneManager.LoadScene("2_Hold2");
        }

        if (other.CompareTag("Climbable"))
        {
            isClimbingZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Climbable"))
        {
            isClimbingZone = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            GameManager.instance.PlayerTouchWater();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
