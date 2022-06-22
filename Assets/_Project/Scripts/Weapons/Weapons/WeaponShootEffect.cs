using UnityEngine;

[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{
    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {
        shootEffectParticleSystem = GetComponent<ParticleSystem>();
    }

    public void SetShootEffect(WeaponShootEffectSO shootEffect, float aimAngle)
    {
        SetShootEffectGradient(shootEffect.colorGradient);

        SetShootEffectParticleStartingValues(shootEffect.duration, shootEffect.startParticleSize, shootEffect.startParticleSpeed, shootEffect.startParticleLifeTime,
            shootEffect.effectGravity, shootEffect.maxParticleNumber);

        SetShootEffectParticleEmission(shootEffect.emissionRate, shootEffect.burstParticleNumber);

        SetEmitterRotation(aimAngle);

        SetShootEffectParticleSprite(shootEffect.sprite);

        SetShootEffectVelocityOverLifeTime(shootEffect.velocityOverLifeTimeMin, shootEffect.velocityOverLifeTimeMax);
    }

    private void SetShootEffectVelocityOverLifeTime(Vector3 velocityOverLifeTimeMin, Vector3 velocityOverLifeTimeMax)
    {
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

        ParticleSystem.MinMaxCurve minMaxCurveX = new();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = velocityOverLifeTimeMin.x;
        minMaxCurveX.constantMax = velocityOverLifeTimeMax.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        ParticleSystem.MinMaxCurve minMaxCurveY = new();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = velocityOverLifeTimeMin.y;
        minMaxCurveY.constantMax = velocityOverLifeTimeMax.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        ParticleSystem.MinMaxCurve minMaxCurveZ= new();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = velocityOverLifeTimeMin.z;
        minMaxCurveZ.constantMax = velocityOverLifeTimeMax.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }

    private void SetShootEffectParticleSprite(Sprite sprite)
    {
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);
    }

    private void SetEmitterRotation(float aimAngle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);
    }

    private void SetShootEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {
        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        ParticleSystem.Burst burst = new(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        emissionModule.rateOverTime = emissionRate;
    }

    private void SetShootEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startParticleLifeTime, float effectGravity, int maxParticleNumber)
    {
        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;

        mainModule.duration = duration;
        mainModule.startSize = startParticleSize;
        mainModule.startSpeed = startParticleSpeed;
        mainModule.startLifetime = startParticleLifeTime;
        mainModule.gravityModifier = effectGravity;
        mainModule.maxParticles = maxParticleNumber;
    }

    private void SetShootEffectGradient(Gradient gradient)
    {
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;
    }
}
