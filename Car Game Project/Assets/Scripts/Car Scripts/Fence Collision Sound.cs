using UnityEngine;

public class FenceCollisionSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource collisionAudioSource;
    public float minCollisionForce = 2f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null && collision.relativeVelocity.magnitude > minCollisionForce)
        {
            if (collisionAudioSource != null && !collisionAudioSource.isPlaying)
            {
                // Adjust volume slightly based on how hard the hit was
                collisionAudioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 15f);
                collisionAudioSource.Play();
            }
        }
    }
}