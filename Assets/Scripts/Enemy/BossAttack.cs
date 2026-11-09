using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Attack")]
public class BossAttack : ScriptableObject
{
    public int attackNo;
    public float cooldown = 4f;
    public float baseWeight = 1f;

    [SerializeReference] public List<AttackCondition> conditions = new List<AttackCondition>();
    [SerializeReference] public List<WeightModifier> modifiers = new List<WeightModifier>();
}