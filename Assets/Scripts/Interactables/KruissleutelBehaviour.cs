using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KruissleutelBehaviour : XRBaseInteractable
{
    [SerializeField] private Transform pivotTransform;

    public UnityEvent<float> OnRotate;

    private float angleBuffer = 0.0f;

    private void Rotate()
    {
        // getting the current angle and comparing it to the buffered angle we saved
        float currentAngle = GetCurAngle();
        float angleOffset = angleBuffer - currentAngle;
        
        // actually rotating the model and storing the new angle
        pivotTransform.Rotate(transform.forward, -angleOffset, Space.World);
        angleBuffer = currentAngle;

        // lastly, we call all listening functions (in our case just the screwing)
        OnRotate?.Invoke(angleOffset);
    }

    private float GetCurAngle()
    {
        float totalAngle = 0;

        // we check whether there is a hand interacting
        if (interactorsSelecting.Count >= 1)
        {
            // then we get the world position of the hand and compare it to the static "up" direction to calculate the angle
            IXRSelectInteractor interactor = interactorsSelecting[0];
            Vector2 direction = transform.InverseTransformPoint(interactor.transform.position).normalized;

            float handAngle = Vector2.SignedAngle(Vector2.up, direction);
            totalAngle = handAngle;
        }

        return totalAngle;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        angleBuffer = GetCurAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        angleBuffer = GetCurAngle();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
                Rotate();
        }
    }
}