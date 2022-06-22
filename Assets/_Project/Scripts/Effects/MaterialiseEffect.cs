using System.Collections;
using UnityEngine;

public class MaterialiseEffect : MonoBehaviour
{
    public IEnumerator MaterialiseRoutine(Shader materialiseShader, Color materialiseColor, float materialiseTime, 
        SpriteRenderer[] sprites, Material standardMaterial)
    {
        Material materialiseMaterial = new Material(materialiseShader);

        materialiseMaterial.SetColor("_EmissionColor", materialiseColor);

        ChangeMaterial(sprites, materialiseMaterial);

        float dissolveAmount = 0f;

        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime / materialiseTime;

            materialiseMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        } 

        ChangeMaterial(sprites, standardMaterial);

    }

    private static void ChangeMaterial(SpriteRenderer[] sprites, Material material)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.material = material;
        }
    }
}
