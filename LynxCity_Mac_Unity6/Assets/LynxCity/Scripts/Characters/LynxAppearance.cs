using UnityEngine;
using LynxCity.Core;

namespace LynxCity.Characters
{
    public class LynxAppearance : MonoBehaviour
    {
        public int OutfitIndex { get; private set; }
        public string OutfitName => HumanFigureFactory.LynxOutfits[Mathf.Clamp(OutfitIndex,0,HumanFigureFactory.LynxOutfits.Length-1)].name;
        Transform visual;
        void Start(){ visual=transform.Find("Visual"); if(!visual) visual=HumanFigureFactory.BuildJapaneseAdult(transform,"Visual",1f,HumanFigureFactory.LynxOutfits[0],true).transform; Apply(0); }
        public void Apply(int index){ OutfitIndex=Mathf.Clamp(index,0,HumanFigureFactory.LynxOutfits.Length-1); if(!visual)visual=transform.Find("Visual"); if(visual)HumanFigureFactory.ApplyOutfit(visual,HumanFigureFactory.LynxOutfits[OutfitIndex]); if(GameState.Instance!=null)GameState.Instance.CurrentOutfit=OutfitName; }
    }
}
