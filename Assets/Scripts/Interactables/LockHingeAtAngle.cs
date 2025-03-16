using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LockHingeAtAngle : MonoBehaviour
{
    [SerializeField] private GameObject handle; // Object with the fixed joint 
    [SerializeField] private float minAngle = -55f; 
    [SerializeField] private float maxAngle = 0f;
    [SerializeField] private float lockAngleThreshold = -40f;
    [SerializeField] private bool startLocked = false;
    private HingeJoint _hingeJoint;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isLocked = false;
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
         if (startLocked)
        {
            LockHinge();
        }

        originalSpring = _hingeJoint.spring;
    }
 
    void Update()
    {
        if (!isLocked && _hingeJoint.angle <= lockAngleThreshold)
        {
            LockHinge();
        }
    }

    private void LockHinge()
    {

        GameStateManager.Instance.NotifyTaskCompleted("Open hood");
        isLocked = true;    

        // Lock the hood in once place so that the player can work in the hood
        JointSpring spring = _hingeJoint.spring;
        spring.spring = 10000f;
        spring.damper = 100f; 
        spring.targetPosition = _hingeJoint.angle;
        _hingeJoint.spring = spring;
        _hingeJoint.useLimits = true; 
    }

    private void UnlockHinge()
    {
       
        isLocked = false;
        _hingeJoint.spring = originalSpring;
        JointLimits limits = _hingeJoint.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        _hingeJoint.limits = limits;
        _hingeJoint.useLimits = true;  
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (isLocked)
        {
            // Wait some time before calling else the hood doenst unlock consistently
            Invoke("UnlockHinge", 0.1f);
        }
    }

    public void SetInitialAngle(float angle)
    {
        maxAngle = angle;
        if (_hingeJoint != null)
        {
            JointLimits limits = _hingeJoint.limits;
            limits.max = angle;
            _hingeJoint.limits = limits;
        }
    }
}

