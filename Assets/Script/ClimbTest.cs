using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTest : MonoBehaviour
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    private Rigidbody playerRigidbody;

    public float climbSpeed = 2f; // �k���t��
    private bool isClimbing = false;

    private Vector3 previousLeftHandPosition;
    private Vector3 previousRightHandPosition;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �P�_��O�_����l�]�i�ھګ���Ĳ�o�^
        if (IsHandOnLadder() && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // ������s
        {
            if (!isClimbing)
                StartClimbing();
        }
        else
        {
            if (isClimbing)
                StopClimbing();
        }

        if (isClimbing)
        {
            ClimbMovement();
        }
    }

    private void StartClimbing()
    {
        isClimbing = true;
        previousLeftHandPosition = leftHandTransform.position;
        previousRightHandPosition = rightHandTransform.position;
        playerRigidbody.useGravity = false; // �T�έ��O
    }

    private void StopClimbing()
    {
        isClimbing = false;
        playerRigidbody.useGravity = true; // ��_���O
    }

    private void ClimbMovement()
    {
        // �p��⪺���ʶq
        Vector3 leftHandDelta = leftHandTransform.position - previousLeftHandPosition;
        Vector3 rightHandDelta = rightHandTransform.position - previousRightHandPosition;

        // �ھڤ⪺���ʤ�V�A�����a�uY�b����
        Vector3 climbMovement = new Vector3(0, (leftHandDelta.y + rightHandDelta.y) * climbSpeed, 0);

        transform.position += climbMovement;

        // ��s�⪺��m
        previousLeftHandPosition = leftHandTransform.position;
        previousRightHandPosition = rightHandTransform.position;
    }

    private bool IsHandOnLadder()
    {
        // �o�̥i�H�K�[���骺�I���˴��޿�A�Ҧp�G
        Collider[] hitColliders = Physics.OverlapSphere(leftHandTransform.position, 0.1f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Climbable")) // ���ݳ]�mTag��"Ladder"
                return true;
        }
        return false;
    }
}
