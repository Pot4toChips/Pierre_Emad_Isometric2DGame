using UnityEngine;

public class PlayerDodgeState : IState
{
    private readonly PlayerStateMachine _stateMachine;

    private readonly float _dodgeSpeed = 10;
    private readonly float _dodgeTime = 0.2f;
    private float _elapsedTime = 0;
    private Vector2 _dodgeDirection;

    public PlayerDodgeState(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _elapsedTime = 0;
        // Store the dodge direction to move along it.
        _dodgeDirection = InputManager.Instance.MoveValue;
        // Stops the player (igore any previous forces).
        _stateMachine.PlayerController.PlayerMovement.SetIdleMovement();
        // Reset the dodge timestamp to avoid returning back after the state ends.
        InputManager.Instance.ResetDodgeTimestamp();
    }

    public void FixedExecute()
    {
        _stateMachine.PlayerController.PlayerMovement.ForceSetVelocity(
            _dodgeDirection.normalized * _dodgeSpeed);

        _elapsedTime += Time.fixedDeltaTime;
    }

    public void Execute()
    {
        if (_elapsedTime >= _dodgeTime)
        {
            _stateMachine.ReturnToPreviousState();
        }
    }

    public void Exit()
    {

    }
}
