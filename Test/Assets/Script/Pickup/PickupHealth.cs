using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    [Header("要恢復的血量")]
    [SerializeField] float healAmount;

    [Header("要Destroy的Pickup Root")]
    [SerializeField] GameObject pickupRoot;

    Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        pickup.onPick += OnPick;
    }

    private void OnPick(GameObject player)
    {
        Health health = player.GetComponent<Health>();

        if (health)
        {
            health.Heal(healAmount);

            Destroy(pickupRoot);
        }
    }
}
