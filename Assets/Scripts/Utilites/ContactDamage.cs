using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBack=1;
    [SerializeField] private bool fire;
    [SerializeField] private int burnTime;

    private Damage _dmg;
    
    private void OnEnable()
    {
        _dmg = new Damage
        {
            Value = damage,
            KnockBack = knockBack,
            BurningTime = burnTime,
            Fire = fire,
        };
    }

    public void SetDamage(Damage dmg)
    {
        _dmg = dmg;
    }

    public Damage GetDamage()
    {
        return _dmg;
    }
}
