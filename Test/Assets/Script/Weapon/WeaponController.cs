using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("武器Icon")]
    public Sprite weaponIcon;

    [Header("武器的主要GameObject 不使用時隱藏")]
    [SerializeField] GameObject weaponRoot;
    [Header("槍口位置")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;
    [Header("Projectile Prefab")]
    [SerializeField] Projectile projectilePrefab;
    [Header("兩次射擊之間的Delay時間")]
    [SerializeField] float delayBetweenShoots = 0.5f;
    [Header("射一發所需的子彈數量")]
    [SerializeField] int bulletPerShoot;

    [Space(5)]
    [Header("每秒Reload的數量")]
    [SerializeField] float ammoReloadRate = 5f;
    [Header("射擊完畢到可以Reload的Delay時間")]
    [SerializeField] float ammoReloadDelay = 2f;
    [Header("最大子彈數量")]
    [SerializeField] float maxAmmo = 8;

    [Header("開火特效")]
    [SerializeField] GameObject muzzleFlashPrefab;
    public GameObject sourcePrefab { get; set; }

    public float currentAmmoRatio { get; private set; }
    public bool isCooling { get; private set; }

    //當前子彈數量
    float currentAmmo;
    //距離上次射擊的時間
    float timeSinceLastShoot;
    //是否在瞄準狀態
    bool isAim;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {
        if (timeSinceLastShoot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            //當前子彈開始Reload
            currentAmmo += ammoReloadRate * Time.deltaTime;

            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

            isCooling = true;
        }
        else
        {
            isCooling = false;
        }

        if (maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1f;
        }
        else
        {
            currentAmmoRatio = currentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
    }

    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Single:
                if (inputDown)
                {
                    print("Single射擊");
                    TryShoot();
                }
                return;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    print("Auto射擊");
                    TryShoot();
                }
                return;

            default: return;
        }
    }

    private void TryShoot()
    {
        if (currentAmmo >= 1f && timeSinceLastShoot + delayBetweenShoots < Time.time)
        {
            HandleShoot();
            currentAmmo -= 1;
        }
    }

    private void HandleShoot()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            newProjectile.Shoot();
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject newMuzzlePrefab = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newMuzzlePrefab, 1.5f);
        }
        timeSinceLastShoot = Time.time;
    }
}
