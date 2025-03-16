using UnityEngine;

public class BasketBallBehaviour : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 1.0f;
    [SerializeField] private float minDistance = 6.0f;
    [SerializeField] private float forceThreshold = 0.1f;

    [Header("Pitch Randomization")]
    [SerializeField] private float minPitch = 0.7f;
    [SerializeField] private float maxPitch = 1.2f;

    private void OnCollisionEnter(Collision collision)
    {

        float collisionForce = collision.relativeVelocity.magnitude;

        // if the bounce is super weak or slow, we dont want to play a sound
        if (collisionForce < forceThreshold)
        {
            return;
        }

        float volume = Mathf.Clamp(collisionForce / 10f, minVolume, maxVolume);
        float pitch = Random.Range(minPitch, maxPitch);

        AudioManager.Play("bounce", transform.position, pitch: pitch, volume: volume, minDistance: minDistance, isNarration:false);
    }
}
