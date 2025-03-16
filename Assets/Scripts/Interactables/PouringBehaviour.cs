using UnityEngine;

public class PouringBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pourEmitter;
    [SerializeField] private float pourThreshold = 85.0f;
    [SerializeField] private Liquid pourLiquid;

    // simple variable to keep track of whether the container is pouring
    [SerializeField] private bool isPouring = false;
    // temporarily unused variable, to check whether the cap is on
    [SerializeField] private bool isOpen = true;
    
    private int receptacleLayerMask;
    private Transform emitterTransform;
    private ReceptacleBehaviour currentReceptacle;

    private void Start() 
    {
        emitterTransform = pourEmitter.transform;
        receptacleLayerMask = LayerMask.GetMask("Receptacle");
    }

    private void Update()
    {
        float objectAngle = Vector3.Angle(transform.up, Vector3.down);

        if (isPouring) 
        {
            if (objectAngle > pourThreshold)
            {
                isPouring = false;
                pourEmitter.SetActive(false);
                ResetCurrentReceptacle();
            }
            else
            {
                testForReceptacle();
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
            ResetCurrentReceptacle();
        }
    }

    private void testForReceptacle() 
    {
        RaycastHit hit;

        if (Physics.Raycast(emitterTransform.position, Vector3.down, out hit, 5.0f, receptacleLayerMask))
        {
            // we check whether the current hit object is a receptacle and whether it is not the same receptacle we are already filling
            ReceptacleBehaviour receptacle = hit.collider.GetComponent<ReceptacleBehaviour>();
            if (receptacle != null && currentReceptacle != receptacle)
            {
                // if we were filling a different receptacle, we now stop that
                ResetCurrentReceptacle();

                // and we start/continue filling the current receptacle
                receptacle.StartFill(pourLiquid);
                currentReceptacle = receptacle;
            }
        }
        else
        {
            // if no object is hit and we were filling a receptacle, we stop filling it
            ResetCurrentReceptacle();
        }
    }

    private void ResetCurrentReceptacle()
    {
        if (currentReceptacle != null)
        {
            currentReceptacle.StopFill();
            currentReceptacle = null;
        }
    }
}
