using UnityEngine;
using System;

public class HorizontalMoveAndRotate : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    public GameObject targetObject;
    
    [Header("Movement Settings")]
    public float targetDistanceX = 1f;    // Target distance to move along X-axis
    public float targetDistanceY = 1f;    // Target distance to move along Y-axis
    public float moveSpeed_Y = 2f;
    public float moveSpeed_X = 2f;          // Movement speed (same for both axes)
    private bool movementComplete = false;

    private float scalevalue = 0.6f;
    

    public float rotationvalue = 0f;

    private float targetDistanceZ = 0f;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;     // Degrees per second
    public float currentRotation = 0f;   // Track current rotation
    public float TARGET_ANGLE = -45f; // Target rotation angle
    private bool rotationComplete = false;

    public int rotationAxis = 0; // 0 = X-axis, 1 = Y-axis, 2 = Z-axis
    
    
    void Start()
    {
        rb = targetObject.GetComponent<Rigidbody>();
        initialPosition = targetObject.transform.position;
        
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX |    // Lock X rotation
                             RigidbodyConstraints.FreezeRotationY |    // Lock Y rotation
                             RigidbodyConstraints.FreezeRotationZ;     // Lock Z rotation
        }

        // float cos = MathF.Cos(rotationvalue);
        // float sin = MathF.Sin(rotationvalue);
        // targetDistanceX = cos * targetDistanceX;
        // targetDistanceZ = sin * targetDistanceX;

        targetDistanceX = scalevalue * targetDistanceX;
        targetDistanceY = scalevalue * targetDistanceY;
        targetDistanceZ = scalevalue * targetDistanceZ;
        moveSpeed_X = scalevalue * moveSpeed_X;
        moveSpeed_Y = scalevalue * moveSpeed_Y;


    }
    
    void FixedUpdate()
    {
        if (rb != null)
        {
            // Handle horizontal movement along X-axis
            float horizontalDistance = targetObject.transform.position.x - initialPosition.x;
            
            if (!movementComplete && Mathf.Abs(horizontalDistance) < targetDistanceX)
            {
                // Move in the X direction (horizontal)
                float moveStep = moveSpeed_X * Time.fixedDeltaTime;
                
                // If we would overshoot, move exactly to target
                if (Mathf.Abs(horizontalDistance) + moveStep >= targetDistanceX)
                {
                    float remainingDistance = targetDistanceX - Mathf.Abs(horizontalDistance);
                    rb.MovePosition(rb.position + new Vector3(remainingDistance, 0, 0));
                }
                else
                {
                    rb.MovePosition(rb.position + new Vector3(moveStep, 0, 0));
                }
            }

            float zDistance = targetObject.transform.position.z - initialPosition.z;

            if (!movementComplete && Mathf.Abs(zDistance) < targetDistanceZ)
            {
                // Move in the Z direction (horizontal)
                float moveStep = moveSpeed_X * Time.fixedDeltaTime;

                // If we would overshoot, move exactly to target
                if (Mathf.Abs(zDistance) + moveStep >= targetDistanceZ)
                {
                    float remainingDistance = targetDistanceZ - Mathf.Abs(zDistance);
                    rb.MovePosition(rb.position + new Vector3(0, 0, remainingDistance));
                }
                else
                {
                    rb.MovePosition(rb.position + new Vector3(0, 0, moveStep));
                }
            }

            // Handle vertical movement along Y-axis
            float verticalDistance = targetObject.transform.position.y - initialPosition.y;

            if (!movementComplete && Mathf.Abs(verticalDistance) < targetDistanceY)
            {
                // Move in the Y direction (vertical)
                float moveStep = moveSpeed_Y * Time.fixedDeltaTime;

                // If we would overshoot, move exactly to target
                if (Mathf.Abs(verticalDistance) + moveStep >= targetDistanceY)
                {
                    float remainingDistance = targetDistanceY - Mathf.Abs(verticalDistance);
                    rb.MovePosition(rb.position + new Vector3(0, remainingDistance, 0));
                }
                else
                {
                    rb.MovePosition(rb.position + new Vector3(0, moveStep, 0));
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
// Handle Z-axis rotation until the target angle is reached
            if (!rotationComplete)
            {
                float remainingRotation = TARGET_ANGLE - currentRotation;
                Debug.Log(remainingRotation);

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
                    float rotationThisFrame = Mathf.Sign(remainingRotation) * rotationSpeed * Time.fixedDeltaTime;

                    // Apply the rotation
                    if (rotationAxis == 1)  // Y-axis rotation
                        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotationThisFrame, 0));
                    else if (rotationAxis == 2) // Z-axis rotation
                        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, rotationThisFrame));
                    else // X-axis rotation
                    rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationThisFrame, 0, 0));

                    // Update currentRotation incrementally
                    currentRotation += rotationThisFrame;
                }
            }
        }
    }
}
