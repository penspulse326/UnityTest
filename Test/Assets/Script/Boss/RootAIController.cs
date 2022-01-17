using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAIController : MonoBehaviour
{
    [Header("追趕距離")]
    [SerializeField] float chaseDistance = 10f;
    [Header("失去目標後的困惑時間")]
    [SerializeField] float confuseTime = 5f;

    [Header("Patrol的GameObject物件")]
    [SerializeField] PatrolPath patrol;
    [Header("待在Waypoint的時間")]
    [SerializeField] float waypointToWaitTime = 3f;
    [Header("需要到達Waypoint的距離")]
    [SerializeField] float wayPointToStay = 3f;
    [Header("巡邏時的速度")]
    [Range(0, 1)]
    [SerializeField] float patrolSpeedRatio = 0.5f;

    GameObject player;
    Animator animator;
    RootMover mover;
    Health health;
    RootFighter fighter;

    //起始位置
    Vector3 beginPosition;
    //上次看到玩家的時間
    private float timeLastSawPlayer = Mathf.Infinity;
    //當前需要到達的waypoint編號
    int currentWayPointIndex = 0;
    //距離上次抵達Waypoint的時間
    float timeSinceArriveWayPoint = 0;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mover = GetComponent<RootMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<RootFighter>();

        beginPosition = transform.position;
        health.onDamage += OnDamage;
        health.onDie += OnDie;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            health.TakeDamage(10);
        }

        if (health.IsDead()) return;

        if (IsInRange())
        {
            AttackBehavior();
        }
        else if (timeLastSawPlayer < confuseTime)
        {
            ConfuseBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimer();
    }

    private void AttackBehavior()
    {
        timeLastSawPlayer = 0;
        animator.SetBool("IsConfuse", false);
        fighter.Attack(player.GetComponent<Health>());
    }

    //巡邏行為
    private void PatrolBehaviour()
    {
        Vector3 nextWayPointPosition = beginPosition;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                timeSinceArriveWayPoint = 0;
                currentWayPointIndex = patrol.GetNextWayPointNumber(currentWayPointIndex);
            }

            if (timeSinceArriveWayPoint > waypointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWayPointIndex));
            }
        }
        else
        {
            //回到原點
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPosition);
        }
    }
    // 檢查是否已經抵達Waypoint
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWayPointIndex)) < wayPointToStay);
    }

    //困惑行為
    private void ConfuseBehaviour()
    {
        fighter.CancelAttack();
        mover.CancelMove();
        animator.SetBool("IsConfuse", true);
    }

    //是否小於追趕距離
    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void UpdateTimer()
    {
        timeLastSawPlayer += Time.deltaTime;
        timeSinceArriveWayPoint += Time.deltaTime;
    }

    private void OnDamage()
    {
        //受到攻擊時觸發的行為
    }

    private void OnDie()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
    }

    //call by Unity
    //繪製可視化物件 怪物的巡邏路徑
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
