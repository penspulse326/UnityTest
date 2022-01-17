using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFighter : MonoBehaviour
{
    [Header("角色攻擊類型")]
    [SerializeField] Actor actorType;
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;
    [Header("跳躍攻擊傷害")]
    [SerializeField] float jumpAttackDamage = 20f;
    [Header("攻擊距離")]
    [SerializeField] float attackRange = 2f;
    [Header("跳躍攻擊距離")]
    [SerializeField] float jumpAttackRange = 4f;
    [Header("攻擊時間間隔")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Space(20)]
    [Header("要丟出去的Projectile")]
    [SerializeField] Projectile throwProjectile;
    [Header("手部座標")]
    [SerializeField] Transform hand;

    GameObject player;
    RootMover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mover = GetComponent<RootMover>();
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
            AttackBehavior("Attack");
        }
        else if (IsInJumpAttackRange())
        {
            mover.CancelMove();
            AttackBehavior("JumpAttack");
        }
        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position);
        }

        UpdateTimer();
    }

    //檢查攻擊動作是否結束
    private bool CheckHasAttack()
    {
        baseLayer = animator.GetCurrentAnimatorStateInfo(0);

        //如果當前動作等於Base Layer.Attack
        if (baseLayer.fullPathHash == Animator.StringToHash("Base Layer.Attack"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void AttackBehavior(string attackName)
    {
        transform.LookAt(targetHealth.transform);

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            TriggerAttack(attackName);
        }
    }

    private void TriggerAttack(string attackName)
    {
        animator.ResetTrigger(attackName);
        animator.SetTrigger(attackName);
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
    }

    private bool IsInJumpAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < jumpAttackRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    private void Hit()
    {
        if (targetHealth == null || actorType != Actor.Boss) return;
        if (IsInAttackRange())
        {
            targetHealth.TakeDamage(attackDamage);
        }
    }

    private void JumpHit()
    {
        if (targetHealth == null || actorType != Actor.Boss) return;
        if (IsInJumpAttackRange())
        {
            targetHealth.TakeDamage(jumpAttackDamage);
        }
    }

    private void Shoot()
    {
        if (targetHealth == null || actorType != Actor.Archer) return;
        if (throwProjectile != null)
        {
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(player.transform.position + Vector3.up * 3
- hand.position));
            newProjectile.Shoot(gameObject);
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
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 2, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 2, jumpAttackRange);
    }
}
