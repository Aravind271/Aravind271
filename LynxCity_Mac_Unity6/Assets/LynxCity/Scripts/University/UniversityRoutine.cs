using UnityEngine;
using LynxCity.Core;
using LynxCity.Interaction;

namespace LynxCity.University
{
    public class UniversityRoutine : MonoBehaviour, IInteractable
    {
        public int attendance;
        public string Prompt => "E — Attend Network Systems lecture";
        public void Interact(GameInteractor interactor)
        {
            attendance++;
            GameState.Instance?.Earn(150); // tiny campus stipend/demo reward
            if (GameState.Instance) GameState.Instance.InteractionPrompt = $"Lecture attended. Attendance: {attendance}. Research progress +1.";
        }
    }
}
