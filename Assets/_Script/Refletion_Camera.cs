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
        // �u�������D�۾��D�ɤϮg��s
        if (UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage != VRTextureUsage.TwoEyes)
        {
            UpdateReflectionCamera();
        }
#endif
    }

    void UpdateReflectionCamera()
    {
        if (mainCamera == null || reflectionCamera == null) return;

        // ��m��٤Ϯg
        Vector3 camPosition = mainCamera.transform.position;
        camPosition.y = -camPosition.y + 2 * transform.position.y; // �H����Y���譱
        reflectionCamera.transform.position = camPosition;

        // �ݪ���V�]�n���
        Vector3 euler = mainCamera.transform.eulerAngles;
        euler.x = -euler.x;
        reflectionCamera.transform.eulerAngles = euler;

        reflectionCamera.Render();
    }
}
