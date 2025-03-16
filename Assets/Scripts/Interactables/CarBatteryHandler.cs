using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CarBatteryHandler : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor blackSocket;
    [SerializeField] private XRSocketInteractor redSocket;

    [SerializeField] private GameObject[] emitters;

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private bool correctlyAttached = false;

    public UnityEvent OnBatteryCharge;

    private void Start()
    {
        foreach (GameObject emitter in emitters)
        {
            particleSystems.Add(emitter.GetComponent<ParticleSystem>());
        }
    }

    public void CheckAnodes()
    {
        InteractionLayerMask blackMask = InteractionLayerMask.GetMask("BlackSnapper");
        InteractionLayerMask redMask = InteractionLayerMask.GetMask("RedSnapper");

        bool redAttached = false;
        bool blackAttached = false;

        // we use an "and" operator to crosscheck the layermasks and see whether the correct object is attached
        if (blackSocket.interactablesSelected.Count >= 1)
        {
            blackAttached = (blackSocket.interactablesSelected[0].interactionLayers & blackMask) != 0;
        }
        if (redSocket.interactablesSelected.Count >= 1)
        {
            redAttached = (redSocket.interactablesSelected[0].interactionLayers & redMask) != 0;
        }

        if (redAttached && blackAttached)
        {
            correctlyAttached = true;
            ShootSparks();
            OnBatteryCharge.Invoke();
        }
    }

    // simple function we can just call in the editor to reset the value (could also use a getter/setter)
    public void ResetAttachments()
    {
        correctlyAttached = false;
    }

    public void ChargeBattery()
    {
        if (correctlyAttached)
        {
            // success
            ShootSparks();
        }
        else
        {
            // death
            ShootSparks();
        }
    }

    private void ShootSparks()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            particleSystem.Play();
        }
    }
}
