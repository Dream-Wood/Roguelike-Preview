using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStats : IStatsProvider
{
    private readonly RaceType _raceType;

    public RaceStats(RaceType raceType)
    {
        _raceType = raceType;
    }
    
    public PlayerStats GetStats()
    {
        switch (_raceType)
        {
            default:
                return new PlayerStats
                {
                    AttackSpeed = 0,
                    Damage = 0,
                    FireAspect = false,
                    Luck = 0,
                    MaxHealth = 0,
                    Speed = 0,
                    Stamina = 0
                };
        }
    }
}