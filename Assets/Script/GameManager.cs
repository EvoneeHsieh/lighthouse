using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Canvas chargeCompleteCanvas;
    [SerializeField] private Canvas waterLevelDownCanvas;

    public bool isLazerActive = true; // 全局控制雷射的開關

    private float chargeTimer = 0f;
    private bool isCharging = false; // 是否正在充能
    private float chargeTime = 1f; // 充能所需時間
    private bool chargeCanvasDisplayed = false; // 標記是否顯示了充能Canvas

    [SerializeField] private Transform WaterObject; // 水位的Plane物件

    private bool isWaterLevelHit = false; // 是否碰到水位物體

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 保證單例模式
        }
    }

    private void Start()
    {
        chargeCompleteCanvas.enabled = false;
        waterLevelDownCanvas.enabled = false;
    }

    public void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0f;
        chargeCanvasDisplayed = false;
    }

    public void HitWaterLevel()
    {
        Debug.Log("hitwaterlevel");
        isWaterLevelHit = true; // 標記水位被擊中

        // 獲取水位物件的Animator組件並觸發動畫
        Animator waterAnimator = WaterObject.GetComponent<Animator>();
        if (waterAnimator != null)
        {
            waterAnimator.SetTrigger("StartLowering"); // 設定觸發條件
        }

        // 等待1秒再觸發動畫
        StartCoroutine(WaitAndShowCanvas(1f));
    }

    private IEnumerator WaitAndShowCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowWaterLevelDownCanvas(); // 顯示水位下降的Canvas
    }


    private void Update()
    {
        // 處理充能邏輯
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            Debug.Log("Charge Timer: " + chargeTimer);

            if (chargeTimer >= chargeTime && !chargeCanvasDisplayed)
            {
                ShowChargeCompleteCanvas(); // 顯示充能完成的Canvas
                chargeCanvasDisplayed = true;
                isCharging = false;
                chargeTimer = 0f;
            }
        }
    }

    public void ShowChargeCompleteCanvas(float duration = 1f)
    {
        chargeCompleteCanvas.enabled = true;
        Invoke("HideChargeCompleteCanvas", duration);
    }

    // 在這裡用動畫事件來顯示水位下降的Canvas
    public void ShowWaterLevelDownCanvas(float duration = 1f)
    {
        waterLevelDownCanvas.enabled = true;
        Invoke("HideWaterLevelDownCanvas", duration);
    }

    private void HideChargeCompleteCanvas()
    {
        chargeCompleteCanvas.enabled = false;
    }

    private void HideWaterLevelDownCanvas()
    {
        waterLevelDownCanvas.enabled = false;
    }

    // 切換雷射和手電筒
    public void ToggleLaserAndFlashlight()
    {
        isLazerActive = !isLazerActive;
    }
}
