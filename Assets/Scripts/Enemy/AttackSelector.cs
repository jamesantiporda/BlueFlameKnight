using System.Collections.Generic;
using UnityEngine;

public class AttackSelector
{
    private readonly List<BossAttack> _attacks;
    private readonly Dictionary<BossAttack, float> _nextAvailable = new Dictionary<BossAttack, float>();
    private readonly Queue<BossAttack> _recent = new Queue<BossAttack>();
    private readonly List<(BossAttack atk, float weight)> _candidates = new List<(BossAttack, float)>();

    private BossAttack _last;
    private float _globalNextAttack;
    private const int RecentMemory = 3;

    public float GlobalCooldown = 1.0f;

    public AttackSelector(List<BossAttack> attacks)
    {
        _attacks = attacks;
        foreach (var a in attacks) _nextAvailable[a] = 0f;
    }

    public BossAttack Choose(AttackContext ctx)
    {
        if (Time.time < _globalNextAttack) return null;

        _candidates.Clear();

        foreach (var atk in _attacks)
        {
            if(atk == null) continue;

            if (Time.time < _nextAvailable[atk]) continue;

            bool valid = true;
            foreach (var c in atk.conditions)
            {
                if (c == null) continue;
                if (!c.IsMet(ctx)) { valid = false; break; }
            }
            if (!valid) continue;

            float w = atk.baseWeight;
            foreach (var m in atk.modifiers)
            {
                if (m == null) continue;
                w = m.Apply(w, ctx);
            }

            if (atk == _last) w *= 0.2f;
            else if (_recent.Contains(atk)) w *= 0.5f;

            if (w > 0f) _candidates.Add((atk, w));
        }

        if (_candidates.Count == 0) return null;

        float total = 0f;
        foreach (var c in _candidates) total += c.weight;

        float roll = Random.value * total;
        foreach (var c in _candidates)
        {
            roll -= c.weight;
            if (roll <= 0f) return c.atk;
        }
        return _candidates[_candidates.Count - 1].atk;
    }

    public BossAttack ReturnOpeningAttack(int index)
    {
        return _attacks[index];
    }

    public bool HasValidAttack(AttackContext ctx) => Choose(ctx) != null;

    // Call when the attack finishes so cooldowns start from the end of the animation
    public void RegisterUsed(BossAttack atk)
    {
        _nextAvailable[atk] = Time.time + atk.cooldown;
        _globalNextAttack = Time.time + GlobalCooldown;
        _last = atk;

        _recent.Enqueue(atk);
        if (_recent.Count > RecentMemory) _recent.Dequeue();
    }
}