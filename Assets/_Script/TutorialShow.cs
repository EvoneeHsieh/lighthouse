using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShow : MonoBehaviour
{
    public Canvas tutorialCanvas;
    [SerializeField] private string detectTag = "Player";
    private void Start()
    {
        // 確保一開始Canvas處於隱藏狀態
        if (tutorialCanvas != null)
            tutorialCanvas.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowCanvas();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 如果離開的物件是指定Tag
        if (other.CompareTag(detectTag))
        {
            HideCanvas();
        }
    }
    private void ShowCanvas()
    {
        if (tutorialCanvas != null)
            tutorialCanvas.enabled = true;
    }

    private void HideCanvas()
    {
        if (tutorialCanvas != null)
            tutorialCanvas.enabled = false;
    }
}
