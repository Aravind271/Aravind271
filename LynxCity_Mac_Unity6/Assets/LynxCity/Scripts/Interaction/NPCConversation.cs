using UnityEngine;
using LynxCity.Core;

namespace LynxCity.Interaction
{
    public class NPCConversation : MonoBehaviour, IInteractable
    {
        public string npcName = "Local";
        [TextArea] public string line = "こんにちは。大学生活には慣れましたか？";
        public string Prompt => $"E — Talk to {npcName}";
        public void Interact(GameInteractor interactor)
        {
            if (GameState.Instance) GameState.Instance.InteractionPrompt = $"{npcName}: {line}";
        }
    }
}
