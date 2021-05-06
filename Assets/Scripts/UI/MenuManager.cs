using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void startGame()
    {
        GameManager.SharedInstance.startGame();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void showTutorial()
    {
        _anim.SetBool("Tutorial", true);
    }

    public void closeTutorial()
    {
        _anim.SetBool("Tutorial", false);
    }

    public void showCredits()
    {
        _anim.SetBool("Credits", true);
    }

    public void closeCredits()
    {
        _anim.SetBool("Credits", false);
    }

    public void showLore()
    {
        _anim.SetBool("Lore", true);
    }

    public void closeLore()
    {
        _anim.SetBool("Lore", false);
    }
}
