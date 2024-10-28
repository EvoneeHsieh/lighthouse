using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float maxLaserLength = 50f;
    public int maxBounces = 5; // 最大反彈次數
    private LineRenderer lr;
    [SerializeField] private Transform startPoint; // Assign this in the inspector
    [SerializeField] private Camera playerCamera; // Main camera reference

    private ChargeManager currentChargeManager;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1; // Initialize position count
        lr.SetPosition(0, startPoint.position);
        lr.enabled = false; // Initially disable the laser
        playerCamera = Camera.main; // Get the main camera reference
    }

    private void Update()
    {
        // Q 鍵切換雷射和手電筒
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.ToggleLaserAndFlashlight();
            lr.enabled = GameManager.instance.isLazerActive;
        }

        // 在這裡調用雷射發射的方法
        if (GameManager.instance.isLazerActive)
        {
            Vector3 direction = playerCamera.transform.forward; // 將雷射方向設為相機面對的方向
            CastLaser(startPoint.position, direction);
        }
    }

    public void CastLaser(Vector3 position, Vector3 direction)
    {
        if (!GameManager.instance.isLazerActive) return; // 如果雷射關閉，直接返回

        lr.positionCount = maxBounces + 1; // +1 是為了包含起始點
        lr.SetPosition(0, startPoint.position);
        float totalDistance = 0f;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300))
            {
                float distance = Vector3.Distance(position, hit.point);
                totalDistance += distance; // 計算總長度

                if (totalDistance > maxLaserLength) // 如果超過最大長度
                {
                    Vector3 adjustedPoint = position + direction.normalized * (maxLaserLength - (totalDistance - distance));
                    lr.positionCount = i + 2; // 更新折線點數量
                    lr.SetPosition(i + 1, adjustedPoint);
                    break;
                }

                position = hit.point;
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, hit.point);

                // 如果碰到特定物件，執行相應操作
                if (hit.transform.CompareTag("EnergyCharge"))
                {
                    ChargeManager chargeManager = hit.transform.GetComponent<ChargeManager>(); // 獲取 EnergyCharge 的 ChargeManager 組件
                    if (chargeManager != null)
                    {
                        GameManager.instance.StartCharging(chargeManager); // 傳遞獲取的 ChargeManager 實例
                    }
                    else
                    {
                        Debug.LogError("ChargeManager not found on the EnergyCharge object.");
                    }
                }

                else if (hit.transform.CompareTag("waterLevel"))
                {
                    GameManager.instance.HitWaterLevel(); // 觸發水位變化
                }

                // 如果碰到的是 Mirror，繼續反射
                if (hit.transform.CompareTag("Mirror"))
                {
                    direction = Vector3.Reflect(direction, hit.normal); // 只有 Mirror 才反射雷射
                }
                else
                {
                    // 如果碰到的不是 Mirror，停止反射
                    break;
                }
            }
            else
            {
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, position);
                break;
            }
        }
    }
}
