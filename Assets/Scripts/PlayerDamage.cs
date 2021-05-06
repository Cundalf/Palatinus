using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public Player playerController;

    [SerializeField]
    private float swordDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        bool playerIsAttacking = (playerController.playerCanDamage || playerController.playerIsAttackingPower);

        if (!playerIsAttacking)
            return;

        playerController.playerCanDamage = false;

        float damage = swordDamage;

        if (playerController.playerIsAttackingPower)
        {
            damage *= 4;
        }
        else if(playerController.isAuraActive)
        {
            damage *= 2;
        }

        other.GetComponent<Enemy>().takeDamage(damage);
    }
}
