using UnityEngine;

public class ClimbControl : MonoBehaviour
{
    public bool isClimbing = false;
    private GameObject climbableObject;
    private GameObject grabbingObject;
    public Rigidbody playerRigidbody;

    public OVRGrabber leftOVRGrabber;  // ���⪺ OVRGrabber
    public OVRGrabber rightOVRGrabber; // �k�⪺ OVRGrabber

    private Vector3 previousRightHandPosition;
    private Vector3 previousLeftHandPosition;
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            // �T���M�k��������
            leftOVRGrabber.m_parentHeldObject = false;
            rightOVRGrabber.m_parentHeldObject = false;

            climbableObject = other.gameObject;
            Debug.Log("Entered climbable object: " + other.gameObject.name);

            // �P�_�I�쪺�O������A�öi��_��
            if (other.transform.IsChildOf(leftHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: true, isRightHand: false);  // �u������_��
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: false, isRightHand: true);  // �u���k��_��
            }
        }

        if (other.CompareTag("Grabbing"))
        {
            // �ҥΧ������A�T�Ϊ���
            if (other.transform.IsChildOf(leftHandTransform))
            {
                leftOVRGrabber.m_parentHeldObject = true; // ���\������
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                rightOVRGrabber.m_parentHeldObject = true; // ���\�k����
            }

            grabbingObject = other.gameObject;
            Debug.Log("Entered Grabbing object: " + other.gameObject.name);

            // �P�_�I�쪺�O������A�öi��_��
            if (other.transform.IsChildOf(leftHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: true, isRightHand: false);  // �u������_��
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: false, isRightHand: true);  // �u���k��_��
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable") || other.CompareTag("Grabbing"))
        {
            climbableObject = null;
            isClimbing = false;

            // �h�X����Χ���ϰ�ɡA�T�Χ���\��
            if (other.CompareTag("Grabbing"))
            {
                leftOVRGrabber.m_parentHeldObject = false;
                rightOVRGrabber.m_parentHeldObject = false;
            }
        }
    }

    private void Update()
    {
        // �T�{������s�����A�A���O������M�k��
        float leftGripButton = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger); // ����������
        float rightGripButton = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger); // �k��������

        if ((leftGripButton > 0.5f || rightGripButton > 0.5f) && climbableObject != null)
        {
            StartClimbing();
        }
        else
        {
            StopClimbing();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            Debug.Log("can climb");
            Vector3 rightHandMovement = previousRightHandPosition - rightHandTransform.position;
            Vector3 leftHandMovement = previousLeftHandPosition - leftHandTransform.position;

            Vector3 handMovement = (rightHandMovement + leftHandMovement) * 0.5f;
            playerRigidbody.velocity = handMovement * 10f;

            previousRightHandPosition = rightHandTransform.position;
            previousLeftHandPosition = leftHandTransform.position;
        }
    }

    private void StartClimbing()//can use
    {
        isClimbing = true;
        previousRightHandPosition = rightHandTransform.position;
        previousLeftHandPosition = leftHandTransform.position;
        playerRigidbody.useGravity = false;
    }

    private void StopClimbing()//can use
    {
        isClimbing = false;
        playerRigidbody.useGravity = true;
        playerRigidbody.velocity = Vector3.zero;
    }

    private void TriggerHapticFeedback(bool isLeftHand, bool isRightHand)
    {
        int intensity = 255;  // �̤j�j��
        int duration = 100;   // �_�ʮɶ�

        // �Ыؾ_�ʰT��
        OVRHapticsClip hapticsClip = new OVRHapticsClip(duration);
        for (int i = 0; i < hapticsClip.Samples.Length; i++)
        {
            hapticsClip.Samples[i] = (byte)intensity;
        }

        // �ھڰѼƨ�Ĳ�o�_��
        if (isLeftHand)
        {
            Debug.Log("Left hand haptics triggered.");
            OVRHaptics.LeftChannel.Preempt(hapticsClip);  // Ĳ�o����_��
        }

        if (isRightHand)
        {
            Debug.Log("Right hand haptics triggered.");
            OVRHaptics.RightChannel.Preempt(hapticsClip);  // Ĳ�o�k��_��
        }
    }
}
