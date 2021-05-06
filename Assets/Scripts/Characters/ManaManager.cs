using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    [SerializeField]
    private float initialMana = 100.0f;
    [SerializeField]
    private float maxMana = 100.0f;
    [SerializeField]
    private float restoreFactor = 1.0f;

    private UIManager uiManager;
    private float currentMana;

    public float mana
    {
        get
        {
            return currentMana;
        }
    }

    public float getMaxMana
    {
        get
        {
            return maxMana;
        }
    }

    private void Awake()
    {
        currentMana = initialMana;
        if (maxMana < 0.0f)
            maxMana = 0.0f;
    }

    void Start()
    {
        if (gameObject.CompareTag("Player"))
            uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (currentMana >= maxMana)
            return;

        float m = currentMana + (restoreFactor * Time.deltaTime);
        currentMana = Mathf.Clamp(m, 0.0f, maxMana);

        if (gameObject.CompareTag("Player"))
            uiManager.updateManaGUI(currentMana, maxMana);
    }

    public bool consume(float amount)
    {
        if (currentMana - amount > 0.0f && amount > 0.0f)
        {
            currentMana -= amount;

            if (gameObject.CompareTag("Player"))
                uiManager.updateManaGUI(currentMana, maxMana);
            return true;
        }

        return false;
    }
}
