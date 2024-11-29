using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f; // ���ʳt��
    public Transform cameraTransform; // �ṳ���]�N���Y����V�^
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ����z����
    }

    void FixedUpdate()
    {
        // ����k�n�쪺��J
        float inputX = Input.GetAxis("Horizontal"); // �k�n�� X �b (���k����)
        float inputZ = Input.GetAxis("Vertical");   // �k�n�� Y �b (�e�Ჾ��)

        // �p���Y�����¦V�]����������V�^
        Vector3 headForward = cameraTransform.forward;
        headForward.y = 0f; // �����Y������������
        headForward.Normalize();

        Vector3 headRight = cameraTransform.right;
        headRight.y = 0f; // ����������V
        headRight.Normalize();

        // �ھڿ�J�p�Ⲿ�ʤ�V
        Vector3 moveDirection = (headForward * inputZ + headRight * inputX).normalized;

        // �������a��m
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
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
