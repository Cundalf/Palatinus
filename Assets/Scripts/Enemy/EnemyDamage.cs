using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public Enemy enemyController;

    [SerializeField]
    private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (!enemyController.IsAlive)
            return;

        if (!other.gameObject.CompareTag("Player"))
            return;

        if (!enemyController.enemyCanDamage)
            return;

        enemyController.enemyCanDamage = false;

        other.GetComponent<Player>().takeDamage(damage);
    }
}
