using UnityEngine;

/// <summary>
/// Player Control Panel, that controls all the integrated player systems.
/// </summary>
[RequireComponent (typeof(PlayerMovement), typeof(PlayerAnimator))]
[RequireComponent (typeof(PlayerCombat))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerAnimator PlayerAnimator { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimator = GetComponent<PlayerAnimator>();
        PlayerCombat = GetComponent<PlayerCombat>();
    }
}
