using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f; // 移動速度
    public Transform cameraTransform; // 攝像機（代表頭部方向）
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止物理旋轉
    }

    void FixedUpdate()
    {
        // 獲取右搖桿的輸入
        float inputX = Input.GetAxis("Horizontal"); // 右搖桿 X 軸 (左右移動)
        float inputZ = Input.GetAxis("Vertical");   // 右搖桿 Y 軸 (前後移動)

        // 計算頭部的朝向（忽略垂直方向）
        Vector3 headForward = cameraTransform.forward;
        headForward.y = 0f; // 忽略頭部的俯仰角度
        headForward.Normalize();

        Vector3 headRight = cameraTransform.right;
        headRight.y = 0f; // 忽略垂直方向
        headRight.Normalize();

        // 根據輸入計算移動方向
        Vector3 moveDirection = (headForward * inputZ + headRight * inputX).normalized;

        // 平移玩家位置
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
