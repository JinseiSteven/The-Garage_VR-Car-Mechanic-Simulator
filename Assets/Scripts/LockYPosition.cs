using UnityEngine;

public class LockYPosition : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -0.616f) {
            transform.position = new Vector3(transform.position.x, -0.616f, transform.position.z);
        }
        if (transform.position.y > -0.616f) {
            transform.position = new Vector3(transform.position.x, -0.616f, transform.position.z);
        }
    }
}
