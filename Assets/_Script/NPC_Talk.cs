using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    public Canvas dialog; // UI ��ܮ�
    public TextMeshProUGUI textBox; // ��r��
    [TextArea(3, 5)] public string npcText; // �o�̥i�H�bInspector��������J�奻
    public InputActionReference talking;

    private void Start()
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false); // �C���}�l�ɥ����ù�ܮ�
        }

        if (textBox != null)
        {
            textBox.text = ""; // �T�O�}���ɨS���ݯd��r
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(talking.action.WasPressedThisFrame())
            StartTalk();
        }
    }

    public void StartTalk()
    {
        if (dialog != null && textBox != null)
        {
            dialog.gameObject.SetActive(true); // ��ܹ�ܮ�
            textBox.text = npcText; // �]�w��ܤ奻
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndTalk();
        }
    }

    public void EndTalk()
    {
        if (dialog != null && textBox != null)
        {
            dialog.gameObject.SetActive(false); // ���a���}�d���������ܮ�
            textBox.text = ""; // �M�Ź�ܮ�
        }
    }
}
