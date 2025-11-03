using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [Header("Hit Feedback (Juice)")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private int flashCount = 2; // Number of flashes
    [SerializeField] private float flashDuration = 0.1f; // Total duration of one flash (red/white)
    [SerializeField] private float punchScaleAmount = 1.2f; // How big to scale

    private int _currentHealth;
    // --- Component References ---
    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalScale;
    private Color _originalColor;
    private Coroutine _hitFeedbackCoroutine;

    // We can use this to see if something is dead
    public bool IsDead { get; private set; }

    private void Awake()
    {
        // as the sprite is often on a child object.
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + " missing a SpriteRenderer!", this);
        }

        _originalScale = transform.localScale;
        _originalColor = _spriteRenderer.color; // Store the original color
    }
    private void Start()
    {
        _currentHealth = maxHealth;
        IsDead = false;
    }

    /// <summary>
    /// This is the public method all other scripts will call.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (IsDead) return; // Don't beat a dead body

        _currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health is now " + _currentHealth);


        // --- Start the Hit Feedback ---
        // Stop any existing coroutine to prevent bugs
        if (_hitFeedbackCoroutine != null)
        {
            StopCoroutine(_hitFeedbackCoroutine);
            // Reset to original state in case it was interrupted
            ResetVisuals();
        }
        _hitFeedbackCoroutine = StartCoroutine(HitFeedbackCoroutine());
        // ------------------------------

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        Debug.Log(gameObject.name + " has died!");

        // Stop all feedback when dead
        if (_hitFeedbackCoroutine != null)
        {
            StopCoroutine(_hitFeedbackCoroutine);
        }
        // Make the character "blink out"
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.localScale = Vector3.zero;

        Destroy(gameObject, 2f); // Destroy after 2 seconds
    }

    /// <summary>
    /// This coroutine handles the visual "punch" and flash.
    /// </summary>
    private IEnumerator HitFeedbackCoroutine()
    {
        Vector3 punchScale = _originalScale * punchScaleAmount;
        float halfFlashDuration = flashDuration / 2;

        for (int i = 0; i < flashCount; i++)
        {
            // --- Phase 1: Punch Out (Red) ---
            transform.localScale = punchScale;
            _spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(halfFlashDuration);

            // --- Phase 2: Return (White) ---
            transform.localScale = _originalScale;
            _spriteRenderer.color = Color.white; // Flash to white
            yield return new WaitForSeconds(halfFlashDuration);
        }

        // --- Cleanup: Reset to original ---
        ResetVisuals();
        _hitFeedbackCoroutine = null; // Mark coroutine as finished
    }

    /// <summary>
    /// Helper method to reset visuals to their original state.
    /// </summary>
    private void ResetVisuals()
    {
        transform.localScale = _originalScale;
        _spriteRenderer.color = _originalColor;
    }
}