using UnityEngine;
using LynxCity.Core;
using LynxCity.Interaction;

namespace LynxCity.World
{
    public class DistrictTravel : MonoBehaviour, IInteractable
    {
        public Transform player;
        public string destinationName = "Shibuya";
        public Vector3 destinationPosition;
        public int fare = 2400;
        public string Prompt => $"E — Taxi to {destinationName} (¥{fare:N0})";

        public void Interact(GameInteractor interactor)
        {
            if (!player) player = interactor.transform;
            if (GameState.Instance != null && !GameState.Instance.Spend(fare))
            {
                GameState.Instance.InteractionPrompt = "Not enough yen for taxi.";
                return;
            }
            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;
            player.position = destinationPosition;
            if (cc) cc.enabled = true;
            if (GameState.Instance)
            {
                GameState.Instance.CurrentDistrict = destinationName;
                GameState.Instance.InteractionPrompt = $"Arrived in {destinationName}.";
            }
        }
    }
}
