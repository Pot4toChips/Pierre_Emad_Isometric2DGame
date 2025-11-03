using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackRangeOffset = 1.0f; // How far IN FRONT to check
    [SerializeField] private float attackRadius = 0.75f;   // How WIDE the check is
    [SerializeField] private LayerMask enemyLayer;


    /// <summary>
    /// This is the public method our states will call.
    /// </summary>
    public void PerformAttack()
    {
        Debug.Log("PLAYER ATTACK!");

        // --- Melee Attack Logic (Area Check) ---

        // Calculate the center of our attack "circle"
        Vector2 attackOrigin = (Vector2)transform.position +
                               InputManager.Instance.LastMoveValue * attackRangeOffset;

        // Check for all colliders on the "Enemy" layer inside that circle
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackOrigin, attackRadius, enemyLayer);

        // Loop through every enemy we hit and apply damage
        foreach (Collider2D enemyCollider in enemiesToDamage)
        {
            if (enemyCollider.TryGetComponent<Health>(out Health enemyHealth))
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }

        // Consume the attack input.
        InputManager.Instance.ResetAttackTimestamp();
    }

    /// <summary>
    /// Draw gizmos in the scene view to help debug the attack range.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (InputManager.Instance == null) return;

        // Set the gizmo color
        Gizmos.color = Color.red;

        // Calculate the attack origin
        Vector2 attackOrigin = (Vector2)transform.position +
                               InputManager.Instance.LastMoveValue * attackRangeOffset;

        // Draw the wire sphere
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}