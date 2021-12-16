using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [Tooltip("最大移動速度")] 
    [SerializeField] float maxSpeed = 6f;
    [SerializeField] float animatorChangeRatio = 0.1f;

    NavMeshAgent navMeshAgent;

    //上一幀的移動速度
    float lastFrameSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        
    }

    private void Update() {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, localVelocity.z, animatorChangeRatio);
        //將全局變量轉為 local 速度變量
        this.GetComponent<Animator>().SetFloat("WalkSpeed", lastFrameSpeed/maxSpeed);
    }

    public void MoveTo(Vector3 destination , float speedRatio)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedRatio);
        navMeshAgent.destination = destination;
    }

    public void CancleMove()
    {
        //停止移動
        navMeshAgent.isStopped = true;
    }
}
