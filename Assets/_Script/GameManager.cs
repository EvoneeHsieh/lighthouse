using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // UI Canvas Groups
    [SerializeField] private CanvasGroup chargeCompleteCanvas;
    [SerializeField] private CanvasGroup waterLevelDownCanvas;
    [SerializeField] private CanvasGroup playerTouchWaterCanvas;
    [SerializeField] private TextMeshProUGUI energyCounterText;

    // Energy count
    [SerializeField] private int maxEnergy = 2;
    [SerializeField] private GameObject gate;
    public bool gateChargeMax;

    public bool isLazerActive = false;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private float chargeTime = 1f;
    public bool chargeCanvasDisplayed = false;
    public int totalEnergy = 0;
    public Material chargedMaterial;
    public Material gateOpen;

    // Animation
    [SerializeField] private Transform waterObject;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject doorObject;

    // Moving
    public Transform respawnPoint;
    private ChargeManager currentChargeManager;

    // 測試變數：手動觸發開門動畫
    public bool testDoorOpen = false;

    private void Awake()
    {
        instance = this; // 沒有 Singleton 檢查，也不跨場景
    }

    private void Start()
    {
        isCharging = false;
        gateChargeMax = false;
        SetCanvasGroupVisibility(chargeCompleteCanvas, false);
        SetCanvasGroupVisibility(waterLevelDownCanvas, false);
        SetCanvasGroupVisibility(playerTouchWaterCanvas, false);
        UpdateEnergyCounterUI();
    }

    private void Update()
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= chargeTime && !chargeCanvasDisplayed)
            {
                ShowChargeCompleteCanvas();
                RegisterCharge();
                isCharging = false;
                chargeTimer = 0f;
            }
        }

        // 測試：如果 `testDoorOpen` 被啟動，則開門
        if (testDoorOpen)
        {
            testDoorOpen = false; // 只執行一次
            OpenGate();
        }
    }

    public void StartCharging(ChargeManager chargeManager)
    {
        if (isCharging || chargeManager.isCharged) return;

        currentChargeManager = chargeManager;
        isCharging = true;
        chargeTimer = 0f;
    }

    private void RegisterCharge()
    {
        if (currentChargeManager != null && !currentChargeManager.isCharged)
        {
            currentChargeManager.SetCharged(chargedMaterial);
            totalEnergy++;
            UpdateEnergyCounterUI();
        }

        if (totalEnergy >= maxEnergy)
        {
            Debug.Log("Max Energy");
            isCharging = false;
            gateChargeMax = true;
            OpenGate();
        }
    }

    private void OpenGate()
    {
        // 觸發門的動畫
        Animator gateOpenAni = doorObject.GetComponent<Animator>();
        if (gateOpenAni != null)
        {
            gateOpenAni.SetTrigger("DoorOpenAni");
        }
        else
        {
            Debug.LogError("門沒有 Animator 組件！");
        }

        // 改變門的材質
        if (gate != null)
        {
            Renderer gateRenderer = gate.GetComponent<Renderer>();
            if (gateRenderer != null)
            {
                gateRenderer.material = gateOpen;
            }
        }
    }

    private void UpdateEnergyCounterUI()
    {
        if (energyCounterText != null)
            energyCounterText.text = $"充電({totalEnergy}/{maxEnergy})";
    }

    public void HitWaterLevel()
    {
        Animator waterAnimator = waterObject.GetComponent<Animator>();
        if (waterAnimator != null)
        {
            waterAnimator.SetTrigger("StartLowering");
        }
        StartCoroutine(WaitAndShowCanvas(waterLevelDownCanvas, 1f));
    }

    private IEnumerator WaitAndShowCanvas(CanvasGroup canvasGroup, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetCanvasGroupVisibility(canvasGroup, true);
        Invoke("HideWaterLevelDownCanvas", 1f);
    }

    public void ShowWaterLevelDownCanvas(float duration = 1f)
    {
        SetCanvasGroupVisibility(waterLevelDownCanvas, true);
        Invoke("HideWaterLevelDownCanvas", duration);
    }

    private void HideWaterLevelDownCanvas()
    {
        SetCanvasGroupVisibility(waterLevelDownCanvas, false);
    }

    public void ToggleLaserAndFlashlight()
    {
        isLazerActive = !isLazerActive;
    }

    public void PlayerTouchWater()
    {
        SetCanvasGroupVisibility(playerTouchWaterCanvas, true);
        Invoke("HidePlayerTouchWaterCanvas", 2f);

        if (player != null)
        {
            Animator touchWater = player.GetComponent<Animator>();
            if (touchWater != null)
            {
                touchWater.SetTrigger("touchWater");
                Debug.Log("Player animation triggered");
                StartCoroutine(ResetTouchWaterTrigger(touchWater));
            }
            else
            {
                Debug.LogWarning("Player does not have an Animator component! Skipping animation.");
            }

            if (respawnPoint != null)
            {
                player.transform.position = respawnPoint.position;
                player.transform.rotation = respawnPoint.rotation;
                Debug.Log("Player respawned at " + respawnPoint.position);
            }
            else
            {
                Debug.LogError("Respawn point is not set!");
            }
        }
        else
        {
            Debug.LogError("Player reference is missing in GameManager!");
        }
    }


    private void HidePlayerTouchWaterCanvas()
    {
        SetCanvasGroupVisibility(playerTouchWaterCanvas, false);
    }

    private IEnumerator ResetTouchWaterTrigger(Animator animator)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.ResetTrigger("touchWater");
        Debug.Log("touchWater trigger reset");
        SetCanvasGroupVisibility(playerTouchWaterCanvas, false);
    }

    private void SetCanvasGroupVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    private void ShowChargeCompleteCanvas(float duration = 1f)
    {
        SetCanvasGroupVisibility(chargeCompleteCanvas, true);
        chargeCanvasDisplayed = true;
        Invoke("HideChargeCompleteCanvas", duration);
    }

    private void HideChargeCompleteCanvas()
    {
        SetCanvasGroupVisibility(chargeCompleteCanvas, false);
        chargeCanvasDisplayed = false;
    }
}
