using UnityEngine;

public class PlanarReflection : MonoBehaviour
{
    public Camera mainCamera;
    public Camera reflectionCamera;
    public RenderTexture reflectionTexture;
    public Material waterMaterial;

    private void Start()
    {
        reflectionTexture = new RenderTexture(1024, 1024, 16);
        reflectionCamera.targetTexture = reflectionTexture;
        waterMaterial.SetTexture("_ReflectionTex", reflectionTexture);
    }

    void LateUpdate()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        // 只讓左眼主相機主導反射更新
        if (UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage != VRTextureUsage.TwoEyes)
        {
            UpdateReflectionCamera();
        }
#endif
    }

    void UpdateReflectionCamera()
    {
        if (mainCamera == null || reflectionCamera == null) return;

        // 位置對稱反射
        Vector3 camPosition = mainCamera.transform.position;
        camPosition.y = -camPosition.y + 2 * transform.position.y; // 以水面Y為鏡面
        reflectionCamera.transform.position = camPosition;

        // 看的方向也要對稱
        Vector3 euler = mainCamera.transform.eulerAngles;
        euler.x = -euler.x;
        reflectionCamera.transform.eulerAngles = euler;

        reflectionCamera.Render();
    }
}
