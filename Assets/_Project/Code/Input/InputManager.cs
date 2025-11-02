using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static InputManager Instance { get; private set; }

    [Header("Input Buffering")]
    [SerializeField] private float _inputBufferTime = 0.2f;

    public Vector2 MoveValue => _moveDirection;
    public Vector2 LastMoveValue => _lastMoveDirection;
    public Vector2 LookValue => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool RunIsPressing =>
        _inputActions.Player.Run.IsPressed();
    public bool AttackPressed => 
        Time.time - _attackBufferTimestamp <= _inputBufferTime;
    public bool DodgePressed =>
        Time.time - _dodgeBufferTimestamp <= _inputBufferTime;

    private InputActionsMap _inputActions;
    private float _attackBufferTimestamp  = -1000; // initial dummy value away of 0
    private float _dodgeBufferTimestamp  = -1000; // initial dummy value away of 0
    private Vector2 _moveDirection;
    private Vector2 _lastMoveDirection;

    private void Awake()
    {
        // --- Singleton Setup ---
        if (Instance != null && Instance != this)
        {
            // If an instance already exists, destroy this one.
            Destroy(gameObject);
            return;
        }
        // This is the one and only instance.
        Instance = this;

        // Creating an instance of the action map.
        _inputActions = new InputActionsMap();
    }
    private void OnEnable()
    {
        // ----- Register Event -----
        // Move Action Event
        _inputActions.Player.Move.performed += MovePerformed;
        _inputActions.Player.Move.canceled += MovePerformed;
        // process the input buffer technique.
        _inputActions.Player.Attack.performed += AttackPerformed;
        _inputActions.Player.Dodge.performed += DodgePerformed;
    }
    private void OnDisable()
    {
        // Unregister events to avoid memory leaks.
        _inputActions.Player.Move.performed -= MovePerformed;
        _inputActions.Player.Move.canceled -= MovePerformed;
        _inputActions.Player.Attack.performed -= AttackPerformed;
        _inputActions.Player.Dodge.performed -= DodgePerformed;
    }

    private void Start()
    {
        // Enabling the gameplay controls on begin.
        _inputActions.Player.Enable();
    }

    public void ResetAttackTimestamp()
    {
        _attackBufferTimestamp = -1000;
    }
    public void ResetDodgeTimestamp()
    {
        _dodgeBufferTimestamp = -1000;
    }

    private void MovePerformed(InputAction.CallbackContext obj)
    {
        _lastMoveDirection = _moveDirection;
        _moveDirection = obj.ReadValue<Vector2>();
    }

    private void AttackPerformed(InputAction.CallbackContext obj)
    {
        _attackBufferTimestamp = Time.time;
    }

    private void DodgePerformed(InputAction.CallbackContext obj)
    {
        _dodgeBufferTimestamp = Time.time;
    }
}
