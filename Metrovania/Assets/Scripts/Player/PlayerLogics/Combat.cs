using UnityEngine;

public class Combat : MonoBehaviour
{
    public Player player;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackCooldown = 1f;
    public LayerMask enemyLayer;
    public int attackDamage = 10;
    private float nextAttackTime;
    public bool CanAttack => Time.time >= nextAttackTime;


    public void AttackAnimationFinished()
    {
        player.AttackAnimationFinished();
    }

    public void Attack()
    {
        if ((!CanAttack))
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
    }
}