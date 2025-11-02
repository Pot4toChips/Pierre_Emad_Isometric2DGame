using UnityEngine;

public class EnemyIdleState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyController _controller;

    private float _timer;

    public EnemyIdleState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _controller = stateMachine.EnemyController;
    }

    public void Enter()
    {
        _controller.Movement.Stop();
        _timer = 0f;

        // --- Bonus Point: Visual Feedback ---
        _controller.SpriteRenderer.color = Color.white;
    }

    public void FixedExecute() { }

    public void Execute()
    {
        _timer += Time.deltaTime;

        // --- Transition to Chase ---
        if (_controller.Sight.CanSeePlayer)
        {
            _stateMachine.ChangeState(_stateMachine.ChaseState);
        }
        // --- Transition to Patrol ---
        else if (_timer >= _controller.PatrolWaitTime)
        {
            _stateMachine.ChangeState(_stateMachine.PatrolState);
        }
    }

    public void Exit() { }
}