using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update() 
    {
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        AmmoHitEffect();

        DisableAmmo();
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirection, bool overrideAmmoMovement = false)
    {
        #region Ammo

        gameObject.SetActive(true);

        this.ammoDetails = ammoDetails;

        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirection);

        spriteRenderer.sprite = ammoDetails.ammoSprite;

        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        ammoRange = ammoDetails.ammoRange;

        this.ammoSpeed = ammoSpeed;
        this.overrideAmmoMovement = overrideAmmoMovement;

        gameObject.SetActive(true);

        #endregion

        #region Trail
            if (ammoDetails.hasAmmoTrail)
            {
                trailRenderer.gameObject.SetActive(true);
                trailRenderer.emitting = true;
                trailRenderer.material = ammoDetails.ammoTrailMaterial;
                trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
                trailRenderer.endWidth = ammoDetails.ammoTrailEndWith;
                trailRenderer.time = ammoDetails.ammoTrailTime;
            }
            else
            {
                trailRenderer.emitting = false;
                trailRenderer.gameObject.SetActive(false);
            }
        #endregion
    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirection)
    {
        float randomSpread = UnityEngine.Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        int spreadToggle = UnityEngine.Random.Range(0, 2) * 2 - 1;

        fireDirectionAngle = aimAngle;

        //if (weaponAimDirection.magnitude < Settings.useAimAngleDistance)
        //{
        //    fireDirectionAngle = aimAngle;
        //}
        //else
        //{
        //    fireDirectionAngle = weaponAimAngle;
        //}

        fireDirectionAngle += spreadToggle * randomSpread;

        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);

        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    private void AmmoHitEffect()
    {
        if (ammoDetails.ammoHitEffect == null && ammoDetails.ammoHitEffect.ammoHitEffectPrefab == null) { return; }

        AmmoHitEffect hitEffect = (AmmoHitEffect)PoolManager.Instance.ReuseComponent(ammoDetails.ammoHitEffect.ammoHitEffectPrefab,
            transform.position, Quaternion.identity);

        hitEffect.SetShootEffect(ammoDetails.ammoHitEffect);
        hitEffect.gameObject.SetActive(true);
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    private void SetAmmoMaterial(Material ammoMaterial)
    {
        spriteRenderer.material = ammoMaterial;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);    
    }

#endif
    #endregion
}