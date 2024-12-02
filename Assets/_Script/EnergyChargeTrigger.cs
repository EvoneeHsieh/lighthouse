using UnityEngine;

public class EnergyChargeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 確保這是雷射
        {
            ChargeManager chargeManager = GetComponent<ChargeManager>();

            if (chargeManager != null && !chargeManager.isCharged) // 檢查是否尚未充能
            {
                GameManager.instance.StartCharging(chargeManager); // 觸發充能
            }
        }
    }

}
