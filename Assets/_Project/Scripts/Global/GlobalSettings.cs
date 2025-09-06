using System.Collections.Generic;
using UnityEngine;

public static class GlobalSettings
{
    #region AI stuff
    /// <summary>
    /// What layers should each team check? Bots should check for humies and viceversa.
    /// </summary>
    public static Dictionary<int, LayerMask> DetectionMasks { get; } = new() {
        {6, 1 << 7},
        {7, 1 << 6}
    };
    /// <summary>
    /// Like above but for guns instead of eyes.
    /// </summary>
    public static Dictionary<int, LayerMask> TargetMasks { get; } = new() {
        {6, 1 << 7 | 1 << 0},
        {8, 1 << 7 | 1 << 0},
        {7, 1 << 6 | 1 << 0}
    };
    /// <summary>
    /// Maximum number of targets AI detection should gather.
    /// </summary>
    public static int MaxTargets { get; private set; } = 20;
    #endregion
    #region Pew pew stuff
    /// <summary>
    /// How far should raycasts go when shooting?
    /// </summary>
    public static float MaxRaycastDist { get; } = 100;
    #endregion
    #region NPC stuff
    public static float SkillDiffCoefficient { get; private set; } = 0.2f;
    #endregion
}