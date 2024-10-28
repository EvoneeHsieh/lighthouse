using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // ���ʳt��
    public float mouseSensitivity = 100f;
    public float jumpForce = 5f; // ���D�O�q
    public LayerMask groundLayer; // �a���h�� LayerMask�A�Ω��˴�����O�_�b�a���W

    public Transform playerBody; // ���a����
    public Transform cameraTransform; // �ṳ��

    private Rigidbody rb; // Rigidbody �Ѧ�
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ����z����v�T����

        Cursor.lockState = CursorLockMode.Locked; // ��w�ƹ�
    }

    void Update()
    {
        // �����ṳ��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // �������������

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ��������W�U����
        playerBody.Rotate(Vector3.up * mouseX); // ����⥪�k����

        // ������D�P�_
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // �ϥ��ṳ�����e�ᥪ�k��V����
        float x = Input.GetAxis("Horizontal"); // A/D �� ���k��
        float z = Input.GetAxis("Vertical");   // W/S �� �W�U��

        // �p�Ⲿ�ʤ�V
        Vector3 moveDirection = (cameraTransform.forward * z + cameraTransform.right * x).normalized;
        moveDirection.y = 0f; // �����]�ṳ���ɱצӦbY�b����

        // �ϥ� Rigidbody �i�沾��
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    // �P�_����O�_�b�a���W
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    // ���D��k
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �V�W�I�[�O�Ϩ�����_
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name); // �ˬd�I��������W��
        if (collision.gameObject.CompareTag("Water"))
        {
            GameManager.instance.PlayerTouchWater();
        }
        if (collision.gameObject.CompareTag("Gate"))
        {
            GameManager.instance.OnPlayerTouchGate();
        }
    }


}
