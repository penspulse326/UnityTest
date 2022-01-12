using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [Header("撿起來會得到的武器")]
    [SerializeField] WeaponController weaponPrefab;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    private void OnPick(GameObject player)
    {
        WeaponManager weaponManager = player.GetComponent<WeaponManager>();

        if (weaponManager)
        {
            if (weaponManager.AddWeapon(weaponPrefab))
            {
                if (weaponManager.GetActiveWeapon() == null)
                {
                    weaponManager.SwitchWeapon(1);
                }

                Destroy(gameObject);
            }
        }
    }
}
