using UnityEngine;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerHapticFeedback(bool isLeftHand, int intensity = 100, int duration = 1)
    {
        OVRHapticsClip hapticsClip = new OVRHapticsClip(duration);
        for (int i = 0; i < hapticsClip.Samples.Length; i++)
        {
            hapticsClip.Samples[i] = (byte)intensity;
        }

        if (isLeftHand)
        {
            OVRHaptics.LeftChannel.Preempt(hapticsClip);
        }
        else
        {
            OVRHaptics.RightChannel.Preempt(hapticsClip);
        }
    }
}
