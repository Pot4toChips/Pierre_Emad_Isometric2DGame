using UnityEngine;

public class PlayerRunState : IState
{
    private readonly PlayerStateMachine _stateMachine;

    public PlayerRunState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("[Run State]: Entered");

    }


    public void FixedExecute() 
    {
        // ----- Core Logic -----
        // Update Direction by input at real-time.
        _stateMachine.PlayerController.PlayerMovement.SetRunMovement(InputManager.Instance.MoveValue);
        // Apply the movement.
        _stateMachine.PlayerController.PlayerMovement.Move();
        // Update sprite animation.
        _stateMachine.PlayerController.PlayerAnimator.SetFload("MoveY", InputManager.Instance.MoveValue.y);
    }

    public void Execute()
    {
        // ----- Transitions -----
        // Check the sqrMagnitude instead of Magnitude for better performance.
        if (InputManager.Instance.MoveValue.sqrMagnitude < 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
        else if (!InputManager.Instance.RunIsPressing)
        {
            // Return to walk
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
        else if (InputManager.Instance.DodgePressed)
        {
            _stateMachine.ChangeState(_stateMachine.DodgeState);
        }
    }

    public void Exit()
    {

    }
}
