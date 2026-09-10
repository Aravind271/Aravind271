using UnityEngine;

namespace LynxCity.Platform
{
    /// <summary>Runtime quality policy for the macOS-first/native Apple Silicon branch.</summary>
    public sealed class MacNativeRuntime : MonoBehaviour
    {
        public const int TargetFps = 60;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = TargetFps;
            QualitySettings.vSyncCount = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            QualitySettings.shadowDistance = 70f;
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            QualitySettings.shadowCascades = 4;
            QualitySettings.pixelLightCount = 4;
            QualitySettings.realtimeReflectionProbes = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if !UNITY_EDITOR_OSX && !UNITY_STANDALONE_OSX
            Debug.LogWarning("This Lynx City branch is intentionally configured for native macOS only.");
#endif
        }
    }
}
