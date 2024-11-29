using UnityEngine;

public class ClimbControl : MonoBehaviour
{
    public bool isClimbing = false;
    private GameObject climbableObject;
    private GameObject grabbingObject;
    public Rigidbody playerRigidbody;

    public OVRGrabber leftOVRGrabber;  // 左手的 OVRGrabber
    public OVRGrabber rightOVRGrabber; // 右手的 OVRGrabber

    private Vector3 previousRightHandPosition;
    private Vector3 previousLeftHandPosition;
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            // 禁止左手和右手抓取物體
            leftOVRGrabber.m_parentHeldObject = false;
            rightOVRGrabber.m_parentHeldObject = false;

            climbableObject = other.gameObject;
            Debug.Log("Entered climbable object: " + other.gameObject.name);

            // 判斷碰到的是哪隻手，並進行震動
            if (other.transform.IsChildOf(leftHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: true, isRightHand: false);  // 只有左手震動
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: false, isRightHand: true);  // 只有右手震動
            }
        }

        if (other.CompareTag("Grabbing"))
        {
            // 啟用抓取物體，禁用爬梯
            if (other.transform.IsChildOf(leftHandTransform))
            {
                leftOVRGrabber.m_parentHeldObject = true; // 允許左手抓取
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                rightOVRGrabber.m_parentHeldObject = true; // 允許右手抓取
            }

            grabbingObject = other.gameObject;
            Debug.Log("Entered Grabbing object: " + other.gameObject.name);

            // 判斷碰到的是哪隻手，並進行震動
            if (other.transform.IsChildOf(leftHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: true, isRightHand: false);  // 只有左手震動
            }
            else if (other.transform.IsChildOf(rightHandTransform))
            {
                TriggerHapticFeedback(isLeftHand: false, isRightHand: true);  // 只有右手震動
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable") || other.CompareTag("Grabbing"))
        {
            climbableObject = null;
            isClimbing = false;

            // 退出爬梯或抓取區域時，禁用抓取功能
            if (other.CompareTag("Grabbing"))
            {
                leftOVRGrabber.m_parentHeldObject = false;
                rightOVRGrabber.m_parentHeldObject = false;
            }
        }
    }

    private void Update()
    {
        // 確認抓取按鈕的狀態，分別為左手和右手
        float leftGripButton = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger); // 左手抓取按鍵
        float rightGripButton = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger); // 右手抓取按鍵

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
        int intensity = 255;  // 最大強度
        int duration = 100;   // 震動時間

        // 創建震動訊號
        OVRHapticsClip hapticsClip = new OVRHapticsClip(duration);
        for (int i = 0; i < hapticsClip.Samples.Length; i++)
        {
            hapticsClip.Samples[i] = (byte)intensity;
        }

        // 根據參數來觸發震動
        if (isLeftHand)
        {
            Debug.Log("Left hand haptics triggered.");
            OVRHaptics.LeftChannel.Preempt(hapticsClip);  // 觸發左手震動
        }

        if (isRightHand)
        {
            Debug.Log("Right hand haptics triggered.");
            OVRHaptics.RightChannel.Preempt(hapticsClip);  // 觸發右手震動
        }
    }
}
