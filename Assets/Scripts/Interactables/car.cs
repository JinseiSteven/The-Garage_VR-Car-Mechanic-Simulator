using UnityEngine;
using System;
using System.Collections;

public class cars : MonoBehaviour
{
    private Vector3 initialPosition;
    public GameObject car;  // Renaming 'targetObject' to 'car'
    public GameObject car2;

    [Header("Movement Settings")]
    public float targetDistanceX = 1f;    // Target distance to move along X-axis
    public float targetDistanceY = 1f;    // Target distance to move along Y-axis
    public float moveSpeed_Y = 2f;
    public float moveSpeed_X = 2f;          // Movement speed (same for both axes)
    private bool movementComplete = false;

    public bool replace = false;

    private float scalevalue = 0.6f;

    public float rotationvalue = 0f;

    private float targetDistanceZ = 0f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;     // Degrees per second
    public float currentRotation = 0f;   // Track current rotation
    public float TARGET_ANGLE = -45f; // Target rotation angle
    private bool rotationComplete = false;

    public int rotationAxis = 0; // 0 = X-axis, 1 = Y-axis, 2 = Z-axis

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    void Start()
    { 
        StartCoroutine(ExampleCoroutine());
        initialPosition = car.transform.position;

        // Scaling values
        targetDistanceX = scalevalue * targetDistanceX;
        targetDistanceY = scalevalue * targetDistanceY;
        targetDistanceZ = scalevalue * targetDistanceZ;
        moveSpeed_X = scalevalue * moveSpeed_X;
        moveSpeed_Y = scalevalue * moveSpeed_Y;
    }

    void Update()
    {
        // Handle horizontal movement along X-axis
        float horizontalDistance = car.transform.position.x - initialPosition.x;

        if (!movementComplete && Mathf.Abs(horizontalDistance) < targetDistanceX)
        {
            // Move in the X direction (horizontal)
            float moveStep = moveSpeed_X * Time.deltaTime;

            // If we would overshoot, move exactly to target
            if (Mathf.Abs(horizontalDistance) + moveStep >= targetDistanceX)
            {
                float remainingDistance = targetDistanceX - Mathf.Abs(horizontalDistance);
                car.transform.Translate(new Vector3(remainingDistance, 0, 0));
            }
            else
            {
                car.transform.Translate(new Vector3(moveStep, 0, 0));
            }
        }

        float zDistance = car.transform.position.z - initialPosition.z;

        if (!movementComplete && Mathf.Abs(zDistance) < targetDistanceZ)
        {
            // Move in the Z direction (horizontal)
            float moveStep = moveSpeed_X * Time.deltaTime;

            // If we would overshoot, move exactly to target
            if (Mathf.Abs(zDistance) + moveStep >= targetDistanceZ)
            {
                float remainingDistance = targetDistanceZ - Mathf.Abs(zDistance);
                car.transform.Translate(new Vector3(0, 0, remainingDistance));
            }
            else
            {
                car.transform.Translate(new Vector3(0, 0, moveStep));
            }
        }

        // Handle vertical movement along Y-axis
        float verticalDistance = car.transform.position.y - initialPosition.y;

        if (!movementComplete && Mathf.Abs(verticalDistance) < targetDistanceY)
        {
            // Move in the Y direction (vertical)
            float moveStep = moveSpeed_Y * Time.deltaTime;

            // If we would overshoot, move exactly to target
            if (Mathf.Abs(verticalDistance) + moveStep >= targetDistanceY)
            {
                float remainingDistance = targetDistanceY - Mathf.Abs(verticalDistance);
                car.transform.Translate(new Vector3(0, remainingDistance, 0));
            }
            else
            {
                car.transform.Translate(new Vector3(0, moveStep, 0));
            }
        }

        // Once both horizontal and vertical movements are complete, stop further movement
        if (Mathf.Abs(horizontalDistance) >= targetDistanceX && Mathf.Abs(verticalDistance) >= targetDistanceY)
        {
            movementComplete = true;
        }
        else
        {
            movementComplete = false;
        }

        // Handle rotation towards the target angle
        if (!rotationComplete)
        {
            float remainingRotation = TARGET_ANGLE - currentRotation;

            // Check if we're very close to the target angle and complete the rotation
            if (Mathf.Abs(remainingRotation) < 0.1f)
            {
                rotationComplete = true;
                // To avoid snapping unexpectedly, set the currentRotation directly to TARGET_ANGLE
                currentRotation = TARGET_ANGLE;
            }
            else
            {
                // Determine the amount to rotate this frame
                float rotationThisFrame = Mathf.Sign(remainingRotation) * rotationSpeed * Time.deltaTime;

                // Apply the rotation
                if (rotationAxis == 1)  // Y-axis rotation
                    car.transform.Rotate(0, rotationThisFrame, 0);
                else if (rotationAxis == 2) // Z-axis rotation
                    car.transform.Rotate(0, 0, rotationThisFrame);
                else // X-axis rotation
                    car.transform.Rotate(rotationThisFrame, 0, 0);

                // Update currentRotation incrementally
                currentRotation += rotationThisFrame;
            }
        }

        if (movementComplete == true && rotationComplete == true && replace == true)
        {
            car.SetActive(false);
            car2.SetActive(true);
            GameStateManager.NotifyTaskActivated("Lift car");
        }
    }
}
