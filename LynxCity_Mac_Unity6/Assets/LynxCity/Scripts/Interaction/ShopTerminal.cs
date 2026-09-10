using UnityEngine;
using LynxCity.Core;
using LynxCity.Combat;

namespace LynxCity.Interaction
{
    public class ShopTerminal : MonoBehaviour, IInteractable
    {
        public enum ShopKind { ConvenienceStore, Restaurant, BicycleShop, Mall }
        public ShopKind kind;
        public string Prompt => $"E — Shop at {DisplayName}";
        public string DisplayName => kind switch
        {
            ShopKind.ConvenienceStore => "Konbini",
            ShopKind.Restaurant => "Restaurant",
            ShopKind.BicycleShop => "Bicycle Shop",
            _ => "Shopping Mall"
        };

        public void Interact(GameInteractor interactor)
        {
            var state = GameState.Instance;
            if (!state) return;
            int price; string item;
            switch (kind)
            {
                case ShopKind.Restaurant: price = 780; item = "Meal"; break;
                case ShopKind.BicycleShop: price = 4500; item = "Used Bicycle"; break;
                case ShopKind.Mall: price = 2200; item = "Casual Outfit"; break;
                default: price = 180; item = "Onigiri"; break;
            }
            if (state.Spend(price))
            {
                state.AddItem(item);
                if (item == "Meal" || item == "Onigiri") interactor.GetComponent<Health>()?.Heal(item == "Meal" ? 30 : 10);
                state.InteractionPrompt = $"Purchased {item} for ¥{price:N0}";
            }
            else state.InteractionPrompt = "Not enough yen.";
        }
    }
}
