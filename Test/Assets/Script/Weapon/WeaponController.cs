using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("武器的主要GameObject 不使用時隱藏")]
    [SerializeField] GameObject weaponRoot;
    [Header("槍口位置")]
    [SerializeField] Transform weaponMuzzle;
    
    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;
    [Header("兩次射擊之間的Delay時間")]
    [SerializeField] float delayBetweenShoots = 0.5f;
    int bulletPerShoot;

    [Space(5)]
    [Header("每秒Reload的數量")]
    [SerializeField] float ammoReloadRate = 1f;
    [Header("射擊完畢到可以Reload的Delay時間")]
    [SerializeField] float ammoReloadDelay = 2f;
    [Header("最大子彈數量")]
    [SerializeField] float maxAmmo = 8;

    public GameObject sourcePrefab {get;set;}

    //當前子彈數量
    float currentAmmo;
    //距離上次射擊的時間
    float timeSinceLastShoot;
    //是否在瞄準狀態
    bool isAim;

    private void Awake() {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);
    }

    public void HandleShootInput(bool inputDown, bool inputHeld , bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Single:
            if(inputDown)
            {
                print("Single射擊");
            }
            return;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    print("Auto射擊");
                }
                return;
            default: return;
        }
    }
}
