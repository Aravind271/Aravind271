using UnityEngine;
using LynxCity.Core;

namespace LynxCity.Characters
{
    public sealed class LynxAppearance : MonoBehaviour
    {
        const string OutfitPref = "LynxCity.OutfitIndex";
        public int OutfitIndex { get; private set; }
        public LynxHeroFactory.OutfitDefinition Current => LynxHeroFactory.Outfits[Mathf.Clamp(OutfitIndex, 0, LynxHeroFactory.Outfits.Length - 1)];
        public string OutfitName => Current.Name;
        Transform visual;

        void Start()
        {
            visual = transform.Find("Visual");
            if (!visual) visual = LynxHeroFactory.Build(transform).transform;
            Apply(PlayerPrefs.GetInt(OutfitPref, 0), false);
        }

        public void Apply(int index, bool save = true)
        {
            OutfitIndex = Mathf.Clamp(index, 0, LynxHeroFactory.Outfits.Length - 1);
            if (!visual) visual = transform.Find("Visual");
            if (visual) LynxHeroFactory.ApplyOutfit(visual, OutfitIndex);
            if (GameState.Instance != null) GameState.Instance.CurrentOutfit = OutfitName;
            if (save)
            {
                PlayerPrefs.SetInt(OutfitPref, OutfitIndex);
                PlayerPrefs.Save();
            }
        }

        public void Cycle(int direction)
        {
            int count = LynxHeroFactory.Outfits.Length;
            Apply((OutfitIndex + direction + count) % count);
        }
    }
}
