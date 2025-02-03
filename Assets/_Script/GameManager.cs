using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this for scene management

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //about Canvas
    [SerializeField] private Canvas chargeCompleteCanvas;
    [SerializeField] private Canvas waterLevelDownCanvas;
    [SerializeField] private Canvas playerTouchWater;
    [SerializeField] private TextMeshProUGUI energyCounterText; // Update TextMeshProUGUI

    //Energy count
    [SerializeField] private int maxEnergy = 2;

    [SerializeField] private GameObject gate; // Reference to the gate GameObject
    public bool gateChargeMax;//testing button

    public bool isLazerActive = false;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private float chargeTime = 1f;
    public bool chargeCanvasDisplayed = false;
    public int totalEnergy = 0;
    public Material chargedMaterial; // Material to switch on charge
    public Material gateOpen;

    //Animation
    [SerializeField] private Transform waterObject;
    [SerializeField] private GameObject player;

    //moving
    public Transform respawnPoint; // 玩家重生點
    //private bool canMove = true;


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
        //canMove = true;
        gateChargeMax=false;
        chargeCompleteCanvas.enabled = false;
        waterLevelDownCanvas.enabled = false;
        playerTouchWater.enabled = false;
        UpdateEnergyCounterUI(); // 初始化顯示
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
                RegisterCharge(); // 將註冊充能移到這裡
                isCharging = false;
                chargeTimer = 0f;
            }
        }

        // Check if the gateChargeMax condition is true
        if (gateChargeMax)
        {
            // Change the gate material to gateOpen
            gate.GetComponent<Renderer>().material = gateOpen; // Ensure gate has a Renderer component
        }
    }

    private void ShowChargeCompleteCanvas(float duration = 1f)
    {
        chargeCompleteCanvas.enabled = true;
        chargeCanvasDisplayed = true;
        Invoke("HideChargeCompleteCanvas", duration);
    }

    private void HideChargeCompleteCanvas()
    {
        chargeCompleteCanvas.enabled = false;
        chargeCanvasDisplayed = false; // Reset canvas display state
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
            isCharging = false; // Stop charging
            gateChargeMax = true; // Set gate charge max to true
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
        StartCoroutine(WaitAndShowCanvas(1f));
    }

    private IEnumerator WaitAndShowCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowWaterLevelDownCanvas();
    }

    public void ShowWaterLevelDownCanvas(float duration = 1f)
    {
        waterLevelDownCanvas.enabled = true;
        Invoke("HideWaterLevelDownCanvas", duration);
    }

    private void HideWaterLevelDownCanvas()
    {
        waterLevelDownCanvas.enabled = false;
    }

    public void ToggleLaserAndFlashlight()
    {
        isLazerActive = !isLazerActive;
    }

    public void PlayerTouchWater()
    {
        playerTouchWater.enabled = true;
        Animator touchWater= player.GetComponent<Animator>();

        if (touchWater != null)
        {
            touchWater.SetTrigger("touchWater"); // 觸發TouchWater動畫
            Debug.Log("Player animation triggered");
        }

        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation; // 可選：重設旋轉
            Debug.Log("Player respawned at " + respawnPoint.position);
        }
        else
        {
            Debug.LogError("Respawn point is not set!");
        }
    }
}
