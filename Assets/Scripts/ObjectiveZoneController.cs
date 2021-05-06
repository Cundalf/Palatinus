using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveZoneController : MonoBehaviour
{
    public GameObject portalGO;
    public GameObject fairyGO;

    public void endGameZone()
    {
        portalGO.SetActive(false);
        fairyGO.SetActive(true);

        UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("No se encontro UIManager");
        }

        uiManager.winGameGUI();
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.WIN);
    }

    public void startGameZone()
    {
        portalGO.SetActive(true);
        fairyGO.SetActive(false);
    }
}
