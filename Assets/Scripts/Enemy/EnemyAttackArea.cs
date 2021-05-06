using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour
{
    public Enemy enemyController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ENEMYDETECT);
            enemyController.goToPlayer(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            enemyController.goToPlayer(null);
    }
}
