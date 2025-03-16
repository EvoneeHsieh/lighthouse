using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    public Canvas dialog; // UI 對話框
    public TextMeshProUGUI textBox; // 文字框
    [TextArea(3, 5)] public string[] npcDialogues; // 儲存多句對話
    public InputActionReference talking;
    public float typingSpeed = 0.05f; // 打字速度

    private int currentDialogueIndex = 0;
    private bool isTalking = false;
    private Coroutine typingCoroutine;
    private bool playerInRange = false; // 玩家是否在範圍內
    public Canvas hindHint;

    private void Start()
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false); // 開場時關閉對話框
        }

        if (textBox != null)
        {
            textBox.text = ""; // 確保對話框內沒有殘留文本
        }
    }

    private void Update()
    {
        // **只有當玩家在範圍內時，才會監聽 T 鍵 或 VR 按鈕**
        if (playerInRange && (talking.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.T)))
        {
            Debug.Log("對話鍵按下");
            ShowNextDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家進入對話範圍");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家離開對話範圍");
            playerInRange = false;
            EndTalk();
        }
    }

    public void StartTalk()
    {
        if (npcDialogues.Length == 0)
        {
            Debug.LogWarning("沒有對話內容！");
            return;
        }

        if (dialog != null && textBox != null)
        {
            dialog.gameObject.SetActive(true); // 顯示對話框
            currentDialogueIndex = 0;
            isTalking = true;
            ShowNextDialogue(); // 顯示第一句話
        }
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < npcDialogues.Length)
        {
            hindHint.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true); // 確保對話框開啟

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // 停止當前的打字動畫
            }

            typingCoroutine = StartCoroutine(TypeDialogue(npcDialogues[currentDialogueIndex])); // 啟動新的對話動畫
            currentDialogueIndex++;
        }
        else
        {
            EndTalk(); // **對話結束時關閉對話框**
        }
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        textBox.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndTalk()
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false);
        }
        isTalking = false;
    }
}
