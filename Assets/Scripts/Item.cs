using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Objetive,
        Health,
        Mana
    }

    public ItemType itemType = ItemType.Objetive;
    public GameObject itemGO;

    [SerializeField]
    private bool reactivatable = false;
    [SerializeField]
    private float reactivatableCooldown = 10.0f;
    [SerializeField]
    private float value = 0.0f;

    public float getValue
    {
        get
        {
            return value;
        }
    }

    private bool isActive;
    private float counter;

    private void Start()
    {
        counter = 0;
        isActive = itemGO.activeSelf;
    }

    private void Update()
    {
        if (!reactivatable || isActive)
            return;

        if (counter < reactivatableCooldown)
        {
            counter += Time.deltaTime;
        }
        else
        {
            isActive = true;
            itemGO.SetActive(true);
            counter = 0;
        }
    }

    public void desactivate()
    {
        itemGO.SetActive(false);
        isActive = false;
    }
}
