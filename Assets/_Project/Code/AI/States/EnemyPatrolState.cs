using UnityEngine;

public class EnemyPatrolState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyController _controller;

    private Transform _currentTarget;

    public EnemyPatrolState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _controller = stateMachine.EnemyController;
    }

    public void Enter()
    {
        // --- Bonus Point: Visual Feedback ---
        _controller.SpriteRenderer.color = Color.green;

        // Choose which patrol point to go to
        if (_currentTarget == null || _currentTarget == _controller.PatrolPointB)
        {
            _currentTarget = _controller.PatrolPointA;
        }
        else
        {
            _currentTarget = _controller.PatrolPointB;
        }

        _controller.Movement.MoveTo(_currentTarget.position);
    }

    public void FixedExecute() { }

    public void Execute()
    {
        // --- Transition to Chase ---
        if (_controller.Sight.CanSeePlayer)
        {
            _stateMachine.ChangeState(_stateMachine.ChaseState);
        }
        // --- Transition to Idle (when destination is reached) ---
        else if (_controller.Movement.HasReachedDestination())
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public void Exit() { }
}