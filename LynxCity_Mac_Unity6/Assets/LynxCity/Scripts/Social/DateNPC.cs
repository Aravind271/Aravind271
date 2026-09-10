using UnityEngine;
using LynxCity.Core;
using LynxCity.Interaction;

namespace LynxCity.Social
{
    public class DateNPC : MonoBehaviour, IInteractable
    {
        public string npcName = "Aoi";
        public int affinity;
        public string Prompt => $"E — Spend time with {npcName}";
        public void Interact(GameInteractor interactor)
        {
            int cost = affinity < 20 ? 900 : 1400;
            if (GameState.Instance != null && GameState.Instance.Spend(cost))
            {
                affinity += 10;
                GameState.Instance.InteractionPrompt = affinity < 30
                    ? $"Coffee with {npcName}. Affinity {affinity}/100"
                    : $"Dinner date with {npcName}. Affinity {affinity}/100";
            }
            else if (GameState.Instance) GameState.Instance.InteractionPrompt = "You need more yen for the date.";
        }
    }
}
