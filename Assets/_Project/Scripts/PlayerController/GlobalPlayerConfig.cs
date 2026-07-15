/// <summary>
/// Global player settings. Things like player speed coefficient and interaction distance. 
/// </summary>
public static class GlobalPlayerConfig
{
    public static float PlayerGroundCheckRadius { get; set; } = 0.4f;
    /// <summary>
    /// How fast can the player move.
    /// </summary>
    public static float PlayerSpeed { get; set; } = 4;
    /// <summary>
    /// How much to multiply the speed by when player is crouching.
    /// </summary>
    public static float PlayerCrouchSpeedMultiplier { get; set; } = 0.5f;
    /// <summary>
    /// How much force to apply when jumping.
    /// </summary>
    public static float JumpForce { get; set; } = 7f;
    /// <summary>
    /// How much gravity to apply downwards when not grounded.
    /// </summary>
    public static float Gravity { get; set; } = 20f;
    /// <summary>
    /// At what distance can the player interact with an interactable.
    /// </summary>
    public static float InteractionDistance { get; set; } = 3;
    /// <summary>
    /// What the player can walk on. 1<<0 | 1<<1 would take the first two layers.
    /// </summary>
    public static int GroundLayerMask { get; set; } = 1 << 0;
}