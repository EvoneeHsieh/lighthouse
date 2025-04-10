using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameByAButton : MonoBehaviour
{
    public InputActionReference aButtonAction; // �b Inspector �̩� A �䪺 Action �i��
    public string sceneToLoad = "0_Tutorial"; // �]�A�n�i�������W�r

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
        Debug.Log("A Button Pressed �X Loading Scene...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
