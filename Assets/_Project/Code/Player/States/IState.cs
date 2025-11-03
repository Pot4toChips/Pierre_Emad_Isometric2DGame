/// <summary>
/// A universal interface for all states in our State Machine.
/// </summary>
public interface IState
{
    /// <summary>
    /// Called once when the state machine transitions to this state.
    /// </summary>
    void Enter();

    /// <summary>
    /// Called every physics frame by the state machine while this state is active.
    /// </summary>
    void FixedExecute();

    /// <summary>
    /// Called every frame by the state machine while this state is active.
    /// </summary>
    void Execute();

    /// <summary>
    // Called once when the state machine transitions out of this state.
    /// </summary>
    void Exit();
}