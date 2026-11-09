using UnityEngine;

public class AttackContext
{
    public Enemy enemy;
    public Transform player;
    public float distance;   // flat XZ distance between boss and player
    public float hpPercent;  // 0 to 1

    public static AttackContext Build(Enemy enemy, Transform player)
    {
        Vector3 e = enemy.transform.position;
        Vector3 p = player.position;

        return new AttackContext
        {
            enemy = enemy,
            player = player,
            distance = Vector3.Distance(new Vector3(e.x, 0, e.z), new Vector3(p.x, 0, p.z)),
            hpPercent = enemy.CurrentHealth / enemy.MaxHealth
        };
    }
}