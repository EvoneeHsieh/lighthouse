using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPC_Talk : MonoBehaviour
{
    public Canvas dialog; // UI ��ܮ�
    public TextMeshProUGUI textBox; // ��r��
    [TextArea(3, 5)] public string[] npcDialogues; // �x�s�h�y���
    public InputActionReference talking;
    public float typingSpeed = 0.05f; // ���r�t��

    private int currentDialogueIndex = 0;
    private bool isTalking = false;
    private Coroutine typingCoroutine;
    private bool playerInRange = false; // ���a�O�_�b�d��
    public Canvas hindHint;

    private void Start()
    {
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false); // �}����������ܮ�
        }

        if (textBox != null)
        {
            textBox.text = ""; // �T�O��ܮؤ��S���ݯd�奻
        }
    }

    private void Update()
    {
        // **�u�����a�b�d�򤺮ɡA�~�|��ť T �� �� VR ���s**
        if (playerInRange && (talking.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.T)))
        {
            Debug.Log("�������U");
            ShowNextDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���a�i�J��ܽd��");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���a���}��ܽd��");
            playerInRange = false;
            EndTalk();
        }
    }

    public void StartTalk()
    {
        if (npcDialogues.Length == 0)
        {
            Debug.LogWarning("�S����ܤ��e�I");
            return;
        }

        if (dialog != null && textBox != null)
        {
            dialog.gameObject.SetActive(true); // ��ܹ�ܮ�
            currentDialogueIndex = 0;
            isTalking = true;
            ShowNextDialogue(); // ��ܲĤ@�y��
        }
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < npcDialogues.Length)
        {
            hindHint.gameObject.SetActive(false);
            dialog.gameObject.SetActive(true); // �T�O��ܮض}��

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // �����e�����r�ʵe
            }

            typingCoroutine = StartCoroutine(TypeDialogue(npcDialogues[currentDialogueIndex])); // �Ұʷs����ܰʵe
            currentDialogueIndex++;
        }
        else
        {
            EndTalk(); // **��ܵ�����������ܮ�**
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
