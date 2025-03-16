using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectHighlightWithXR : MonoBehaviour
{
    private Material originalMaterial; // The original material
    private Renderer objectRenderer;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectRenderer = GetComponentInChildren<Renderer>();

        // Create a unique material instance to avoid shared material issues
        originalMaterial = new Material(objectRenderer.material);

        // Assign the instance back to the object renderer
        objectRenderer.material = originalMaterial;

        // Subscribe to the hover events
        grabInteractable.hoverEntered.AddListener(HoverEntered);
        grabInteractable.hoverExited.AddListener(HoverExited);
    }

    private void HoverEntered(HoverEnterEventArgs arg0)
    {
        HighlightObject(true);  // Change the color to red
    }

    private void HoverExited(HoverExitEventArgs arg0)
    {
        HighlightObject(false);  // Reset to original color
    }

    // Change the material to highlight or reset
    void HighlightObject(bool highlight)
    {
        if (highlight)
        {
            // Change the object's color to red
            objectRenderer.material.color = originalMaterial.color * 1.2f;
        }
        else
        {
            // Reset the color to the original material color
            objectRenderer.material.color = originalMaterial.color;
        }
    }

    // Unsubscribe from events when the object is destroyed
    private void OnDestroy()
    {
        grabInteractable.hoverEntered.RemoveListener(HoverEntered);
        grabInteractable.hoverExited.RemoveListener(HoverExited);
    }
}
