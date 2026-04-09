using TheLastHeir.Runtime.Entity;
namespace TheLastHeir.Runtime.Interactions
{
    /// <summary>
    /// 모든 상호작용 가능한 객체
    /// </summary>
    public interface IInteractable
    {
        string GetInteractionPrompt();
        void Interact(PlayerInteraction player);
    }
}