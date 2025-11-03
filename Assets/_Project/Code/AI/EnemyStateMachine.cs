using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyController EnemyController { get; private set; }

    // --- Concrete States ---
    public EnemyIdleState IdleState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }

    // --- State Management ---
    private IState _currentState;

    private void Awake()
    {
        EnemyController = GetComponent<EnemyController>();

        // --- Initialize state "objects" ---
        IdleState = new EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
    }

    private void Start()
    {
        // --- Set the initial state ---
        _currentState = PatrolState;
        _currentState.Enter();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedExecute();
    }

    private void Update()
    {
        _currentState?.Execute();
    }

    public void ChangeState(IState newState)
    {
        if (newState == null || newState == _currentState) return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}