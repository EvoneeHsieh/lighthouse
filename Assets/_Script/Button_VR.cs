using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_VR : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame ³QÂI¤F¡I");
        SceneManager.LoadScene("0_Gamestart");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
