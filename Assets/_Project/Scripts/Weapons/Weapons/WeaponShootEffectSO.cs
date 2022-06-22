using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShootEffect_", menuName = "Scriptable Objects/Effects/Weapon Shoot Effect")]
public class WeaponShootEffectSO : ScriptableObject
{
    [Header("WEAPON SHOOT EFFECT DETAILS")]
    public Gradient colorGradient;
    public float duration = 0.5f;
    public float startParticleSize = 0.25f;
    public float startParticleSpeed = 3f;
    public float startParticleLifeTime = 0.5f;
    public int maxParticleNumber = 100;
    public int emissionRate = 100;
    public int burstParticleNumber = 20;
    public float effectGravity;
    public Sprite sprite;
    public Vector3 velocityOverLifeTimeMin;
    public Vector3 velocityOverLifeTimeMax;
    public GameObject weaponShootEffectPrefab;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootEffectPrefab), weaponShootEffectPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(duration), duration, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(emissionRate), emissionRate, true);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber, false);
    }
#endif
    #endregion
}
