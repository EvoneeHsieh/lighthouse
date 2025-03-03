using UnityEngine;
using System.Collections;

public class BGMPlaying : MonoBehaviour
{
    public AudioClip audio2;  // 固定播放的音效
    public AudioClip[] randomSounds; // 可隨機播放的音效陣列
    private AudioSource audioSource;

    public bool autoPlay = true;  // 是否自動播放隨機音效
    public float minInterval = 2f; // 最小間隔時間
    public float maxInterval = 5f; // 最大間隔時間

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 如果沒有 AudioSource，就自動新增
        }

        if (autoPlay)
        {
            StartCoroutine(PlayRandomSoundLoop()); // 啟動自動播放
        }
    }

    // **固定播放指定音效 audio2**
    public void PlaySoundEffect()
    {
        if (audio2 != null)
        {
            audioSource.PlayOneShot(audio2);
        }
        else
        {
            Debug.LogWarning("音效檔案 audio2 沒有指定！");
        }
    }

    // **隨機播放音效**
    public void PlayRandomSoundEffect()
    {
        if (randomSounds == null || randomSounds.Length == 0)
        {
            Debug.LogWarning("沒有可用的隨機音效！");
            return;
        }

        int randomIndex = Random.Range(0, randomSounds.Length);
        audioSource.PlayOneShot(randomSounds[randomIndex]);
    }

    // **協程：每隔一段時間自動播放隨機音效**
    private IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval)); // 等待隨機秒數
            PlayRandomSoundEffect();
        }
    }
}
