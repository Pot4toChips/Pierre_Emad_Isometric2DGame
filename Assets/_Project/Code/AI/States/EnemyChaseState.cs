using UnityEngine;

public class EnemyChaseState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyController _controller;

    public EnemyChaseState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _controller = stateMachine.EnemyController;
    }

    public void Enter()
    {
        // --- Bonus Point: Visual Feedback ---
        _controller.EnemyAnimator.UpdateColor(Color.yellow);
    }

    public void FixedExecute()
    {
        // Pathfinding logic runs in FixedUpdate for physics
        if (_controller.PlayerTarget != null)
        {
            _controller.Movement.MoveTo(_controller.PlayerTarget.position);
        }
    }

    public void Execute()
    {
        _controller.EnemyAnimator.UpdateSpriteToTarget(_controller.PlayerTarget);

        // --- Transition to Attack ---
        if (_controller.Sight.IsPlayerInAttackRange)
        {
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }
        // --- Transition to Idle/Patrol ---
        else if (!_controller.Sight.CanSeePlayer)
        {
            // Player is gone, go back to idling
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public void Exit()
    {
        // Stop moving when we exit chase
        _controller.Movement.Stop();
    }
}