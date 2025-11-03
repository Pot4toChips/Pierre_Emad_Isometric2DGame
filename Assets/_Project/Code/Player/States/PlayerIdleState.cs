using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly PlayerStateMachine _stateMachine;

    public PlayerIdleState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter() 
    {
        // Stop Player Movement.
        _stateMachine.PlayerController.PlayerMovement.SetIdleMovement();
        _stateMachine.PlayerController.PlayerAnimator.Play("Idle");
        // Getting the last move direction before idle.
        // to keep facing the same direction.
        _stateMachine.PlayerController.PlayerAnimator.SetFloat("MoveY", InputManager.Instance.LastMoveValue.y);
    }

    public void FixedExecute()
    {
        // Move by zero (Force Stop).
        _stateMachine.PlayerController.PlayerMovement.Move();
    }

    public void Execute()
    {
        if (InputManager.Instance.AttackPressed)
        {
            _stateMachine.PlayerController.PlayerCombat.PerformAttack();
        }

        // ----- Transitions -----
        // Check the sqrMagnitude instead of Magnitude for better performance.
        if (InputManager.Instance.MoveValue.sqrMagnitude > 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }

    public void Exit() {  }
}