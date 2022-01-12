using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("一開始就有的武器")]
    [SerializeField] List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("儲存武器的位置")]
    [SerializeField] Transform equipWeaponParent;

    [Header("瞄準的準備時間")]
    [SerializeField] float aimTime = 2f;

    public event Action<WeaponController,int> onAddWeapon;

    // 目前裝備的武器清單位置
    int activeWeaponIndex;

    //武器最多三個
    WeaponController[] weapons = new WeaponController[3];
    PlayerController player;
    InputController input;

    bool isAim;

    // Start is called before the first frame update
    void Start()
    {
        //初始狀態
        activeWeaponIndex = -1;

        input = GameManagerSingleton.Instance.InputController;
        player = GetComponent<PlayerController>();
        player.onAim += OnAim;

        foreach(WeaponController weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }

        SwitchWeapon(1);
    }


    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        if (activeWeapon && isAim)
        {
            //處理射擊
            activeWeapon.HandleShootInput(
                input.GetFireInputDown(),
                input.GetFireInputHeld(),
                input.GetFireInputUp()
            );
        }

        int switchWeaponInput = input.GetSwitchWeaponInput();
        if (switchWeaponInput != 0)
        {
            SwitchWeapon(switchWeaponInput);
        }
    }

    //切換武器
    public void SwitchWeapon(int addIndex)
    {
        int newWeaponIndex = 0;

        if (activeWeaponIndex + addIndex > weapons.Length -1)
        {
            newWeaponIndex = 0;
        }
        else if(activeWeaponIndex + addIndex <0)
        {
            newWeaponIndex = weapons.Length -1;
        }
        else
        {
            newWeaponIndex = activeWeaponIndex + addIndex;
        }
        //換武器到NewIndex
        SwitchToWeaponIndex(newWeaponIndex);
    }

    private void SwitchToWeaponIndex(int index)
    {
        if(index >= 0 && index < weapons.Length)
        {
            if(GetWeaponAtSlotIndex(index)!=null)
            {
                //如果目前已裝備武器就隱藏
                if(GetActiveWeapon()!=null)
                {
                    GetActiveWeapon().ShowWeapon(false);
                }

                //顯示武器
                activeWeaponIndex = index;
                GetActiveWeapon().ShowWeapon(true);
            }
        }
    }

    private void OnAim(bool value)
    {
        if(value)
        {
            StopCoroutine(DelayAim());
            StartCoroutine(DelayAim());
        }
        else
        {
            isAim = value;
        }
    }

    IEnumerator DelayAim()
    {
        yield return new WaitForSecondsRealtime(aimTime);
        isAim = true;
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        if (index >= 0 && index < weapons.Length && weapons[index] != null)
        {
            return weapons[index];
        }

        return null;
    }

    public bool AddWeapon (WeaponController weaponprefab)
    {
        //檢查是否已存在武器
        if(HasWeapon(weaponprefab))
        {
            return false;
        }

        //找到下一個空插槽
        for(int i=0 ; i<weapons.Length ; i++)
        {
            if(weapons[i] == null)
            {
                //產生Weapon到設定好的位置底下
                WeaponController weaponInstace = Instantiate(weaponprefab,equipWeaponParent);

                weaponInstace.sourcePrefab = weaponprefab.gameObject;
                weaponInstace.ShowWeapon(false);

                weapons[i] = weaponInstace;

                onAddWeapon?.Invoke(weaponInstace, i);

                return true;
            }
        }

        return false;
    }

    private bool HasWeapon(WeaponController weaponPrefab)
    {
        foreach(WeaponController weapon in weapons)
        {
            if(weapon!=null && weapon.sourcePrefab == weaponPrefab)
            {
                return true;
            }
        }

        return false;
    }
}
