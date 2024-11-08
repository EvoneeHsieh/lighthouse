using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float maxLaserLength = 50f;
    public int maxBounces = 5; // 最大反彈次數
    public LineRenderer lr;
    [SerializeField] private Transform startPoint; // Assign this in the inspector
    [SerializeField] private Camera playerCamera; // Main camera reference

    private bool isLaserTrackingActive = false;

    private ChargeManager currentChargeManager;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1; // Initialize position count
        lr.SetPosition(0, startPoint.position);
        lr.enabled = false; // Initially disable the laser
        playerCamera = Camera.main; // Get the main camera reference
    }

    void Update()
    {
        // Q 鍵切換雷射和手電筒
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.ToggleLaserAndFlashlight();
            lr.enabled = GameManager.instance.isLazerActive;

            // 如果雷射被啟動，啟動方向追蹤
            isLaserTrackingActive = GameManager.instance.isLazerActive;

            // 按下Q鍵時立即射出雷射
            if (isLaserTrackingActive)
            {
                Vector3 direction = playerCamera.transform.forward;
                CastLaser(startPoint.position, direction);
            }
        }

        // 當isLaserTrackingActive為true時才會在每幀更新雷射方向
        if (isLaserTrackingActive)
        {
            Vector3 direction = playerCamera.transform.forward;
            CastLaser(startPoint.position, direction);
        }
    }


    public void CastLaser(Vector3 position, Vector3 direction)
    {
        if (!GameManager.instance.isLazerActive) return; // 如果雷射關閉，直接返回

        lr.positionCount = 1; // +1 是為了包含起始點
        lr.SetPosition(0, startPoint.position);
        float totalDistance = 0f;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxLaserLength - totalDistance))
            {
                float distance = Vector3.Distance(position, hit.point);
                totalDistance += distance; // 計算總長度

                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, hit.point);
                position = hit.point;

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
