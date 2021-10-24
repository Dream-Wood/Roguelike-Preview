public class Damage
{
    public float Value;
    public float KnockBack;
    public int BurningTime;
    public bool Fire;

    public static Damage operator *(Damage d1, float val)
    {
        return new Damage()
        {
            Value = d1.Value * val,
            KnockBack = d1.KnockBack,
            BurningTime = d1.BurningTime,
            Fire = d1.Fire,
        };
    }
}
