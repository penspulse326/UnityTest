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

    [SerializeField] float damage = 40f;

    //當前速度
    Vector3 currentVelocity;


    private void OnEnable()
    {
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
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Player") return;

        if (other.gameObject.tag == "Enemy" && type == ProjectileType.Collider)
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

    public void Shoot()
    {
        currentVelocity = transform.forward * projectileSpeed;
    }

}
