using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float maxLaserLength = 50f;
    public int maxBounces = 5; // �̤j�ϼu����
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
        // Q ������p�g�M��q��
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.ToggleLaserAndFlashlight();
            lr.enabled = GameManager.instance.isLazerActive;
        }

        // �b�o�̽եιp�g�o�g����k
        if (GameManager.instance.isLazerActive)
        {
            Vector3 direction = playerCamera.transform.forward; // �N�p�g��V�]���۾����諸��V
            CastLaser(startPoint.position, direction);
        }
    }

    public void CastLaser(Vector3 position, Vector3 direction)
    {
        if (!GameManager.instance.isLazerActive) return; // �p�G�p�g�����A������^

        lr.positionCount = maxBounces + 1; // +1 �O���F�]�t�_�l�I
        lr.SetPosition(0, startPoint.position);
        float totalDistance = 0f;

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300))
            {
                float distance = Vector3.Distance(position, hit.point);
                totalDistance += distance; // �p���`����

                if (totalDistance > maxLaserLength) // �p�G�W�L�̤j����
                {
                    Vector3 adjustedPoint = position + direction.normalized * (maxLaserLength - (totalDistance - distance));
                    lr.positionCount = i + 2; // ��s��u�I�ƶq
                    lr.SetPosition(i + 1, adjustedPoint);
                    break;
                }

                position = hit.point;
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, hit.point);

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
