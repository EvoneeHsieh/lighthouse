using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameByAButton : MonoBehaviour
{
    public InputActionReference aButtonAction; // 在 Inspector 裡拖 A 鍵的 Action 進來
    public string sceneToLoad = "0_Tutorial"; // 設你要進的場景名字

    private void OnEnable()
    {
        aButtonAction.action.performed += OnAPressed;
        aButtonAction.action.Enable();
    }

    private void OnDisable()
    {
        aButtonAction.action.performed -= OnAPressed;
        aButtonAction.action.Disable();
    }

    private void OnAPressed(InputAction.CallbackContext context)
    {
        Debug.Log("A Button Pressed — Loading Scene...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
