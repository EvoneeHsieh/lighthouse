using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public AudioSource audioSource; // �Ψ��x�s AudioSource

    void Start()
    {
        // ���������e����W�� AudioSource
        //audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("�S����� AudioSource�A�нT�{����W�O�_�����Ĩӷ��I");
        }
    }

    // �}���ɼ��񭵮�
    public void PlayGateSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // ���񭵮�
            Debug.Log("����������ġG" + audioSource.clip.name);
        }
        else
        {
            Debug.LogWarning("�L�k���񭵮ġAAudioSource �� AudioClip �i�ର�šI");
        }
    }
}
