using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballPower : MonoBehaviour
{
    [SerializeField]
    public float force;
    [SerializeField]
    public float damage;

    void Start()
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.forward * force, ForceMode.Impulse);
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.SPELL1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
