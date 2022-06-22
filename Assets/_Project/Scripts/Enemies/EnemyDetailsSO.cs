using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]
    public string enemyName;
    public float chaseDistance = 50f;
    public GameObject enemyPrefab;

    [Space(10)]
    [Header("MATERIALISE SETTINGS")]
    public Material enemyStandardMaterial;
    public float enemyMaterialiseTime = 1f;
    public Shader enemyMaterialiseShader;
    [ColorUsage(true, true)]
    public Color enemyMaterialiseColor;

    [Space(10)]
    [Header("ENEMY WEAPON SETTINGS")]
    public WeaponDetailsSO enemyWeapon;
    public float fireIntervalMin = 0.1f;
    public float fireIntervalMax = 1f;
    public float firingDurationMin = 0.1f;
    public float firingDurationMax = 0.5f;
    public bool firingLineOfSightRequired;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyMaterialiseTime), enemyMaterialiseTime, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMaterialiseShader), enemyMaterialiseShader);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyStandardMaterial), enemyStandardMaterial);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(fireIntervalMin), fireIntervalMin, nameof(fireIntervalMax), fireIntervalMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax), firingDurationMax, false);
    }
#endif
    #endregion
}
