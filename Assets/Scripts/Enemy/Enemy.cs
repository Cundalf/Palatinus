using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public Slider healthSlider;
    public EnemyRespawn respawn;
    public bool enemyCanDamage;
    [SerializeField]
    private float attackCooldown = 1.0f;
    
    private float attackCountdown; 
    private HealthManager _healthManager;
    private NavMeshAgent _agent;
    private Animator _anim;
    private CapsuleCollider _collider;
    private Transform attackTarget;
    private bool isMoving;
    private bool isAttacking;
    private bool isAlive = true;
    
    public bool IsAlive 
    { 
        get
        {
            return isAlive;
        } 
    }

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        attackCountdown = attackCooldown;

        updateHealthBar();
    }

    private void Update()
    {
        if (!isAlive)
            return;

        if (_agent.velocity == Vector3.zero || isAttacking)
            isMoving = false;
        else
            isMoving = true;

        _anim.SetBool("Running", isMoving);


        if (attackTarget == null)
            return;
        
        _agent.destination = attackTarget.position;

        if (isAttacking)
            return;

        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if(distance < 2f)
        {
            if (_agent.velocity == Vector3.zero)
            {
                _agent.isStopped = true;

                if (attackCountdown < attackCooldown)
                {
                    attackCountdown += Time.deltaTime;
                }
                else
                {
                    int attackId = Random.Range(1, 3);

                    switch (attackId)
                    {
                        case 1:
                            _anim.SetTrigger("Attack");
                            break;
                        case 2:
                            _anim.SetTrigger("AttackAlt");
                            break;
                        default:
                            _anim.SetTrigger("Attack");
                            break;
                    }

                    isAttacking = true;
                    enemyCanDamage = true;
                }
            }
        }
        else
        {
            _agent.isStopped = false;
        }
        
    }

    private void updateHealthBar() {
        healthSlider.value = Mathf.Clamp(_healthManager.health / _healthManager.getMaxHealth, 0.0f, 1.0f);
    }

    public void takeDamage(float damage)
    {
        _healthManager.damage(damage);
        updateHealthBar();

        if(_healthManager.health <= 0.0f)
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ENEMY_DEATH);
            healthSlider.gameObject.SetActive(false);
            _agent.isStopped = true;
            isAlive = false;
            _anim.SetTrigger("Death");
            _collider.enabled = false;
            Invoke("destroyMe", 7.5f);
        }
        else
        {
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ENEMY_DAMAGE);
        }
    }

    private void destroyMe()
    {
        if (respawn != null)
            respawn.newEnemyAfterTime(10f);

        Destroy(gameObject);
    }

    public void goToPlayer(Transform player)
    {
        attackTarget = player;
    }

    // FIXME: Lo ejecuta MutantEndAttackAnim que es una animacion temporal.
    public void attackFinished()
    {
        attackCountdown = 0.0f;
        isAttacking = false;
        enemyCanDamage = false;
        _agent.isStopped = false;
    }
}
