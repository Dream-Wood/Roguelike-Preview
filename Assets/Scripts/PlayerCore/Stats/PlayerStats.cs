public class PlayerStats
{
    public float Speed;
    public float Stamina;
    public float Damage;
    public float AttackSpeed;
    public float MaxHealth;
    public float Luck;

    public bool FireAspect;

    public static PlayerStats operator +(PlayerStats s1, PlayerStats s2)
    {
        return new PlayerStats()
        {
            Speed = s1.Speed + s2.Speed,
            Stamina = s1.Stamina + s2.Stamina,
            Damage = s1.Damage + s2.Damage,
            AttackSpeed = s1.AttackSpeed + s2.AttackSpeed,
            MaxHealth = s1.MaxHealth + s2.MaxHealth,
            Luck = s1.Luck + s2.Luck,
            
            FireAspect = s1.FireAspect || s2.FireAspect,
        };
    }
}