using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementAudioBhv : MonoBehaviour,
    IPointerClickHandler
{
    // public fields
    public AudioClip uiClickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        this.PlayClip();
    }

    public void PlayClip()
    {
        AudioManager.instance.PlayClip(uiClickClip);
    }
}
