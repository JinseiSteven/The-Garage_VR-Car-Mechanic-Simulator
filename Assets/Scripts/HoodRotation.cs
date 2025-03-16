using UnityEngine;
using UnityEngine.InputSystem;


public class HoodRotation : MonoBehaviour
{
    private float currentAngle = 0f;
    private float poppedHoodAngle = 2f;
    private float rotationSpeed = 25f;
    JointLimits rotationLimits = new JointLimits
    {
        min = 0f,
        max = 65f
    };
    private Vector3 pivotPoint;
    private Vector3 rotationAxis;
    private bool isPopping = false;
    private bool isPopped = false;
    private HingeJoint hoodHingeJoint;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private InputAction popHoodAction;
    private InputAction resetHoodAction;

    void Awake()
    {
        // Initialize input actions for testing
        popHoodAction = new InputAction("PopHood", binding: "<XRController>{RightHand}/secondaryButton"); // <Keyboard>/p
        popHoodAction.performed += ctx => StartPopHood();
        popHoodAction.Enable();

        resetHoodAction = new InputAction("ResetHood", binding: "<XRController>{RightHand}/secondaryButton"); // <Keyboard>/r
        resetHoodAction.performed += ctx => ResetHood();
        resetHoodAction.Enable();

        // Get the HingeJoint component
        hoodHingeJoint = GetComponent<HingeJoint>();
        if (hoodHingeJoint == null)
        {
            Debug.LogError("HingeJoint component is missing!");
        }

        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing!");
        }
        else
        {
            // Disable grabbing until popped
            grabInteractable.enabled = false;
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Vector3 carCenter = transform.parent.position;
            pivotPoint = renderer.bounds.ClosestPoint(carCenter);
            rotationAxis = Vector3.Cross(Vector3.up, pivotPoint - carCenter).normalized;

            hoodHingeJoint.anchor = pivotPoint;
            hoodHingeJoint.axis = rotationAxis;
            hoodHingeJoint.limits = rotationLimits;
            hoodHingeJoint.useLimits = true;
        }
        else
        {
            Debug.LogError("MeshRenderer component is missing!");
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (isPopping)
        {
            popHood();
        }
    }

    private void StartPopHood()
    {
        if (currentAngle < poppedHoodAngle && !isPopped)
        {
            isPopping = true;
        }
    }

    private void popHood()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;
        float remainingAngle = poppedHoodAngle - currentAngle;
        float angleToRotate = Mathf.Min(rotationStep, remainingAngle);
        transform.RotateAround(pivotPoint, rotationAxis, -angleToRotate);
        currentAngle += angleToRotate;

        if (Mathf.Approximately(currentAngle, poppedHoodAngle) || currentAngle >= poppedHoodAngle)
        {
            isPopping = false;
            isPopped = true;
            grabInteractable.enabled = true; // Allow grabbing
        }
    }

    private void ResetHood()
    {
        if (currentAngle > 0f)
        {
            transform.RotateAround(pivotPoint, rotationAxis, currentAngle);
            currentAngle = 0f;
            isPopped = false;
            grabInteractable.enabled = false; // Disable grabbing
        }
    }

    void OnDisable()
    {
        popHoodAction.Disable();
        resetHoodAction.Disable();
    }
}
