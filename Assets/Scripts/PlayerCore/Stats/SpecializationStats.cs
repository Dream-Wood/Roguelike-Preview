using System;

public class SpecializationStats : StatsDecorator
{
    private readonly Specialization _specialization;

    public SpecializationStats(IStatsProvider wrappedEntity, Specialization specialization) : base(wrappedEntity)
    {
        _specialization = specialization;
    }

    protected override PlayerStats GetStatsInternal()
    {
        return _wrappedEntity.GetStats() + GetSpecStats(_specialization);
    }

    private PlayerStats GetSpecStats(Specialization spec)
    {
        switch (spec)
        {
            case Specialization.Archer:
                return new PlayerStats
                {
                    Speed = 1,
                    MaxHealth = 80,
                    Stamina = 100,
                    Damage = 5,
                    AttackSpeed = 1,
                    Luck = 0,
                };
            case Specialization.Warrior:
                return new PlayerStats
                {
                    Speed = 1,
                    MaxHealth = 120,
                    Stamina = 80,
                    Damage = 5,
                    AttackSpeed = 1,
                    Luck = 0,
                };
            default:
                throw new NotImplementedException($"Specialization {spec} incorrect");
        }
    }
}