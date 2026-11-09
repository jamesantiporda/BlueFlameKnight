using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RangeCondition : AttackCondition
{
    public float min, max = 4f;
    public override bool IsMet(AttackContext ctx) => ctx.distance >= min && ctx.distance <= max;
}
