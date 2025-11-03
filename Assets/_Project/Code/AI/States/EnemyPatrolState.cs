using UnityEngine;

public class EnemyPatrolState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyController _controller;

    private PatrolPointsManager.PatrolPoint _currentTarget;

    public EnemyPatrolState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _controller = stateMachine.EnemyController;
    }

    public void Enter()
    {
        // --- Bonus Point: Visual Feedback ---
        _controller.EnemyAnimator.UpdateColor(Color.green);

        // Get the random point from the controller.
        _currentTarget = PatrolPointsManager.Instance.GetRandomPatrolPoint();

        // Check the case of no available point returned.
        if (_currentTarget.transform == null)
        {
            // Get back to idle (wait a while then check again)
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }

        _controller.Movement.MoveTo(_currentTarget.transform.position);
    }

    public void FixedExecute() { }

    public void Execute()
    {
        _controller.EnemyAnimator.UpdateSpriteToTarget(_currentTarget.transform);

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

    public void Exit() 
    {
        // Check the case of no available point returned.
        if (_currentTarget.transform == null) return;
        
        PatrolPointsManager.Instance.FreeExitedPoint(_currentTarget, _controller.PatrolWaitTime);
    }
}