using UnityEngine.EventSystems;

public interface IToggleable :
    IPointerEnterHandler,
    IPointerClickHandler,
    IPointerExitHandler
{
    void Disable();
    void Enable();
}