using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = _animator.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Flip the sprite based on the X axis input.
        // If no input, just keep the last frame flip state.
        if (InputManager.Instance.MoveValue.sqrMagnitude < 0.01f)
            _renderer.flipX = InputManager.Instance.LastMoveValue.x < 0;
        else
            _renderer.flipX = InputManager.Instance.MoveValue.x < 0;
    }

    public void Play(string animationName)
    {
        _animator.Play(animationName);
    }
    public void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

}
