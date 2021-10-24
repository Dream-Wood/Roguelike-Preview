using System;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance = new ItemDataBase();
    public PlayerStats GetStats(IStatsProvider wrappedEntity, Artifacts artifact)
    {
        var result = wrappedEntity.GetStats();
        switch (artifact)
        {
            case Artifacts.RuneOfFire:
                return new PlayerStats()
                {
                    FireAspect = true
                };
            
            case Artifacts.HellishDonut:
                return new PlayerStats()
                {
                    MaxHealth = -40,
                    Damage = 2.5f,
                    FireAspect = true
                };
            
            case Artifacts.Halo:
                return new PlayerStats()
                {
                    Damage = 1.5f,
                    MaxHealth = 30,
                    Stamina = 30,
                    AttackSpeed = .5f,
                    Luck = 1,
                    Speed = .25f,
                };
            
            case Artifacts.RoundedCube:
                return new PlayerStats()
                {
                    Speed = -.15f,
                    Stamina = -20,
                    MaxHealth = 50,
                };
            
            case Artifacts.HaresFoot:
                return new PlayerStats()
                {
                    Luck = 1,
                }; 
            
            case Artifacts.TigerTooth:
                return new PlayerStats()
                {
                    Damage = 0.75f,
                };
                        
            case Artifacts.BlueTooth:
                return new PlayerStats()
                {
                    //What?
                };
            
            case Artifacts.RedSquare:
                return new PlayerStats()
                {
                    //What's this?
                    Speed = -.25f
                };
            
                        
            case Artifacts.RazorBlade:
                return new PlayerStats()
                {
                    Damage = 1.5f
                };
            
            case Artifacts.ElectricBroom:
                return new PlayerStats()
                {
                    Speed = .35f,
                    AttackSpeed = .25f
                };

            default:
                throw new NotImplementedException($"Item {artifact} not found from ItemsDataBase");
        }
    }
}