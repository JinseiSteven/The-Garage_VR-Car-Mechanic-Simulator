using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenHood : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [Header("Hood Settings")]
    [SerializeField] private Rigidbody hoodRigidbody; // Reference to the hood's rigidbody
    [SerializeField] private float popForce = 0.00001f; // Force to pop the hood
    [SerializeField] private LockHingeAtAngle lockHingeComponent; // Add this field

    private HingeJoint _hoodHinge;
    private bool hasPopped = false;
    private Rigidbody leverRigidbody;

    protected override void Awake()
    {
        base.Awake();
        
        leverRigidbody = GetComponent<Rigidbody>();
        if (leverRigidbody != null)
        {
            leverRigidbody.isKinematic = true;  // Make it static by default
            leverRigidbody.useGravity = false;  // Disable gravity
        }

        if (hoodRigidbody != null)
        {
            _hoodHinge = hoodRigidbody.GetComponent<HingeJoint>();
            // Get the LockHingeAtAngle component if not set in inspector
            if (lockHingeComponent == null)
            {
                lockHingeComponent = hoodRigidbody.GetComponent<LockHingeAtAngle>();
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (leverRigidbody != null)
        {
            leverRigidbody.isKinematic = false;
        }
        PopHood();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    
        if (leverRigidbody != null)
        {
            leverRigidbody.isKinematic = true;
        }
    }

    private void PopHood()
    {
        // Pop once
        if (hoodRigidbody == null || hasPopped) return;
        
        hoodRigidbody.AddForce(Vector3.up * popForce, ForceMode.Impulse);
        
        // Instead of directly modifying the hinge limits, we'll let LockHingeAtAngle handle it
        if (lockHingeComponent != null)
        {
            lockHingeComponent.SetInitialAngle(-3f);
        }

        // Play the hood release sound
        AudioManager.Play("push_hood_release", transform.position, isNarration:false);

        hasPopped = true;
    }
}
