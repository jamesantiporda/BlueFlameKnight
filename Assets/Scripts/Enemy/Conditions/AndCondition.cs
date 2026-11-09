using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AndCondition : AttackCondition
{
    [SerializeReference] public List<AttackCondition> conditions = new List<AttackCondition>();

    public override bool IsMet(AttackContext ctx)
    {
        foreach (var c in conditions)
        {
            if (c == null) continue;
            if (!c.IsMet(ctx)) return false;
        }
        return true;
    }
}