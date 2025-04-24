using UnityEngine;
using TMPro;
using System.Collections;


public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // TMP ����
    private string[] dialogues = new string[]
    {
        "�L�����·t���A�ȳѪ��n���O�������G��w���A�V���۪��ݦa�O",
        "���ݸI���n��p��ӡA�����r�a½��A���H�����P���鴲�����n�T",
        "²�u��r�̦��B�{�G �u�t�έ��Ҥ��K�v",
        "�����C�G�A���y�f�d���f�A�s�󴲸��A�·t���{�{�L��",
        "��M�A�@�D���~��z�·t�A�ӫG�F�e�誺�D��",
        "�A�O�֡H������|�X�{�b�o�̡H�o�̤w�g�ܤ[�S���H�ӹL�F�K�K",
    };

    private int currentDialogueIndex = 0; // ��e����ܯ���
    private bool isTyping = false; // ����O�_���b���r

    private void Start()
    {
        if (dialogues.Length > 0)
        {
            Debug.Log($"��ܲĤ@�q��r: {dialogues[currentDialogueIndex]}");
            StartCoroutine(TypeText(dialogues[currentDialogueIndex])); // ��ܲĤ@�q��r
        }
        else
        {
            Debug.LogError("Dialogues �}�C���šI");
        }
    }

    private void Update()
    {
        // �˴��ƹ�������U
        if (Input.GetMouseButtonDown(0)) // 0 �N��ƹ�����
        {
            ShowNextDialogue();
        }
    }

    public void ShowNextDialogue()
    {
        // �ˬd�O�_�٦��U�@�q��r
        if (!isTyping && currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++; // ����U�@�q
            StartCoroutine(TypeText(dialogues[currentDialogueIndex])); // �Ұʥ��r�ĪG
        }
        else if (!isTyping)
        {
            Debug.Log("�w�g��ܧ��Ҧ���ܡI");
        }
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        textComponent.text = ""; // �M�Ť�r
        foreach (char letter in text)
        {
            textComponent.text += letter; // �v�r���
            yield return new WaitForSeconds(0.05f); // ����r�t��
        }
        isTyping = false;
    }
}





