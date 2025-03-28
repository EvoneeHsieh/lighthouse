using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.VFX;

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

    //特效
    [Header("VFX")]
    public VisualEffect lightningVFX; // 把你的閃電特效拖進 Inspector
    public string lightningDirectionParam = "LightningDirection"; // 這是 VFX Graph 中的參數名

    private void Start()
    {
        aimHint.enabled = true;
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1; // Initialize position count
        lr.SetPosition(0, startPoint.position);
        lr.enabled = false; // Initially disable the laser
        playerCamera = Camera.main; // Get the main camera reference
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

        // 開啟或關閉 VFX 特效顯示
        if (lightningVFX != null)
        {
            if (isLaserOn)
                lightningVFX.Play();
            else
                lightningVFX.Stop();
        }
    }

    void UpdateLaser()
    {
        Vector3 direction = playerCamera.transform.forward;

        if (lightningVFX != null && lightningVFX.HasVector3(lightningDirectionParam))
        {
            lightningVFX.SetVector3(lightningDirectionParam, direction);
        }

        CastLaser(startPoint.position, direction);
    }

    public void CastLaser(Vector3 position, Vector3 direction)
    {
        if (!GameManager.instance.isLazerActive) return; // 如果雷射關閉，直接返回

        lr.positionCount = 1;
        lr.SetPosition(0, startPoint.position);
        float totalDistance = 0f;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxLaserLength - totalDistance))
            {
                float distance = Vector3.Distance(position, hit.point);
                totalDistance += distance;

                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, hit.point);
                position = hit.point;

                if (hit.transform.CompareTag("EnergyCharge"))
                {
                    ChargeManager chargeManager = hit.transform.GetComponent<ChargeManager>();
                    if (chargeManager != null)
                    {
                        GameManager.instance.StartCharging(chargeManager);
                    }
                    else
                    {
                        Debug.LogError("ChargeManager not found on the EnergyCharge object.");
                    }
                }
                else if (hit.transform.CompareTag("waterLevel"))
                {
                    GameManager.instance.HitWaterLevel();
                }

                if (hit.transform.CompareTag("Mirror"))
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                }
                else
                {
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
