using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Lazer : MonoBehaviour
{
    public float maxLaserLength = 50f;
    public int maxBounces = 5; // �̤j�ϼu����
    public LineRenderer lr;
    [SerializeField] private Transform startPoint; // Assign this in the inspector
    [SerializeField] private Camera playerCamera; // Main camera reference

    private bool isLaserOn = false;
    public InputActionReference inputAction;


    private ChargeManager currentChargeManager;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1; // Initialize position count
        lr.SetPosition(0, startPoint.position);
        lr.enabled = false; // Initially disable the laser
        playerCamera = Camera.main; // Get the main camera reference
        //inputData=GetComponent<InputData>();
    }

    void Update()
    {
        if (inputAction.action.WasPressedThisFrame())
        {
            Debug.Log("AAAAAAAAAA press");
            isLaserOn = !isLaserOn;
            lr.enabled = isLaserOn;
        }

        if (isLaserOn)
        {
            UpdateLaser();
        }
    }
    void UpdateLaser()
    {
        // ... ��s�p�g���޿� ...
        Vector3 direction = playerCamera.transform.forward;
        CastLaser(startPoint.position, direction);
    }

    public void CastLaser(Vector3 position, Vector3 direction)
    {
        if (!GameManager.instance.isLazerActive) return; // �p�G�p�g�����A������^

        lr.positionCount = 1; // +1 �O���F�]�t�_�l�I
        lr.SetPosition(0, startPoint.position);
        float totalDistance = 0f;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxLaserLength - totalDistance))
            {
                float distance = Vector3.Distance(position, hit.point);
                totalDistance += distance; // �p���`����

                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, hit.point);
                position = hit.point;

                // �p�G�I��S�w����A��������ާ@
                if (hit.transform.CompareTag("EnergyCharge"))
                {
                    ChargeManager chargeManager = hit.transform.GetComponent<ChargeManager>(); // ��� EnergyCharge �� ChargeManager �ե�
                    if (chargeManager != null)
                    {
                        GameManager.instance.StartCharging(chargeManager); // �ǻ������ ChargeManager ���
                    }
                    else
                    {
                        Debug.LogError("ChargeManager not found on the EnergyCharge object.");
                    }
                }

                else if (hit.transform.CompareTag("waterLevel"))
                {
                    GameManager.instance.HitWaterLevel(); // Ĳ�o�����ܤ�
                }

                // �p�G�I�쪺�O Mirror�A�~��Ϯg
                if (hit.transform.CompareTag("Mirror"))
                {
                    direction = Vector3.Reflect(direction, hit.normal); // �u�� Mirror �~�Ϯg�p�g
                }
                else
                {
                    // �p�G�I�쪺���O Mirror�A����Ϯg
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
