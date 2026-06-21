using UnityEngine;

public class CarSoundSystem : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource IdleAudio;
    public AudioSource DriveAudio;
    public AudioSource BrakeAudio;
    public AudioSource HornAudio;

    public float MinPitch = 0.8f;
    public float MaxPitch = 2.0f;

    // FIX: Added the Rigidbody variable so the script knows what 'rb' is
    private Rigidbody rb;

    void Start()
    {
        // FIX: Grab the Rigidbody component when the game starts
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("CarSoundSystem missing Rigidbody on this GameObject!", this);
        }
    }

    void Update()
    {
        // Note: GetInput() and ApplyBrakes() were called here but missing from your script. 
        // If they are in another script, you don't need them here.

        UpdateEngineSound();
        HandleAudio();
    }

    void UpdateEngineSound()
    {
        if (rb == null) return;

        float speed = rb.linearVelocity.magnitude;

        float normalizedSpeed = Mathf.Clamp01(speed / 40f);

        IdleAudio.pitch = Mathf.Lerp(MinPitch, 1.2f, normalizedSpeed);
        DriveAudio.pitch = Mathf.Lerp(MinPitch, MaxPitch, normalizedSpeed);

        DriveAudio.volume = normalizedSpeed;
        IdleAudio.volume = 1f - (normalizedSpeed * 0.5f);
    }

    void HandleAudio()
    {
        if (rb == null) return;

        float speed = rb.linearVelocity.magnitude;

        // Brake sound ONLY when moving
        bool braking = Input.GetKey(KeyCode.Space) && speed > 2f;

        if (braking)
        {
            if (!BrakeAudio.isPlaying)
                BrakeAudio.Play();
        }
        else
        {
            if (BrakeAudio.isPlaying)
                BrakeAudio.Stop();
        }

        // Horn
        if (Input.GetKeyDown(KeyCode.H))
        {
            HornAudio.Play();
        }
    }
}