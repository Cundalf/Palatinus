using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private uint _cantFiresOfHope;
    public uint cantFiresOfHope
    {
        get
        {
            return _cantFiresOfHope;
        }
    }

    private uint _firesOfHope;
    public uint firesOfHope
    {
        get
        {
            return _firesOfHope;
        }
    }

    // Singleton
    private static GameManager sharedInstance = null;

    public static GameManager SharedInstance
    {
        get
        {
            return sharedInstance;
        }
    }

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(this);

        resetGame();
    }

    public void resetGame()
    {
        _firesOfHope = 0;
    }

    public void startGame()
    {
        resetGame();
        SceneManager.LoadScene("Level1");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void addFireOfHope()
    {
        _firesOfHope++;

        UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("No se encontro UIManager");
        }

        uiManager.updateObjetiveGUI();

        if (_firesOfHope >= _cantFiresOfHope)
        {
            startEndGame();
        }
    }

    private void startEndGame()
    {
        ObjectiveZoneController obj = GameObject.FindObjectOfType<ObjectiveZoneController>().GetComponent<ObjectiveZoneController>();

        if(obj == null)
        {
            Debug.LogError("No se encontro ObjectiveZoneController");
            return;
        }

        obj.startGameZone();
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.OBJETIVE_COMPLETE);
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
    }
}
