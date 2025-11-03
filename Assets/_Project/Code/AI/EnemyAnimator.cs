using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _renderer = _animator.GetComponent<SpriteRenderer>();
    }

    public void UpdateSpriteToTarget(Transform target)
    {
            _renderer.flipX = target.position.x < transform.position.x;
    }

    public void UpdateColor(Color color)
    {
        _renderer.color = color;
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
