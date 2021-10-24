using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour, IDamageTaker
{
    public float hp;
    [SerializeField] private bool knockBackable;
    [SerializeField] private Drops drops;
    [SerializeField] private ItemsGroups dropsCollections;

    private List<ItemDrop> _currentDrops;

    public virtual void TakeDamage(Damage dmg)
    {
        if (hp - dmg.Value <= 0)
        {
            switch (drops)
            {
                case Drops.Box:
                    _currentDrops = dropsCollections.boxDrops;
                    break;
                case Drops.Slime: 
                    _currentDrops = dropsCollections.slimeDrops;
                    break;
            }

            if (_currentDrops != null)
            {
                float value = 0;
                foreach (var i in _currentDrops)
                {
                    value += i.chance;
                }

                value = Random.Range(0,value);

                foreach (var item in _currentDrops)
                {
                    if (value <= item.chance)
                    {
                        if (item.item != null)
                        {
                            Instantiate(item.item, transform.position,
                                Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), 0));
                        }
                    }
                    else
                    {
                        value -= item.chance;
                    }
                }
            }

            Destroy(gameObject);
        }

        hp -= dmg.Value;

        if (knockBackable)
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().AddForce(-transform.forward * dmg.KnockBack * 10, ForceMode.Force);
            }
            else
            {
                transform.Translate(-transform.forward * dmg.KnockBack * Time.deltaTime);
            }
        }

        if (dmg.Fire)
        {
            StartCoroutine(Burning(dmg.Value * .1f, dmg.BurningTime));
        }
    }

    IEnumerator Burning(float burningDamage, int burningTime)
    {
        while (burningTime > 0)
        {
            hp -= burningDamage;
            burningTime--;
            yield return new WaitForSeconds(1);
        }
    }

    private enum Drops
    {
        Box,
        Slime,
        Nope
    }
}