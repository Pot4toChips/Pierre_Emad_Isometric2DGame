using UnityEngine;

public class EnemyAttackState : IState
{
    private readonly EnemyStateMachine _stateMachine;
    private readonly EnemyController _controller;

    private float _attackTimer;

    public EnemyAttackState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _controller = stateMachine.EnemyController;
    }

    public void Enter()
    {
        // --- Bonus Point: Visual Feedback ---
        _controller.EnemyAnimator.UpdateColor(Color.red);

        // Stop moving to perform the attack
        _controller.Movement.Stop();

        _attackTimer = 0f; // Start attack timer
    }

    public void FixedExecute() { }

    public void Execute()
    {
        _attackTimer += Time.deltaTime;

        // Wait for the attack timer to finish
        if (_attackTimer >= _controller.AttackRate)
        {
            // --- PERFORM THE ATTACK ---
            Debug.Log("ENEMY is ATTACKING!");
            // Call The Combat System.

            // --- Transition out of Attack ---
            // After attacking, check if player is still in range
            if (_controller.Sight.IsPlayerInAttackRange)
            {
                // Still in range, reset timer for next attack
                _attackTimer = 0f;
            }
            else
            {
                // Player ran away, go back to chasing
                _stateMachine.ChangeState(_stateMachine.ChaseState);
            }
        }
        else if (!_controller.Sight.IsPlayerInAttackRange)
        {
            // Player ran away *during* the attack windup
            _stateMachine.ChangeState(_stateMachine.ChaseState);
        }
    }

    public void Exit() { }
}