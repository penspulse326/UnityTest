using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [Header("水平軸靈敏度")]
    [SerializeField] float sensibility_X = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensibility_Y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensibility_Z = 5;

    [Header("最小垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;

    [Header("相機與目標距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("相機與目標最小距離")]
    [SerializeField] float minDistance = 2;
    [Header("相機與目標最大距離")]
    [SerializeField] float maxDistance = 25;

    [Header("Player的Health")]
    [SerializeField] GameObject player;
    [Header("被攻擊的特效")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("加速特效")]
    [SerializeField] ParticleSystem sprintParticle;
    [Header("Pause的UI")]
    [SerializeField] GameObject pauseUI;

    [Header("Offset")]
    [SerializeField] Vector3 offset;

    float mouse_X = 0;
    float mouse_Y = 30;

    InputController input;

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onSprint += OnSprint;
    }

    private void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;

            mouse_X += input.GetMouseXAxis() * sensibility_X;
            mouse_Y += input.GetMouseYAxis() * sensibility_Y;

            //限制Y軸最大角度
            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            //保持相機角度與距離
            transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);
            transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -cameraToTargetDistance) + target.position + Vector3.up * offset.y;

            cameraToTargetDistance += input.GetMouseScrollWheelAxis() * sensibility_Z;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, minDistance, maxDistance);
        }

        else
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnDamage()
    {
        if (beHitParticle == null) return;
        beHitParticle.Play();
    }

    private void OnSprint()
    {
        if (sprintParticle == null) return;
        sprintParticle.Play();
    }
}
