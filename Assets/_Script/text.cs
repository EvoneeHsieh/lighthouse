using UnityEngine;
using TMPro;
using System.Collections;


public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // TMP 元件
    private string[] dialogues = new string[]
    {
        "無垠的黑暗中，僅剩的聲音是間歇的液體滴落，敲擊著金屬地板",
        "金屬碰撞聲突如其來，視角猛地翻轉，伴隨重擊與物體散落的聲響",
        "簡短文字依次浮現： 「系統重啟中…」",
        "視角低矮，掃描貨櫃裂口，零件散落，黑暗中閃爍微光",
        "突然，一道光芒穿透黑暗，照亮了前方的道路",
        "你是誰？為什麼會出現在這裡？這裡已經很久沒有人來過了……",
    };

    private int currentDialogueIndex = 0; // 當前的對話索引
    private bool isTyping = false; // 控制是否正在打字

    private void Start()
    {
        if (dialogues.Length > 0)
        {
            Debug.Log($"顯示第一段文字: {dialogues[currentDialogueIndex]}");
            StartCoroutine(TypeText(dialogues[currentDialogueIndex])); // 顯示第一段文字
        }
        else
        {
            Debug.LogError("Dialogues 陣列為空！");
        }
    }

    private void Update()
    {
        // 檢測滑鼠左鍵按下
        if (Input.GetMouseButtonDown(0)) // 0 代表滑鼠左鍵
        {
            ShowNextDialogue();
        }
    }

    public void ShowNextDialogue()
    {
        // 檢查是否還有下一段文字
        if (!isTyping && currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++; // 移到下一段
            StartCoroutine(TypeText(dialogues[currentDialogueIndex])); // 啟動打字效果
        }
        else if (!isTyping)
        {
            Debug.Log("已經顯示完所有對話！");
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        textComponent.text = ""; // 清空文字
        foreach (char letter in text)
        {
            textComponent.text += letter; // 逐字顯示
            yield return new WaitForSeconds(0.05f); // 控制打字速度
        }
        isTyping = false;
    }
}





