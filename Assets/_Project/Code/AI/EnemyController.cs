using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Extensions;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemySight))]
public class EnemyController : MonoBehaviour
{
    // --- Core Components ---
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemySight Sight { get; private set; }

    // --- Target ---
    public Transform PlayerTarget { get; private set; }

    // --- AI Settings (for states to read) ---
    [Header("AI Settings")]
    [Tooltip("How far the AI can 'see' the player.")]
    public float DetectionRange = 10f;
    [Tooltip("How close the AI needs to be to attack.")]
    public float AttackRange = 1.5f;

    [Header("Patrol Settings")]
    public Transform PatrolPointA;
    public Transform PatrolPointB;
    [Tooltip("How long to wait at each patrol point.")]
    public float PatrolWaitTime = 2f;

    [Header("Attack Settings")]
    [Tooltip("Time between attacks")]
    public float AttackRate = 1.0f;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Movement = GetComponent<EnemyMovement>();
        Sight = GetComponent<EnemySight>();

        // These might be on child objects
        Animator = GetComponentInChildren<Animator>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Find the player in the scene
        PlayerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (PlayerTarget == null)
        {
            Debug.LogError("AI Controller: Cannot find GameObject with tag 'Player'!");
        }
    }
}