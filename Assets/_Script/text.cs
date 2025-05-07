using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.5f;
    public InputActionReference talk;

    [Header("場景切換設定")]
    public string nextSceneName;
    public bool autoContinue = false;
    public float autoContinueDelay = 10f;

    [Header("文本內容")]
    public string[] dialogues;

    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
      //  fadeCanvasGroup.alpha = 0; // 🔹 初始完全透明
       // StartCoroutine(FadeIn()); // 🔹 自動開始淡入，不綁定按鍵

        if (dialogues.Length > 0)
        {
            StartCoroutine(TypeText(dialogues[currentDialogueIndex]));
        }
        else
        {
            Debug.LogError("Dialogues 陣列為空！");
        }

        if (autoContinue)
        {
            StartCoroutine(AutoSwitchScene());
        }
        if (talk != null && talk.action != null)
        {
            Debug.Log($"VR 輸入成功捕捉: {talk.action.name}");
        }

        if (dialogues.Length > 0)
        {
            StartCoroutine(TypeText(dialogues[currentDialogueIndex]));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextDialogue();
        }
        if (talk.action.WasPressedThisFrame())
        {
            Debug.Log("XR 控制器按鍵被觸發！");
        }
        if (talk.action.WasPressedThisFrame())
        {
            Debug.Log($"VR 控制器按鍵被觸發！輸入動作: {talk.action.activeControl}");
        }
        if (talk.action.WasPressedThisFrame())
        {
            Debug.Log($"VR 控制器輸入成功：{talk.action.name}，綁定的動作：{talk.action.activeControl}");
        }
    }

    public void ShowNextDialogue()
    {
        if (!isTyping && currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++;
            StartCoroutine(TypeText(dialogues[currentDialogueIndex]));
        }
        else if (!isTyping && currentDialogueIndex == dialogues.Length - 1)
        {
            Debug.Log("對話已完成，開始退場...");
            StartCoroutine(FadeOutAndSwitchScene()); // 只有文本全部顯示完畢才淡出
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        textComponent.text = "";
        foreach (char letter in text)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration); // 🔹 讓場景從黑慢慢變亮
            yield return null;
        }
    }

    private IEnumerator FadeOutAndSwitchScene()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration); // 🔹 淡出至全黑
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator AutoSwitchScene()
    {
        yield return new WaitForSeconds(autoContinueDelay);
        SceneManager.LoadScene(nextSceneName);
    }
}









