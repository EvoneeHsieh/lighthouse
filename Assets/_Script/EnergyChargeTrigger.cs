using UnityEngine;

public class EnergyChargeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �T�O�o�O�p�g
        {
            ChargeManager chargeManager = GetComponent<ChargeManager>();

            if (chargeManager != null && !chargeManager.isCharged) // �ˬd�O�_�|���R��
            {
                GameManager.instance.StartCharging(chargeManager); // Ĳ�o�R��
            }
        }
    }

}
