using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Lazer : MonoBehaviour
{
    public float maxLaserLength = 50f;
    public int maxBounces = 5; // 最大反彈次數
    public LineRenderer lr;
    [SerializeField] private Transform startPoint; // Assign this in the inspector
    [SerializeField] private Camera playerCamera; // Main camera reference

    private bool isLaserOn = false;
    public InputActionReference inputAction;

    public Canvas aimHint;
    private ChargeManager currentChargeManager;
    private void Start()
    {
        aimHint.enabled = true;
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1; // Initialize position count
        lr.SetPosition(0, startPoint.position);
        lr.enabled = false; // Initially disable the laser
        playerCamera = Camera.main; // Get the main camera reference
        //inputData=GetComponent<InputData>();
    }

    void Update()
    {
        // VR 控制器按鈕輸入
        if (inputAction.action.WasPressedThisFrame())
        {
            Debug.Log("VR 控制器按鈕按下");
            ToggleLaser();
        }

        // 鍵盤按鍵輸入 (Q 鍵)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q 鍵按下");
            ToggleLaser();
        }

        if (isLaserOn)
        {
            UpdateLaser();
        }
    }
    void ToggleLaser()
    {
        isLaserOn = !isLaserOn;
        lr.enabled = isLaserOn;
        GameManager.instance.isLazerActive = isLaserOn; // 讓 GameManager 的狀態同步
        //Debug.Log("雷射狀態切換：" + (isLaserOn ? "開啟" : "關閉") + "，lr.enabled = " + lr.enabled);
    }

    void UpdateLaser()
    {
        // ... 更新雷射的邏輯 ...
        Vector3 direction = playerCamera.transform.forward;
        CastLaser(startPoint.position, direction);
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
                lr.SetPosition(i + 1, position + direction * (maxLaserLength - totalDistance));
                break;
            }
        }
    }
}
