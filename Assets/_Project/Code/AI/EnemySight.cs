using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemySight : MonoBehaviour
{
    // Public properties for the State Machine to read
    public bool CanSeePlayer { get; private set; }
    public bool IsPlayerInAttackRange { get; private set; }

    private EnemyController _controller;
    private Transform _playerTarget;

    // ----- Line of Sight (LOS) Settings -----
    [Header("Line of Sight")]
    [Tooltip("The layer(s) that will block the AI's sight (e.g., Walls).")]
    [SerializeField] private LayerMask obstacleLayerMask;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }
    private void Start()
    {
        // Assign the target. In Start not Awake /!\
        _playerTarget = _controller.PlayerTarget;
    }

    private void Update()
    {
        if (_playerTarget == null)
        {
            CanSeePlayer = false;
            IsPlayerInAttackRange = false;
            return;
        }

        Vector3 directionToPlayer = _playerTarget.position - transform.position;
        float distanceSqr = directionToPlayer.sqrMagnitude;

        // --- Check Attack Range ---
        IsPlayerInAttackRange = distanceSqr <= _controller.AttackRange * _controller.AttackRange;

        // --- Check Detection Range ---
        if (distanceSqr <= _controller.DetectionRange * _controller.DetectionRange)
        {
            // In range, now check for obstacles (Line of Sight)
            // Doing a raycast from the enemy's feet to the player's feet
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, _controller.DetectionRange, obstacleLayerMask);

            // If the raycast DID NOT hit an obstacle, we can see the player.
            CanSeePlayer = (hit.collider == null);
        }
        else
        {
            // Player is too far away
            CanSeePlayer = false;
        }
    }

    // --- Gizmo for debugging ---
    private void OnDrawGizmosSelected()
    {
        if (_controller == null) _controller = GetComponent<EnemyController>();

        // Draw Detection Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _controller.DetectionRange);

        // Draw Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _controller.AttackRange);
    }
}