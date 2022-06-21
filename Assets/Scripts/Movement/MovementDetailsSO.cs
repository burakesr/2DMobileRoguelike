using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/Movement/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header MOVEMENT DETAILS
    [Header("MOVEMENT DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("The minimum move speed. The GetMoveSpeed methods calculates a random value between the minimum and the maximum.")]
    #endregion
    public float minMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("The maximum move speed. The GetMoveSpeed methods calculates a random value between the minimum and the maximum.")]
    #endregion
    public float maxMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("If there is roll movement - this is roll speed.")]
    #endregion
    public float rollSpeed; // For player;
    #region Tooltip
    [Tooltip("If there is roll movement - this is roll distance.")]
    #endregion
    public float rollDistance; // For player
    #region Tooltip
    [Tooltip("If there is roll movement - this is roll cooldown.")]
    #endregion
    public float rollCooldown; // For player


    /// <summary>
    /// Return a random move speed between minMoveSpeed and maxMoveSpeed parameters.
    /// </summary>
    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), 
            minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);
        if (rollDistance != 0 || rollCooldown != 0 || rollSpeed != 0)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollSpeed), rollSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollDistance), rollDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollCooldown), rollCooldown, false);
        }
    }
#endif
    #endregion
}
