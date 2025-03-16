using UnityEngine;

public class DrPeppaBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pourEmitter;
    [SerializeField] private float pourThreshold = 85.0f;
    [SerializeField] private bool isPouring = false;
    [SerializeField] private bool isOpen = true;

    private void Update()
    {
        float objectAngle = Vector3.Angle(transform.up, Vector3.down);

        if (isPouring) 
        {
            if (objectAngle > pourThreshold)
            {
                isPouring = false;
                pourEmitter.SetActive(false);
            }
        }
        else if (objectAngle < pourThreshold && isOpen)
        {
            isPouring = true;
            pourEmitter.SetActive(true);
        }
    }

    public void OpenContainer()
    {
        isOpen = true;
    }

    public void CloseContainer()
    {
        isOpen = false;
        // if the user is pouring and closes the container, we also need to stop the pouring
        if (isPouring)
        {
            isPouring = false;
            pourEmitter.SetActive(false);
        }
    }
}