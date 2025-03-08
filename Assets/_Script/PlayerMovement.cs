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

    //jumping
    public float jumpForce = 5f; // ���D�O�D
    public float jumpCooldown = 1f; // ���D�N�o�ɶ�
    public bool canJump = true;
    public bool isGrounded = true;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        if (playerRb == null)
        {
            Debug.LogError("�䤣�� Rigidbody�A�нT�O���}�����b�� Rigidbody ������W�I");
        }
    }

    private void Update()
    {
        // VR ������s��J
        if (jump.action.WasPressedThisFrame() && canJump && isGrounded)
        {
            Debug.Log("VR ������s���U");
            jumpStart();
        }

        // ��L�����J (�ť���)
        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
        {
            Debug.Log("�ť�����U");
            jumpStart();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gate"))
        {
            Debug.Log("Hi"); // ����Ĳ�o
            if (gameManager.gateChargeMax)
            {
                Debug.Log("Why"); // ����Ĳ�o
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
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // ���a���}�a��
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
            isGrounded = true; // �T�O���a�b�a���W
        }
    }

    //jump
    private void jumpStart()
    {
        if (playerRb == null) return;

        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �I�[�V�W���O
        canJump = false;
        isGrounded = false;
        Invoke(nameof(ResetJump), jumpCooldown); // �]�w�N�o�ɶ�
    }

    private void ResetJump()
    {
        canJump = true;
    }
}
