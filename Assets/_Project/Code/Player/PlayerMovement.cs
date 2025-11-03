using UnityEngine;

// Ensure this component is on a GameObject with a Rigidbody2D.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The maximum walk speed the player can move in.")]
    [SerializeField] private float _maxWalkSpeed = 3f;
    [Tooltip("The maximum run speed the player can move in.")]
    [SerializeField] private float _maxRunSpeed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private float _currentSpeed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves the player by setting the rigidbody linear velocity.
    /// </summary>
    public void Move()
    {
        // --- Core Movement Logic ---

        // Task 2 asked for "instant acceleration".
        // Setting the velocity directly achieves this.

        // We get nomralized input direction, but I renormalize again -
        // just in case of after-input modifications.
        Vector2 targetVelocity = _moveDirection.normalized * _currentSpeed;

        // Setting the rigidbody's linear velocity.
        _rigidbody.linearVelocity = targetVelocity;
    }

    /// <summary>
    /// The Player State Machine will call this to make the player walk.
    /// </summary>
    /// <param name="direction">The direction to move, 
    /// usually from the InputManager.</param>
    public void SetWalkMovement(Vector2 direction)
    {
        _moveDirection = direction;
        _currentSpeed = _maxWalkSpeed;
    }
    /// <summary>
    /// The Player State Machine will call this to make the player run.
    /// </summary>
    /// <param name="direction">The direction to move, 
    /// usually from the InputManager.</param>
    public void SetRunMovement(Vector2 direction)
    {
        _moveDirection = direction;
        _currentSpeed = _maxRunSpeed;
    }
    /// <summary>
    /// The Player State Machine will call this to make the player stop.
    /// </summary>
    public void SetIdleMovement()
    {
        _moveDirection = Vector2.zero;
        _currentSpeed = 0;
    }
    /// <summary>
    /// For the states that must force change the velocity (like Dodge).
    /// </summary>
    /// <param name="velocity"></param>
    public void ForceSetVelocity(Vector2 velocity)
    {
        _rigidbody.linearVelocity = velocity;
    }
}