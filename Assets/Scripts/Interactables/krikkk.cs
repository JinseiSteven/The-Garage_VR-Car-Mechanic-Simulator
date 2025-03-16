// using UnityEngine;
// using System;

// public class jup : MonoBehaviour
// {
//     private Vector3 initialPosition;
//     public GameObject targetObject;

//     [Header("Movement Settings")]
//     public float targetDistanceX = 1f;    // Target distance to move along X-axis
//     public float targetDistanceY = 1f;    // Target distance to move along Y-axis

//     public float targetDistanceZ = 1f;    // Target distance to move along Z-axis
//     public float moveSpeed_Y = 2f;
//     public float moveSpeed_X = 2f;        // Movement speed (same for both axes)

//     public float moveSpeed_Z = 2f;        // Movement speed (same for both axes)
//     private bool movementComplete = false;

//     private float scalevalue = 0.6f;

//     public float rotationvalue = 0f;

//     [Header("Rotation Settings")]
//     public float rotationSpeed = 90f;     // Degrees per second
//     public float currentRotation = 0f;   // Track current rotation
//     public float TARGET_ANGLE = -45f;    // Target rotation angle
//     private bool rotationComplete = false;

//     public int rotationAxis = 0;          // 0 = X-axis, 1 = Y-axis, 2 = Z-axis

//     void Start()
//     {
//         initialPosition = targetObject.transform.localPosition; // Use local position to track relative distance
        
//         // Scale adjustments to distances
//         targetDistanceX = scalevalue * targetDistanceX;
//         targetDistanceY = scalevalue * targetDistanceY;
//         targetDistanceZ = scalevalue * targetDistanceZ;
//         moveSpeed_X = scalevalue * moveSpeed_X;
//         moveSpeed_Y = scalevalue * moveSpeed_Y;
//     }

//     void Update()
//     {
//         // Handle horizontal movement along X-axis (local space)
//         float horizontalDistance = targetObject.transform.localPosition.x - initialPosition.x;

//         if (!movementComplete && Mathf.Abs(horizontalDistance) < targetDistanceX)
//         {
//             float moveStep = moveSpeed_X * Time.deltaTime;

//             if (Mathf.Abs(horizontalDistance) + moveStep >= targetDistanceX)
//             {
//                 float remainingDistance = targetDistanceX - Mathf.Abs(horizontalDistance);
//                 targetObject.transform.Translate(new Vector3(remainingDistance, 0, 0), Space.Self); // Local space movement
//             }
//             else
//             {
//                 targetObject.transform.Translate(new Vector3(moveStep, 0, 0), Space.Self);
//             }
//         }

//         // Handle Z-axis movement (local space)
//         float zDistance = targetObject.transform.localPosition.z - initialPosition.z;

//         if (!movementComplete && Mathf.Abs(zDistance) < targetDistanceZ)
//         {
//             float moveStep = moveSpeed_Z * Time.deltaTime;

//             if (Mathf.Abs(zDistance) + moveStep >= targetDistanceZ)
//             {
//                 float remainingDistance = targetDistanceZ - Mathf.Abs(zDistance);
//                 targetObject.transform.Translate(new Vector3(0, 0, remainingDistance), Space.Self);
//             }
//             else
//             {
//                 targetObject.transform.Translate(new Vector3(0, 0, moveStep), Space.Self);
//             }
//         }

//         // Handle vertical movement along Y-axis (local space)
//         float verticalDistance = targetObject.transform.localPosition.y - initialPosition.y;

//         if (!movementComplete && Mathf.Abs(verticalDistance) < targetDistanceY)
//         {
//             float moveStep = moveSpeed_Y * Time.deltaTime;

//             if (Mathf.Abs(verticalDistance) + moveStep >= targetDistanceY)
//             {
//                 float remainingDistance = targetDistanceY - Mathf.Abs(verticalDistance);
//                 targetObject.transform.Translate(new Vector3(0, remainingDistance, 0), Space.Self);
//             }
//             else
//             {
//                 targetObject.transform.Translate(new Vector3(0, moveStep, 0), Space.Self);
//             }
//         }

//         // Once both horizontal and vertical movements are complete, stop further movement
//         if (Mathf.Abs(horizontalDistance) >= targetDistanceX && Mathf.Abs(verticalDistance) >= targetDistanceY)
//         {
//             movementComplete = true;
//         }
//         else
//         {
//             movementComplete = false;
//         }

//         // Handle rotation towards the target angle
//         if (!rotationComplete)
//         {
//             float remainingRotation = TARGET_ANGLE - currentRotation;

//             // Check if we're very close to the target angle and complete the rotation
//             if (Mathf.Abs(remainingRotation) < 0.1f)
//             {
//                 rotationComplete = true;
//                 currentRotation = TARGET_ANGLE; // Set the final target rotation
//             }
//             else
//             {
//                 float rotationThisFrame = Mathf.Sign(remainingRotation) * rotationSpeed * Time.deltaTime;

