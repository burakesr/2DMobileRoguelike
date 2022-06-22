using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyWeaponAI : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform weaponShootPosition;

    private Enemy enemy;
    private EnemyDetailsSO enemyDetails;

    private float firingIntervalTimer;
    private float firingDurationTimer;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemyDetails = enemy.EnemyDetails;

        firingDurationTimer = WeaponShootDuration();
        firingIntervalTimer = WeaponShootInterval();
    }

    private void Update()
    {
        firingIntervalTimer -= Time.deltaTime;

        if (firingIntervalTimer < 0)
        {
            if (firingDurationTimer > 0)
            {
                firingDurationTimer -= Time.deltaTime;

                FireWeapon();
            }
            else
            {
                firingDurationTimer = WeaponShootDuration();
                firingIntervalTimer = WeaponShootInterval();
            }
        }
    }

    private void FireWeapon()
    {
        Vector3 targetDirectionVector = GameManager.Instance.GetPlayer().GetPlayerPosition() - transform.position;

        Vector3 weaponDirection = (GameManager.Instance.GetPlayer().GetPlayerPosition() - weaponShootPosition.position);

        float weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);
        float enemyAngleDegrees = HelperUtilities.GetAngleFromVector(targetDirectionVector);

        AimDirection aimDirection = HelperUtilities.GetAimDirection(enemyAngleDegrees);

        enemy.AimWeaponEvent.CallAimWeaponEvent(aimDirection, enemyAngleDegrees, weaponAngleDegrees, weaponDirection);

        if (enemyDetails.enemyWeapon != null)
        {
            float ammoRange = enemyDetails.enemyWeapon.weaponCurrentAmmo.ammoRange;

            if (targetDirectionVector.magnitude <= ammoRange)
            {
                if (enemyDetails.firingLineOfSightRequired && !IsTargetInLineOfSight(weaponDirection, ammoRange)) return;

                enemy.FireWeaponEvent.CallFireWeaponEvent(true, true, aimDirection, enemyAngleDegrees, weaponAngleDegrees, weaponDirection);
            }
        }
    }

    private bool IsTargetInLineOfSight(Vector3 weaponDirection, float ammoRange)
    {
        RaycastHit2D hit = Physics2D.Raycast(weaponShootPosition.position, (Vector2)weaponDirection, ammoRange, targetLayer);

        if (hit && hit.transform.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    private float WeaponShootInterval() => Random.Range(enemyDetails.fireIntervalMin, enemyDetails.fireIntervalMax);

    private float WeaponShootDuration() => Random.Range(enemyDetails.firingDurationMin, enemyDetails.firingDurationMax);

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootPosition), weaponShootPosition);
    }
#endif
    #endregion
}
