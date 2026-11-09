using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OrCondition : AttackCondition
{
    [SerializeReference] public List<AttackCondition> conditions = new List<AttackCondition>();

    public override bool IsMet(AttackContext ctx)
    {
        foreach (var c in conditions)
        {
            if (c == null) continue;
            if (c.IsMet(ctx)) return true;
        }
        return false;
    }
}