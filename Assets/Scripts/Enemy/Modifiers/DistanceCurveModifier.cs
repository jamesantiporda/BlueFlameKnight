using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DistanceCurveModifier : WeightModifier
{
    public AnimationCurve multiplierByDistance = AnimationCurve.Linear(0, 1, 10, 1);
    public override float Apply(float w, AttackContext ctx) => w * multiplierByDistance.Evaluate(ctx.distance);
}
