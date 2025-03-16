using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FluidShaderBehaviour : MonoBehaviour
{
    private Material material;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        // making the material a new INSTANCE of the material (so we can have multiple objects without interferance)
        if (material == null)
        {
            material = new Material(objectRenderer.material);
        }

        objectRenderer.material = material;
        UpdateBounds();
    }

    void UpdateBounds()
    {
        // we check whether the object has a material and whether or not it has actually rotated
        if (material != null)
        {
            Bounds bounds = objectRenderer.bounds;

            material.SetFloat("_MinWorldY", bounds.min.y);
            material.SetFloat("_MaxWorldY", bounds.max.y);
        }
    }

    void Update()
    {
        UpdateBounds();
    }
}

