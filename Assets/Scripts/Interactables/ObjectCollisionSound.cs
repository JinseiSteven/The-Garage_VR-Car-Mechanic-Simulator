using UnityEngine;

public class ObjectCollisionSound : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private string soundName = string.Empty;
    [SerializeField] private float minDistance = 4.0f;
    [SerializeField] private float forceThreshold = 0.5f;

    [Header("Pitch Randomization")]
    [SerializeField] private float minPitch = 0.7f;
    [SerializeField] private float maxPitch = 1.2f;

    private void OnCollisionEnter(Collision collision)
    {

        float collisionForce = collision.relativeVelocity.magnitude;


        if (collisionForce < forceThreshold)
        {
            return;
        }

        float pitch = Random.Range(minPitch, maxPitch);

        AudioManager.Play(soundName, transform.position, pitch: pitch, minDistance: minDistance, isNarration:false);
    }
}
