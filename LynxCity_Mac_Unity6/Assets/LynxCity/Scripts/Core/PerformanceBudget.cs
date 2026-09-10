using UnityEngine;

namespace LynxCity.Core
{
    public class PerformanceBudget : MonoBehaviour
    {
        public enum Preset { Performance, Balanced, Quality }
        public static PerformanceBudget Instance { get; private set; }
        public Preset Current { get; private set; }=Preset.Balanced;
        public int UrbanCrowdPerDistrict { get; private set; }=34;
        void Awake(){Instance=this;DontDestroyOnLoad(gameObject);Apply(AutoPreset());}
        Preset AutoPreset(){int ram=SystemInfo.systemMemorySize;int vram=SystemInfo.graphicsMemorySize;if(ram>=24||vram>=8192)return Preset.Quality;if(ram>=12)return Preset.Balanced;return Preset.Performance;}
        public void Apply(Preset preset){Current=preset;Application.targetFrameRate=60;QualitySettings.vSyncCount=0;switch(preset){case Preset.Performance:UrbanCrowdPerDistrict=22;QualitySettings.shadowDistance=35f;QualitySettings.antiAliasing=0;break;case Preset.Balanced:UrbanCrowdPerDistrict=34;QualitySettings.shadowDistance=55f;QualitySettings.antiAliasing=2;break;default:UrbanCrowdPerDistrict=50;QualitySettings.shadowDistance=75f;QualitySettings.antiAliasing=4;break;}}
    }
}
