using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ManaManager))]
[RequireComponent(typeof(HealthManager))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float power1Cooldown = 8.0f;
    private float power1Counter;
    [SerializeField]
    private float power2Cooldown = 12.0f;
    private float power2Counter;
    [SerializeField]
    private float power3Cooldown = 20.0f;
    private float power3Counter;

    public GameObject fireballRespawn;
    [SerializeField]
    public GameObject fireballPrefab;
    [SerializeField]
    private float speed = 7.5f;
    [SerializeField]
    private float rotationSpeed = 200.0f;
    [SerializeField]
    private float auraDuration = 5.0f;
    [SerializeField]
    private ParticleSystem auraPS;
    [SerializeField]
    private ParticleSystem powerAttackPS;
    [SerializeField]
    private float power1cost = 20.0f;
    [SerializeField]
    private float power2cost = 20.0f;
    [SerializeField]
    private float power3cost = 30.0f;

    private float inputTranslation;
    private float inputRotation;
    private Animator _animator;
    private HealthManager _healthManager;
    private ManaManager _manaManager;

    private bool isAttacking; // Controla si el jugador esta haciendo un ataque
    private bool playerAttacked; // Control para evitar cola de ataques en la animacion
    public bool playerCanDamage; // Controla si el player ataco a un enemigo
    private bool isWalking;
    private bool isJumping;
    private bool isBlocking;
    private bool inBlockingAnim;
    private bool isAlive;
    private bool isCasting;
    public bool isAuraActive;
    public bool playerIsAttackingPower;

    private float withoutAttackTime;
    private int cantSimpleAttack;

    private UIManager uiManager;

    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _manaManager = GetComponent<ManaManager>();
        _animator = GetComponent<Animator>();

        isAttacking = false;
        isWalking = false;
        isJumping = false;
        isBlocking = false;
        playerAttacked = false;
        inBlockingAnim = false;
        isCasting = false;
        isAuraActive = false;
        playerIsAttackingPower = false;

        isAlive = true;

        withoutAttackTime = 0f;
        cantSimpleAttack = 0;

        power1Counter = power1Cooldown;
        power2Counter = power2Cooldown;
        power3Counter = power3Cooldown;

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {
        if (!isAlive)
            return;

        cooldownPowersControl();
        inputControl();

        if (canMoving())
        {
            inputTranslation = Input.GetAxis("Vertical");
            inputRotation = Input.GetAxis("Horizontal");

            float translationVelocity = speed;
            if (isWalking)
            {
                translationVelocity = speed / 2;
            }
            else if (inputTranslation < 0.0f)
            {
                translationVelocity = speed * 0.6f;
            }

            float translation = inputTranslation * translationVelocity * Time.deltaTime;
            float rotation = inputRotation * rotationSpeed * Time.deltaTime;

            transform.Translate(0f, 0f, translation);
            transform.Rotate(0f, rotation, 0f);
        }
        else
        {
            inputTranslation = 0f;
            inputRotation = 0f;
        }

        animateMoving();
        attackControl();
        animateBlock();
    }

    private bool canMoving()
    {
        return (!isBlocking && !inBlockingAnim && !isAttacking && !isCasting);
    }

    private void inputControl()
    {
        if (isCasting)
            return;

        if (Input.GetKeyUp(KeyCode.Keypad1) || Input.GetKeyUp(KeyCode.Alpha1))
        {
            castFireball();
            return;
        }

        if (Input.GetKeyUp(KeyCode.Keypad2) || Input.GetKeyUp(KeyCode.Alpha2))
        {
            castAura();
            return;
        }

        if (Input.GetKeyUp(KeyCode.Keypad3) || Input.GetKeyUp(KeyCode.Alpha3))
        {
            
            castAttackPower();
            return;
        }

        if (Input.GetMouseButtonUp(0) && !isAttacking)
        {
            isAttacking = true;
            playerAttacked = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            isWalking = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isWalking = false;

        if (!isWalking)
        {
            if (Input.GetMouseButtonDown(1))
                isBlocking = true;

            if (Input.GetMouseButtonUp(1))
                isBlocking = false;

            if (Input.GetKeyUp(KeyCode.Space) && !isBlocking)
                isJumping = true;
        }
    }

    private void cooldownPowersControl()
    {
        float t = Time.deltaTime;

        if(power1Counter < power1Cooldown)
        {
            power1Counter += t;
            uiManager.updatePower1GUI(power1Counter, power1Cooldown);
        }

        if (power2Counter < power2Cooldown)
        {
            power2Counter += t;
            uiManager.updatePower2GUI(power2Counter, power2Cooldown);
        }

        if (power3Counter < power3Cooldown)
        {
            power3Counter += t;
            uiManager.updatePower3GUI(power3Counter, power3Cooldown);
        }
    }

    private void castAttackPower()
    {
        if (_manaManager.mana >= power3cost && power3Counter >= power3Cooldown)
        {
            power3Counter = 0.0f;
            _manaManager.consume(power3cost);
            isCasting = true;
            playerIsAttackingPower = true;
            _animator.SetTrigger("CastBuff");
            powerAttackPS.Play();
            Invoke("endCast", 2f);
            Invoke("endCastAttackPower", 2f);
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.SPELL3);
        }
    }

    private void endCastAttackPower()
    {
        playerIsAttackingPower = false;
    }

    private void castAura()
    {
        if (_healthManager.health >= power2cost && power2Counter >= power2Cooldown)
        {
            power2Counter = 0.0f;
            _healthManager.damage(power2cost);
            isCasting = true;
            isAuraActive = true;
            _animator.SetTrigger("PowerUp");
            auraPS.Play();
            Invoke("endCast", 2.5f);
            Invoke("endAura", auraDuration);
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.SPELL2);
        }
    }

    private void castFireball()
    {
        if (_manaManager.mana >= power1cost && power1Counter >= power1Cooldown)
        {
            power1Counter = 0.0f;
            _manaManager.consume(power1cost);
            isCasting = true;
            _animator.SetTrigger("CastMagic");
            Invoke("invokeFireball", 0.4f);
            Invoke("endCast", 1.25f);
        }
    }

    private void endAura()
    {
        isAuraActive = false;
        auraPS.Stop();
    }

    private void endCast()
    {        
        isCasting = false;
    }

    private void invokeFireball()
    {
        Instantiate(fireballPrefab, fireballRespawn.transform.position, fireballRespawn.transform.rotation);
    }

    private void animateMoving()
    {
        if (inputTranslation != 0.0f)
        {
            _animator.SetBool("Walking", isWalking);
            _animator.SetBool("Running", !isWalking);
        }
        else
        {
            _animator.SetBool("Running", false);
            _animator.SetBool("Walking", false);

            /*
            if(inputRotation != 0.0f)
            {
                _Animator.SetBool("Turning", false);
            }
            else
            {
                _Animator.SetBool("Turning", true);
            }
            */
        }

        if (isJumping && !isWalking)
        {
            _animator.SetTrigger("Jump");
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.PLAYER_JUMP);
            // TODO: Detectar cuando deja de saltar
            isJumping = false;
        }

        _animator.SetFloat("Turn", Mathf.Clamp(inputRotation, -1f, 1f));
        _animator.SetFloat("Forward", Mathf.Clamp(inputTranslation, -1f, 1f));
    }

    private void attackControl()
    {
        if (isAttacking && !playerAttacked)
        {
            animateAttack();
        }
        else
        {
            if (withoutAttackTime <= 2f)
            {
                withoutAttackTime += Time.deltaTime;
            }
            else
            {
                cantSimpleAttack = 0;
            }
        }
    }

    private void animateAttack()
    {
        if (cantSimpleAttack < 2)
        {
            _animator.SetInteger("AttackId", 1);
            cantSimpleAttack++;
        }
        else
        {
            _animator.SetInteger("AttackId", 2);
            cantSimpleAttack = 0;
        }

        _animator.SetTrigger("Attack");
        withoutAttackTime = 0f;
        playerAttacked = true;
        playerCanDamage = true;
        Invoke("playAttackSFX", 0.5f);
    }

    private void playAttackSFX()
    {
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.PLAYER_ATTACK);
    }

    // FIXME: Lo ejecuta EndAttackAnim que es una animacion temporal.
    public void attackFinished()
    {
        isAttacking = false;
        playerCanDamage = false;
        _animator.SetInteger("AttackId", 0);
    }

    private void animateBlock()
    {
        if (isBlocking)
        {
            _animator.SetBool("Blocking", true);
            inBlockingAnim = true;
        }
        else
        {
            _animator.SetBool("Blocking", false);
            
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
            {
                inBlockingAnim = false;
            }
        }
    }

    public void takeDamage(float damage)
    {
        if (!isAlive)
            return;

        if(isBlocking)
        {
            _animator.SetTrigger("DamageBlock");
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.BLOCK);
        }
        else
        {
            _healthManager.damage(damage);
            _animator.SetTrigger("Damage");

            if (_healthManager.health <= 0.0f)
            {
                isAlive = false;
                _animator.SetInteger("DeathId", 2);
                _animator.SetTrigger("Death");
                Invoke("endGameGUI", 2.0f);
                SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.PLAYER_DEATH);
                SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.LOSE);
                AudioManager.SharedInstance.Stop();
            }
            else
            {
                SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.PLAYER_DAMAGE);
            }
        }
    }

    private void endGameGUI()
    {
        uiManager.endGameGUI();
    }

    public void objetiveComplete()
    {
        isAlive = false;
        _animator.SetBool("Dancing", true);
        _animator.SetBool("Running", false);
        _animator.SetBool("Walking", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAlive)
            return;

        if (!other.gameObject.CompareTag("Item"))
            return;

        Item item = other.gameObject.GetComponent<Item>();

        if(item.itemType == Item.ItemType.Health)
        {
            if (_healthManager.health >= _healthManager.getMaxHealth)
                return;

            _healthManager.heal(item.getValue);
            item.desactivate();
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.HEAL);
        }

        if (item.itemType == Item.ItemType.Objetive)
        {
            GameManager.SharedInstance.addFireOfHope();
            Destroy(other.gameObject);
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.TARGET);
        }
    }
}
