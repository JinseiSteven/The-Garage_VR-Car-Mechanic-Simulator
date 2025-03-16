using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CompleteTaskOnGrab : MonoBehaviour
{

    public void OnGrab(string taskToComplete)
    {
        GameStateManager.Instance.NotifyTaskCompleted(taskToComplete);
    }

  
}
