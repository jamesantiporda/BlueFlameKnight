using itsSALT.FinalCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerHealingCondition : AttackCondition
{
    public override bool IsMet(AttackContext ctx)
        => ctx.player.GetComponent<PlayerController>().ReturnIsDrinking();
}
