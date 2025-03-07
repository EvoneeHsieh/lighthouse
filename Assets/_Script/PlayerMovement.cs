using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 5f; // ���D�O�D
    public float jumpCooldown = 1f; // ���D�N�o�ɶ�
    private bool canJump = true;

    public Rigidbody playerRb;
    public InputActionReference inputAction; // VR ��������J
    public GameManager gameManager;

    private bool isClimbing = false; // �O�_���b�k��
    private ClimbProvider climbProvider;

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
        if (inputAction.action.WasPressedThisFrame() && canJump)
        {
            Jump();
        }

        // ��L�����J (Q ��)
        if (Input.GetKeyDown(KeyCode.Q) && canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (playerRb == null) return;

        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �I�[�V�W���O
        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown); // �]�w�N�o�ɶ�
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

        // �ˬd�O�_�i�J�k���ϰ�
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

    // **�}�l�k���G�������**
    private void StartClimbing()
    {
        isClimbing = true;
        playerRb.constraints = RigidbodyConstraints.None; // �Ѱ��Ҧ�����
        Debug.Log("�}�l�k���A�Ѱ�������w");
    }

    // **�����k���G��_������w**
    private void StopClimbing()
    {
        isClimbing = false;
        LockRotation();
        Debug.Log("�����k���A��_������w");
    }

    // **��w����**
    private void LockRotation()
    {
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Debug.Log("������w�w��_");
    }
}
