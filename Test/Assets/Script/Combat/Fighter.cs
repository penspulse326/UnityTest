using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;
    [Header("攻擊距離")]
    [SerializeField] float attackRange = 2f;
    [Header("攻擊時間間隔")]
    [SerializeField] float timeBetweenAttack = 2f;

    Mover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    float timeSinceLastAttack = Mathf.Infinity;

    void Start()
    {
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetHealth == null || targetHealth.IsDead()) return;

        if (IsInAttackRange())
        {
            mover.CancelMove();
            AttackBehavior();
        }

        else if (timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position, 1f);
        }

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void AttackBehavior()
    {
        transform.LookAt(targetHealth.transform);

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            TriggerAttack();
        }
    }

    private void TriggerAttack()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    private void Hit()
    {
        if(targetHealth == null) return;
        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }
    }

    public void CancelAttack()
    {
        targetHealth = null;
    }

    private void OnDie()
    {
        this.enabled = false;
    }

    //call by Unity
    //繪製可視化物件 怪物的巡邏路徑
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
