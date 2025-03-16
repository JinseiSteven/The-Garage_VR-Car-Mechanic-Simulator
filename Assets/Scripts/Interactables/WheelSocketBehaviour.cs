using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WheelSocketBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject interactiveWheel;

    // magic trick to switch out the fake wheel with the real one
    public void OnWheelReceived()
    {
        IXRSelectInteractable wheelObject = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
        wheelObject.transform.gameObject.SetActive(false);
        interactiveWheel.SetActive(true);
    }
}
