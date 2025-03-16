using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableScriptOnSelect : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor; 
    public MonoBehaviour scriptToDisable;      

    void OnEnable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnObjectSelected);
        }
    }

    void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnObjectSelected);
        }
    }

    private void OnObjectSelected(SelectEnterEventArgs args)
    {
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = false; 
            Debug.Log($"{scriptToDisable.GetType().Name} has been disabled.");
        }
    }
}
