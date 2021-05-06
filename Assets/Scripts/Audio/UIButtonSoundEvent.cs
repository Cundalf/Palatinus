using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSoundEvent : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData ped)
    {
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.SWITCH_OPTION);
    }
}