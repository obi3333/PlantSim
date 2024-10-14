using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Effects/Crepuscular Rays", -1)]
public class Crepuscular : MonoBehaviour
{
    public Material material;
    public GameObject light;
    public Color rayColor = Color.white;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null && light != null)
        {
            Vector3 lightPos = GetComponent<Camera>().WorldToViewportPoint(light.transform.position);
            material.SetVector("_LightPos", new Vector4(lightPos.x, lightPos.y, lightPos.z, 1));
            material.SetColor("_RayColor", rayColor);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination); // Just pass through if there's no material
        }
    }
}