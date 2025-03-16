using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LockHingeAtTop : MonoBehaviour
{
    [SerializeField] private GameObject handle; // Object with the fixed joint 
    private HingeJoint _hingeJoint;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isLocked = false;
    private float lockAngleThreshold = -40f; 
    private JointSpring originalSpring;

    void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>(); 
        
        if (handle != null)
        {
            grabInteractable = handle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(OnGrab);
            }
           
        }

        originalSpring = _hingeJoint.spring;
    }

    void Update()
    {
        if (!isLocked && _hingeJoint.angle <= lockAngleThreshold)
        {
            LockHood();
        }
    }

    private void LockHood()
    {
        isLocked = true;    

        // Lock the hood in once place so that the player can work in the hood
        JointSpring spring = _hingeJoint.spring;
        spring.spring = 10000f;
        spring.damper = 100f; 
        spring.targetPosition = _hingeJoint.angle;
        _hingeJoint.spring = spring;
        _hingeJoint.useLimits = true; 
    }

    private void UnlockHood()
    {
        isLocked = false;
        _hingeJoint.spring = originalSpring;
        JointLimits limits = _hingeJoint.limits;
        limits.min = -55f;
        limits.max = 0f;
        _hingeJoint.limits = limits;
        _hingeJoint.useLimits = true;  
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isLocked)
        {
            // Wait some time before calling else the hood doenst unlock consistently
            Invoke("UnlockHood", 0.1f);
        }
    }
}
