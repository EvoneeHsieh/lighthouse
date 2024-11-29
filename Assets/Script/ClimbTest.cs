using System.Collections;
using UnityEngine;

public class ClimbTest : MonoBehaviour
{
    public float gravity = 45.0f;
    public float sensitivity = 45.0f;
    private Hand currentHand = null;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // �ϥ� Rigidbody ���N CharacterController
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (currentHand != null)
        {
            CalculateMovement(); // �ھڤ⪺���ʶq�p���k��
        }
    }

    private void CalculateMovement()
    {
        Vector3 movement = Vector3.zero;

        if (currentHand != null)
        {
            movement += currentHand.Delta * sensitivity;
        }

        if (movement == Vector3.zero)
        {
            movement.y -= gravity * Time.deltaTime; // �������O
        }

        rb.MovePosition(rb.position + movement * Time.deltaTime); // ��� Rigidbody ����
    }

    public void StartClimbing()
    {
        if (currentHand == null)
        {
            currentHand = currentHand.GetComponent<Hand>();
        }
    }

    public void StopClimbing()
    {
        currentHand = null;
    }
}
