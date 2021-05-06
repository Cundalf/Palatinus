using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider objetiveSlider;
    public Image power1Filter;
    public Image power2Filter;
    public Image power3Filter;
    public GameObject endGameGO;
    public GameObject winGameGO;
    public List<GameObject> framesGO;
    public GameObject pauseGO;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if(pauseGO.activeSelf)
        {
            GameManager.SharedInstance.resumeGame();
            pauseGO.SetActive(false);
        }
        else
        {
            GameManager.SharedInstance.pauseGame();
            pauseGO.SetActive(true);
        }
    }

    public void updateHealthGUI(float health, float maxHealth)
    {
        healthSlider.value = Mathf.Clamp(health / maxHealth, 0.0f, 1.0f);
    }

    public void updateManaGUI(float mana, float maxMana)
    {
        manaSlider.value = Mathf.Clamp(mana / maxMana, 0.0f, 1.0f);
    }

    public void updatePower1GUI(float currentTime, float maxTime)
    {
        power1Filter.fillAmount = 1 - Mathf.Clamp(currentTime / maxTime, 0.0f, 1.0f);
    }

    public void updatePower2GUI(float currentTime, float maxTime)
    {
        power2Filter.fillAmount = 1 - Mathf.Clamp(currentTime / maxTime, 0.0f, 1.0f);
    }

    public void updatePower3GUI(float currentTime, float maxTime)
    {
        power3Filter.fillAmount = 1 - Mathf.Clamp(currentTime / maxTime, 0.0f, 1.0f);
    }

    public void updateObjetiveGUI()
    {
        objetiveSlider.value = Mathf.Clamp((float)GameManager.SharedInstance.firesOfHope / (float)GameManager.SharedInstance.cantFiresOfHope, 0.0f, 1.0f);
    }

    public void goToMainMenu()
    {
        GameManager.SharedInstance.goToMainMenu();
    }

    public void endGameGUI()
    {
        desactiveGUI();
        endGameGO.SetActive(true);
    }

    public void winGameGUI()
    {
        desactiveGUI();
        winGameGO.SetActive(true);
    }

    private void desactiveGUI()
    {
        foreach(GameObject go in framesGO)
        {
            go.SetActive(false);
        }
    }
}
