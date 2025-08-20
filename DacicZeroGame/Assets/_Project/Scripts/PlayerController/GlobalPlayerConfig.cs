/// <summary>
/// Global player settings. Things like player speed coefficient and interaction distance. 
/// </summary>
public static class GlobalPlayerConfig
{
    public static float PlayerGroundCheckRadius { get; set; } = 0.4f;
    /// <summary>
    /// How fast can the player move.
    /// </summary>
    public static float PlayerSpeed { get; set; } = 10;
    /// <summary>
    /// How much force to apply when jumping.
    /// </summary>
    public static float JumpForce { get; set; } = 10;
    /// <summary>
    /// How much gravity to apply downwards when not grounded.
    /// </summary>
    public static float Gravity { get; set; } = .35f;
    /// <summary>
    /// At what distance can the player interact with an interactable.
    /// </summary>
    public static float InteractionDistance { get; set; } = 3;
    /// <summary>
    /// What the player can walk on. 1<<0 | 1<<1 would take the first two layers.
    /// </summary>
    public static int GroundLayerMask { get; set; } = 1 << 0;
}