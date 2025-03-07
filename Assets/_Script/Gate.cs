using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public AudioSource audioSource; // 用來儲存 AudioSource

    void Start()
    {
        // 嘗試獲取當前物件上的 AudioSource
        //audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("沒有找到 AudioSource，請確認物件上是否有音效來源！");
        }
    }

    // 開門時播放音效
    public void PlayGateSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // 播放音效
            Debug.Log("播放門的音效：" + audioSource.clip.name);
        }
        else
        {
            Debug.LogWarning("無法播放音效，AudioSource 或 AudioClip 可能為空！");
        }
    }
}
