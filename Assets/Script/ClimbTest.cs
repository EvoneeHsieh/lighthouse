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
        rb = GetComponent<Rigidbody>(); // 使用 Rigidbody 替代 CharacterController
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (currentHand != null)
        {
            CalculateMovement(); // 根據手的移動量計算攀爬
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
            movement.y -= gravity * Time.deltaTime; // 模擬重力
        }

        rb.MovePosition(rb.position + movement * Time.deltaTime); // 基於 Rigidbody 移動
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
