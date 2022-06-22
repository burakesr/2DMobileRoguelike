using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    #region Variables

    [Header("BASIC AMMO DETAILS")]
    public string ammoName;
    public bool isPlayerAmmo;

    [Space(10)]
    [Header("AMMO SPRITE, PREFAB & MATERIAL")]
    public Sprite ammoSprite;
    public GameObject[] ammoPrefabs;

    public Material ammoMaterial;
    public float ammoChargeTime = 0.1f;
    public Material ammoChargeMaterial;

    [Space(10)]
    [Header("AMMO HIT EFFECT")]
    public AmmoHitEffectSO ammoHitEffect;

    [Space(10)]
    [Header("AMMO BASE PARAMETERS")]
    public int ammoDamage = 1;
    public float ammoMinSpeed;
    public float ammoMaxSpeed;
    public float ammoRange = 20;
    public float ammoRotationSpeed = 1f;

    [Space(10)]
    [Header("AMMO SPREAD PARAMETERS")]
    public float ammoSpreadMin = 0f;
    public float ammoSpreadMax = 0f;

    [Space(10)]
    [Header("AMMO SPAWN DETAILS")]
    public int ammoSpawnAmountMin = 1;
    public int ammoSpawnAmountMax = 1;
    public float ammoSpawnIntervalMin = 0f;
    public float ammoSpawnIntervalMax = 0f;
    
    [Space(10)]
    [Header("AMMO TRAIL DETAILS")]
    public bool hasAmmoTrail = false;
    public float ammoTrailTime = 1f;
    public Material ammoTrailMaterial;
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    [Range(0f, 1f)] public float ammoTrailEndWith;

    #endregion
    
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoSprite), ammoSprite);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoPrefabs), ammoPrefabs);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoMaterial), ammoMaterial);
        if (ammoChargeTime > 0)
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoDamage), ammoDamage, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoMinSpeed), ammoMinSpeed, nameof(ammoMaxSpeed), ammoMaxSpeed, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoRange), ammoRange, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpreadMin), ammoSpreadMin, nameof(ammoSpreadMax), ammoSpreadMax, true);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnAmountMin), ammoSpawnAmountMin, nameof(ammoSpawnAmountMax), ammoSpawnAmountMax, false);
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpawnIntervalMin), ammoSpawnIntervalMin, nameof(ammoSpawnIntervalMax), ammoSpawnIntervalMax, true);
        if (hasAmmoTrail)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWith), ammoTrailEndWith, false);
        }
    }
#endif
    #endregion
}
