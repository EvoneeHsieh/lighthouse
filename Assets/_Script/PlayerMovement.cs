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

    private Rigidbody playerRb;
    public InputActionReference inputAction; // VR ��������J

    public GameManager gameManager;
    private ClimbProvider climbProvider;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        climbProvider = GetComponent<ClimbProvider>(); // ���o ClimbProvider

        if (playerRb == null)
        {
            Debug.LogError("�䤣�� Rigidbody�A�нT�O���}�����b�� Rigidbody ������W�I");
        }
    }

    private void Update()
    {
        // VR ������s��J
        if (inputAction.action.WasPressedThisFrame() && canJump && !IsClimbing())
        {
            Jump();
        }

        // ��L�����J (Q ��)
        if (Input.GetKeyDown(KeyCode.Q) && canJump && !IsClimbing())
        {
            Jump();
        }

        // �۰��˴��O�_�b�k���A�ھڪ��A����O
        HandleGravity();
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
    }

    public void OnTriggerExit(Collider other)
    {
        // �T�O ClimbProvider �s�b�A�åB���a���}�k���ɶ}�ҭ��O
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

    // **�ˬd�O�_�b�k��**
    private bool IsClimbing()
    {
        return climbProvider != null && climbProvider.locomotionPhase == LocomotionPhase.Moving;
    }

    // **�ʺA����O**
    private void HandleGravity()
    {
        if (IsClimbing())
        {
            playerRb.useGravity = false;
            playerRb.velocity = Vector3.zero;
        }
        else
        {
            StartCoroutine(EnableGravitySmoothly()); // ���ƱҰʭ��O
        }
    }
    private IEnumerator EnableGravitySmoothly()
    {
        yield return new WaitForSeconds(0.1f); // �u�ȵ��ݡA�T�O���}�k�����A
        playerRb.useGravity = true; // �A�}�ҭ��O
    }

}
