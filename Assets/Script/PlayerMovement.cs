using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // 移動速度
    public float mouseSensitivity = 100f;
    public float jumpForce = 5f; // 跳躍力量
    public LayerMask groundLayer; // 地面層的 LayerMask，用於檢測角色是否在地面上

    public Transform playerBody; // 玩家身體
    public Transform cameraTransform; // 攝像機

    private Rigidbody rb; // Rigidbody 參考
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止物理旋轉影響角色

        Cursor.lockState = CursorLockMode.Locked; // 鎖定滑鼠
    }

    void Update()
    {
        // 旋轉攝像機
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制垂直視角角度

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // 控制視角上下旋轉
        playerBody.Rotate(Vector3.up * mouseX); // 控制角色左右旋轉

        // 角色跳躍判斷
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 使用攝像機的前後左右方向移動
        float x = Input.GetAxis("Horizontal"); // A/D 或 左右鍵
        float z = Input.GetAxis("Vertical");   // W/S 或 上下鍵

        // 計算移動方向
        Vector3 moveDirection = (cameraTransform.forward * z + cameraTransform.right * x).normalized;
        moveDirection.y = 0f; // 防止角色因攝像機傾斜而在Y軸移動

        // 使用 Rigidbody 進行移動
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    // 判斷角色是否在地面上
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    // 跳躍方法
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 向上施加力使角色跳起
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name); // 檢查碰撞的物件名稱
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
