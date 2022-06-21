using UnityEngine;

public interface IFireable
{
    void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, 
    Vector3 weaponAimDirection, bool overrideAmmoMovement = false);

    GameObject GetGameObject();
}