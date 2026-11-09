using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealthRangeCondition : AttackCondition
{
    [Range(0, 1)] public float min = 0f, max = 1f;
    public override bool IsMet(AttackContext ctx) => ctx.hpPercent >= min && ctx.hpPercent <= max;
}
