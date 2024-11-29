using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTest : MonoBehaviour
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    private Rigidbody playerRigidbody;

    public float climbSpeed = 2f; // 攀爬速度
    private bool isClimbing = false;

    private Vector3 previousLeftHandPosition;
    private Vector3 previousRightHandPosition;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 判斷手是否抓住梯子（可根據按鍵觸發）
        if (IsHandOnLadder() && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) // 抓取按鈕
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
        playerRigidbody.useGravity = false; // 禁用重力
    }

    private void StopClimbing()
    {
        isClimbing = false;
        playerRigidbody.useGravity = true; // 恢復重力
    }

    private void ClimbMovement()
    {
        // 計算手的移動量
        Vector3 leftHandDelta = leftHandTransform.position - previousLeftHandPosition;
        Vector3 rightHandDelta = rightHandTransform.position - previousRightHandPosition;

        // 根據手的移動方向，讓玩家沿Y軸移動
        Vector3 climbMovement = new Vector3(0, (leftHandDelta.y + rightHandDelta.y) * climbSpeed, 0);

        transform.position += climbMovement;

        // 更新手的位置
        previousLeftHandPosition = leftHandTransform.position;
        previousRightHandPosition = rightHandTransform.position;
    }

    private bool IsHandOnLadder()
    {
        // 這裡可以添加具體的碰撞檢測邏輯，例如：
        Collider[] hitColliders = Physics.OverlapSphere(leftHandTransform.position, 0.1f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Climbable")) // 橫桿需設置Tag為"Ladder"
                return true;
        }
        return false;
    }
}
