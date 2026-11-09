using UnityEngine;
using System;

[Serializable]
public abstract class AttackCondition
{
    public abstract bool IsMet(AttackContext ctx);
}