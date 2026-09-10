namespace LynxCity.Interaction
{
    public interface IInteractable
    {
        string Prompt { get; }
        void Interact(GameInteractor interactor);
    }
}
