using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public GameObject enemy;
    
    void Start()
    {
        newEnemy();
    }

    public void newEnemyAfterTime(float time = 5.0f)
    {
        Invoke("newEnemy", time);
    }

    private void newEnemy()
    {
        GameObject enemyGO = Instantiate(enemy, transform.position, transform.rotation);
        enemyGO.GetComponent<Enemy>().respawn = this;
    }
}
