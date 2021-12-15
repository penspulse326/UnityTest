using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動參數")]
    [Tooltip("最高移動速度")]
    [SerializeField] float moveSpeed = 8;
    [Tooltip("加速倍速")]
    [Range(1, 3)]
    [SerializeField] float sprintSpeedModifier = 2;
    [Tooltip("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;
    [Tooltip("蹲下時減速")]
    [Range(0, 1)]
    [SerializeField] float crouchedSpeedModifier = 0.5f;
    [Tooltip("旋轉速度")]
    [SerializeField] float rotateSpeed = 5f;
    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("向上的力")]
    [SerializeField] float jumpForce = 15;
    [Tooltip("向下的力")]
    [SerializeField] float gravityForce = 50;
    [Tooltip("與地面的距離")]
    [SerializeField] float distanceToGround = 0.1f;
    
    InputController input;
    CharacterController controller;
    Animator animator;

    //Next Frame 
    Vector3 targetMovement;
    Vector3 jumpDirection;

    float lastFrameSpeed;

    // Start is called before the first frame update
    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        //玩家行為
        MoveBehaviour();
        JumpBehaviour();

        Debug.DrawRay(transform.position,-Vector3.up,Color.red);
    }

    //行為處理
    private void MoveBehaviour()
    {
        targetMovement = Vector3.zero;
        targetMovement += input.GetMoveInput().z * GetCurrentCameraForward();
        targetMovement += input.GetMoveInput().x * GetCurrentCameraRight();

        //限制對角線不超過1
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);

        //下一幀速度
        float nextFrameSpeed = 0f;

        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
        }
        else if (input.GetSprintInput())
        {

            nextFrameSpeed = 1f;

            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
        }
        else
        {
            nextFrameSpeed = 0.5f;

            SmoothRotation(targetMovement);
        }

        if(nextFrameSpeed!=lastFrameSpeed)
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed,nextFrameSpeed,addSpeedRatio);

        animator.SetFloat("WalkSpeed", lastFrameSpeed);

        controller.Move(targetMovement * Time.deltaTime * moveSpeed);
    }



    private void JumpBehaviour()
    {
        if (input.GetJumpInputDown() && IsGrounded())
        {
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
            
        }

        jumpDirection.y -= gravityForce * Time.deltaTime;
        //jumpDirection.y = Mathf.Max(jumpDirection.y, -gravityForce);

        controller.Move(jumpDirection * Time.deltaTime);
        //animator.ResetTrigger("IsJump");
    }

    //檢查是否在地上
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up , distanceToGround);
    }

    //取得相機正前方方向
    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }

    //取得相機的右方方向
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 caneraRight = Camera.main.transform.right;
        caneraRight.y = 0f;
        caneraRight.Normalize();
        return caneraRight;
    }

    //轉向平滑化
    private void SmoothRotation(Vector3 targetMovement)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMovement, Vector3.up), rotateSpeed * Time.deltaTime);
    }
}
