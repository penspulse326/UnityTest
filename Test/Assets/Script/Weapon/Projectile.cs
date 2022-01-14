using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Collider,
    Particle,
}

public class Projectile : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] ProjectileType type;
    [Header("射到目標的Particle")]
    [SerializeField] GameObject hitParticle;
    [Header("擊中物件特效的存活時間")]
    [SerializeField] float particleLifeTime = 2f;
    [Header("子彈速度")]
    [SerializeField] float projectileSpeed;

    [Header("Projectile的存在時間")]
    [SerializeField] float maxLifeTime = 3f;
    [Header("重力(影響子彈末端下降的力量)")]
    [SerializeField] float gravityDownForce = 0f;
    [Header("Projectile的傷害")]
    [SerializeField] float damage = 40f;

    [Space(5)]
    [Header("Destroy音效")]
    [SerializeField] AudioClip detroySFX;

    AudioSource audioSource;
    GameObject owner;
    bool canAttack;

    //當前速度
    Vector3 currentVelocity;


    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if (gravityDownForce > 0)
        {
            currentVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner || !canAttack) return;

        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Collider)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == owner || !canAttack) return;

        if ((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Particle)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
        Destroy(gameObject);
        
        if (detroySFX != null)
        {
            audioSource.PlayOneShot(detroySFX);
        }
    }

    private void HitEffect(Vector3 hitPoint)
    {
        if (hitParticle)
        {
            GameObject newParticleInstance = Instantiate(hitParticle, hitPoint, transform.rotation);
            if (particleLifeTime > 0)
            {
                Destroy(newParticleInstance, particleLifeTime);
            }
        }
    }

    public void Shoot(GameObject gameObject)
    {
        owner = gameObject;
        currentVelocity = transform.forward * projectileSpeed;
        canAttack = true;
    }

}
