using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Canvas chargeCompleteCanvas;
    [SerializeField] private Canvas waterLevelDownCanvas;

    public bool isLazerActive = true; // ��������p�g���}��

    private float chargeTimer = 0f;
    private bool isCharging = false; // �O�_���b�R��
    private float chargeTime = 1f; // �R��һݮɶ�
    private bool chargeCanvasDisplayed = false; // �аO�O�_��ܤF�R��Canvas

    [SerializeField] private Transform WaterObject; // ���쪺Plane����

    private bool isWaterLevelHit = false; // �O�_�I����쪫��

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // �O�ҳ�ҼҦ�
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
        isWaterLevelHit = true; // �аO����Q����

        // ������쪫��Animator�ե��Ĳ�o�ʵe
        Animator waterAnimator = WaterObject.GetComponent<Animator>();
        if (waterAnimator != null)
        {
            waterAnimator.SetTrigger("StartLowering"); // �]�wĲ�o����
        }

        // ����1��AĲ�o�ʵe
        StartCoroutine(WaitAndShowCanvas(1f));
    }

    private IEnumerator WaitAndShowCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowWaterLevelDownCanvas(); // ��ܤ���U����Canvas
    }


    private void Update()
    {
        // �B�z�R���޿�
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            Debug.Log("Charge Timer: " + chargeTimer);

            if (chargeTimer >= chargeTime && !chargeCanvasDisplayed)
            {
                ShowChargeCompleteCanvas(); // ��ܥR�৹����Canvas
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

    // �b�o�̥ΰʵe�ƥ����ܤ���U����Canvas
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

    // �����p�g�M��q��
    public void ToggleLaserAndFlashlight()
    {
        isLazerActive = !isLazerActive;
    }
}