//                 // Apply rotation in local space
//                 if (rotationAxis == 1)  // Y-axis rotation
//                     targetObject.transform.Rotate(0, rotationThisFrame, 0, Space.Self);
//                 else if (rotationAxis == 2) // Z-axis rotation
//                     targetObject.transform.Rotate(0, 0, rotationThisFrame, Space.Self);
//                 else // X-axis rotation
//                     targetObject.transform.Rotate(rotationThisFrame, 0, 0, Space.Self);

//                 currentRotation += rotationThisFrame;
//             }
//         }
//     }
// }

using UnityEngine;
using System;

public class krikk : MonoBehaviour
{
    private Vector3 initialPosition;
    public GameObject targetObject;

    [Header("Movement Settings")]
    public float targetDistanceX = 1f;    // Target distance to move along X-axis
    public float targetDistanceY = 1f;    // Target distance to move along Y-axis
    public float moveSpeed_Y = 2f;
    public float moveSpeed_X = 2f;  
    
    public float moveSpeed_Z = 2f;      // Movement speed (same for both axes)
    private bool movementComplete = false;

    private float scalevalue = 0.6f;

    public float rotationvalue = 0f;

    public float targetDistanceZ = 0f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;     // Degrees per second
    public float currentRotation = 0f;   // Track current rotation
    public float TARGET_ANGLE = -45f;    // Target rotation angle
    private bool rotationComplete = false;

    public int rotationAxis = 0;          // 0 = X-axis, 1 = Y-axis, 2 = Z-axis

    void Start()
    {
        initialPosition = targetObject.transform.localPosition; // Use local position to track relative distance

        // Scale adjustments to distances
        targetDistanceX = scalevalue * targetDistanceX;
        targetDistanceY = scalevalue * targetDistanceY;
        targetDistanceZ = scalevalue * targetDistanceZ;
        moveSpeed_X = scalevalue * moveSpeed_X;
        moveSpeed_Y = scalevalue * moveSpeed_Y;
    }

    void Update()
    {
        // Handle horizontal movement along X-axis (local space)
        float horizontalDistance = targetObject.transform.localPosition.x - initialPosition.x;

        if (Mathf.Abs(horizontalDistance) < targetDistanceX)
        {
            float moveStep = moveSpeed_X * Time.deltaTime;

            if (Mathf.Abs(horizontalDistance + moveStep) >= targetDistanceX)
            {
                float remainingDistance = targetDistanceX - Mathf.Abs(horizontalDistance);
                targetObject.transform.Translate(new Vector3(remainingDistance, 0, 0), Space.Self); // Local space movement
            }
            else
            {
                targetObject.transform.Translate(new Vector3(moveStep, 0, 0), Space.Self);
            }
        }

        // Handle Z-axis movement (local space)
        float zDistance = targetObject.transform.localPosition.z - initialPosition.z;

        if (Mathf.Abs(zDistance) < targetDistanceZ)
        {
            float moveStep = moveSpeed_Z * Time.deltaTime;

            if (Mathf.Abs(zDistance + moveStep) >= targetDistanceZ)
            {
                float remainingDistance = targetDistanceZ - Mathf.Abs(zDistance);
                targetObject.transform.Translate(new Vector3(0, 0, remainingDistance), Space.Self);
            }
            else
            {
                targetObject.transform.Translate(new Vector3(0, 0, moveStep), Space.Self);
            }
        }

        // Handle vertical movement along Y-axis (local space)
        float verticalDistance = targetObject.transform.localPosition.y - initialPosition.y;

        if (!movementComplete && Mathf.Abs(verticalDistance) < targetDistanceY)
        {
            float moveStep = moveSpeed_Y * Time.deltaTime;

            if (Mathf.Abs(verticalDistance) + moveStep >= targetDistanceY)
            {
                float remainingDistance = targetDistanceY - Mathf.Abs(verticalDistance);
                targetObject.transform.Translate(new Vector3(0, remainingDistance, 0), Space.Self);
            }
            else
            {
                targetObject.transform.Translate(new Vector3(0, moveStep, 0), Space.Self);
            }
        }

        // Movement completion check
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
                currentRotation = TARGET_ANGLE; // Set the final target rotation
            }
            else
            {
                float rotationThisFrame = Mathf.Sign(remainingRotation) * rotationSpeed * Time.deltaTime;

                // Apply rotation in local space
                if (rotationAxis == 1)  // Y-axis rotation
                    targetObject.transform.Rotate(0, rotationThisFrame, 0, Space.Self);
                else if (rotationAxis == 2) // Z-axis rotation
                    targetObject.transform.Rotate(0, 0, rotationThisFrame, Space.Self);
                else // X-axis rotation
                    targetObject.transform.Rotate(rotationThisFrame, 0, 0, Space.Self);

                currentRotation += rotationThisFrame;
            }
        }
    }
}

