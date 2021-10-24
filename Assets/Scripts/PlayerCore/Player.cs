using System;
using System.Collections.Generic;
using Items;
using UnityEngine;


public class Player : Entity
{
    public delegate void ChangeHealth(float currentVal, float maxVal);

    public event ChangeHealth changeHealth;

    public delegate void ChangeStats(PlayerStats stats);

    public event ChangeStats changeStats;

    public delegate void ChangeItems(Dictionary<DropItemType, int> items);

    public event ChangeItems changeItems;

    [SerializeField] private ContactDamage contactDamage;
    [SerializeField] private List<Artifacts> artifacts = new List<Artifacts>();

    private Specialization _spec = Specialization.Warrior;
    private PlayerStats _stats;

    private readonly Dictionary<DropItemType, int> _items = new Dictionary<DropItemType, int>(2)
    {
        {DropItemType.HealPotion, 0},
        {DropItemType.Coin, 0},
    };

    private IStatsProvider _provider;

    public void SetSpecialization(Specialization cp)
    {
        _spec = cp;
        UpdateStats();
    }

    public void AddItem(DropItemType type)
    {
        _items[type] += 1;
        changeItems?.Invoke(_items);
    }

    public void UseHealPotion()
    {
        if (_items[DropItemType.HealPotion] <= 0 || Math.Abs(hp - _stats.MaxHealth) < .1)
        {
            return;
        }

        _items[DropItemType.HealPotion] -= 1;

        hp += 20;
        if (hp > _stats.MaxHealth)
        {
            hp = _stats.MaxHealth;
        }
        
        changeItems?.Invoke(_items);
        changeHealth?.Invoke(hp, _stats.MaxHealth);
    }

    public void AddArtifact(Artifacts artifact)
    {
        artifacts.Add(artifact);
        UpdateStats();
    }

    public void TestEvent()
    {
        UpdateStats();
        changeItems?.Invoke(_items);
    }

    private void Start()
    {
        UpdateStats();
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateStats()
    {
        _provider = new RaceStats(RaceType.Orc);
        _provider = new SpecializationStats(_provider, _spec);
        _provider = new ItemStatsProvider(_provider, artifacts);
        _stats = _provider.GetStats();
        
        changeStats?.Invoke(_stats);

        if (hp > _stats.MaxHealth)
        {
            hp = _stats.MaxHealth;
        }

        changeHealth?.Invoke(hp, _stats.MaxHealth);

        contactDamage.SetDamage(new Damage()
        {
            BurningTime = 3,
            Fire = _stats.FireAspect,
            KnockBack = 10,
            Value = _stats.Damage,
        });
    }

    public override void TakeDamage(Damage dmg)
    {
        base.TakeDamage(dmg);
        changeHealth?.Invoke(hp, _stats.MaxHealth);
    }
}