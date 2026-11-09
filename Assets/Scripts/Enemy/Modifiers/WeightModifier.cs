using System;

[Serializable]
public abstract class WeightModifier
{
    public abstract float Apply(float currentWeight, AttackContext ctx);
}