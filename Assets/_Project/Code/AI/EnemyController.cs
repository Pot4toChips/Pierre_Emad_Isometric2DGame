using UnityEngine;


[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemySight))]
[RequireComponent(typeof(EnemyAnimator))]
public class EnemyController : MonoBehaviour
{
    // --- AI Settings (for states to read) ---
    [Header("AI Settings")]
    [Tooltip("How far the AI can 'see' the player.")]
    [SerializeField] private float _detectionRange = 10f;
    [Tooltip("How close the AI needs to be to attack.")]
    [SerializeField] private float _attackRange = 1.5f;

    [Header("Patrol Settings")]
    [Tooltip("How long to wait at each patrol point.")]
    [SerializeField] private float _patrolWaitTime = 2f;

    [Header("Attack Settings")]
    [Tooltip("Time between attacks")]
    [SerializeField] private float _attackRate = 1.0f;

    // --- Core Components ---
    public EnemyAnimator EnemyAnimator { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemySight Sight { get; private set; }

    // --- Settings ---
    public Transform PlayerTarget { get; private set; }
    public float DetectionRange => _detectionRange;
    public float AttackRange => _attackRange;
    public float PatrolWaitTime => _patrolWaitTime;
    public float AttackRate => _attackRate;

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();
        Sight = GetComponent<EnemySight>();
        EnemyAnimator = GetComponent<EnemyAnimator>();

        // Find the player in the scene
        PlayerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (PlayerTarget == null)
        {
            Debug.LogError("AI Controller: Cannot find GameObject with tag 'Player'!");
        }
    }
}