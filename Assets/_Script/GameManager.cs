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
    [SerializeField] private TextMeshProUGUI energyCounterText; // 這個不需要 CanvasGroup

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

    // Moving
    public Transform respawnPoint;
    private ChargeManager currentChargeManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gateChargeMax = false;
        SetCanvasGroupVisibility(chargeCompleteCanvas, false);
        SetCanvasGroupVisibility(waterLevelDownCanvas, false);
        SetCanvasGroupVisibility(playerTouchWaterCanvas, false);
        UpdateEnergyCounterUI();
    }

    public void StartCharging(ChargeManager chargeManager)
    {
        if (isCharging || chargeManager.isCharged) return;

        currentChargeManager = chargeManager;
        isCharging = true;
        chargeTimer = 0f;
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

        if (gateChargeMax)
        {
            gate.GetComponent<Renderer>().material = gateOpen;
        }
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
        }
    }

    private void UpdateEnergyCounterUI()
    {
        energyCounterText.text = $"Energy({totalEnergy}/{maxEnergy})";
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
        Invoke("HidePlayerTouchWaterCanvas", 2f); // 確保 HidePlayerTouchWaterCanvas 存在

        Animator touchWater = player.GetComponent<Animator>();

        if (touchWater != null)
        {
            touchWater.SetTrigger("touchWater");
            Debug.Log("Player animation triggered");
            StartCoroutine(ResetTouchWaterTrigger(touchWater));
        }
        else
        {
            Debug.LogError("Player does not have an Animator component!");
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
}
