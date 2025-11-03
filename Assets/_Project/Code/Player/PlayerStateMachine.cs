using UnityEngine;

/// <summary>
/// State Machine Pattern: Controls the player's states
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerStateMachine : MonoBehaviour
{
    // Reference to the player control pannel.
    public PlayerController PlayerController { get; private set; }

    // --- Concrete States ---
    // State properties for easy access
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }

    // --- State Management ---
    private IState _currentState;
    private IState _previousState;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();

        // --- Initialize state "objects" ---
        // Pass 'this' (the state machine) into each state's constructor.
        // Applying Dependency Injection.

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        DodgeState = new PlayerDodgeState(this);
    }

    private void Start()
    {
        // --- Set the initial state ---
        _previousState = IdleState;
        _currentState = IdleState;
        _currentState.Enter();
    }

    private void FixedUpdate()
    {
        // This is the physics loop of the FSM.
        _currentState.FixedExecute();
    }

    private void Update()
    {
        // This is the core loop of the FSM
        _currentState?.Execute();
    }

    /// <summary>
    /// Changing player behaviour based on states.
    /// </summary>
    public void ChangeState(IState newState)
    {
        // Safety Check
        if (newState == null || newState == _currentState) return;

        // Store the current as previous state.
        _previousState = _currentState;

        // Call Exit() on the old state
        _currentState?.Exit();

        // Swap to the new state
        _currentState = newState;

        // Call Enter() on the new state
        _currentState.Enter();
    }
    public void ReturnToPreviousState()
    {
        ChangeState(_previousState);
    }
}