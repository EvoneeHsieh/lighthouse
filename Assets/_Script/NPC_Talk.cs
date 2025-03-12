using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    public Canvas dialog; // UI 對話框
    public TextMeshProUGUI textBox; // 文字框
    [TextArea(3, 5)] public string npcText; // 這裡可以在Inspector內直接輸入文本
    public InputActionReference talking;

    private void Start()
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false); // 遊戲開始時先隱藏對話框
        }

        if (textBox != null)
        {
            textBox.text = ""; // 確保開場時沒有殘留文字
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
            dialog.gameObject.SetActive(true); // 顯示對話框
            textBox.text = npcText; // 設定對話文本
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
            dialog.gameObject.SetActive(false); // 玩家離開範圍時關閉對話框
            textBox.text = ""; // 清空對話框
        }
    }
}
