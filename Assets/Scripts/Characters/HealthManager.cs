using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private float initialHealth = 100.0f;
    [SerializeField]
    private float maxHealth = 100.0f;
    private float currentHealth;
    private UIManager uiManager;

    public float health
    {
        get
        {
            return currentHealth;
        }
    }

    public float getMaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    private void Awake()
    {
        currentHealth = initialHealth;
        if (maxHealth < 0.0f)
            maxHealth = 0.0f;
    }

    void Start()
    {
        if (gameObject.CompareTag("Player"))
            uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void heal(float healAmount)
    {
        if (currentHealth == maxHealth || healAmount <= 0.0f)
            return;

        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0.0f, maxHealth);

        if (gameObject.CompareTag("Player"))
            uiManager.updateHealthGUI(currentHealth, maxHealth);
    }

    public void damage(float damageAmount)
    {
        if(currentHealth > 0.0f && damageAmount > 0.0f)
        {
            currentHealth -= damageAmount;
            if (currentHealth < 0.0f)
                currentHealth = 0.0f;

            if(gameObject.CompareTag("Player"))
                uiManager.updateHealthGUI(currentHealth, maxHealth);
        }
    }
}
