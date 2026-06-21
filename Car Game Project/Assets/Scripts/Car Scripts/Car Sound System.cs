using UnityEngine;
using UnityEngine.InputSystem;

public class CarSoundSystem : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource IdleAudio;
    public AudioSource DriveAudio;
    public AudioSource BrakeAudio;
    public AudioSource HornAudio;

    public float MinPitch = 0.8f;
    public float MaxPitch = 2.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("CarSoundSystem could not find a Rigidbody on this object or any parent objects!", this);
        }

        // Fix 2: Explicitly turn on engine loops at startup if they aren't playing yet
        if (IdleAudio != null && !IdleAudio.isPlaying) IdleAudio.Play();
        if (DriveAudio != null && !DriveAudio.isPlaying) DriveAudio.Play();
    }

    void Update()
    {
        UpdateEngineSound();
        HandleAudio();
    }

    void UpdateEngineSound()
    {
        if (rb == null || IdleAudio == null || DriveAudio == null) return;

        float speed = rb.linearVelocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / 40f);

        IdleAudio.pitch = Mathf.Lerp(MinPitch, 1.2f, normalizedSpeed);
        DriveAudio.pitch = Mathf.Lerp(MinPitch, MaxPitch, normalizedSpeed);

        DriveAudio.volume = normalizedSpeed;
        IdleAudio.volume = 1f - (normalizedSpeed * 0.5f);
    }

    void HandleAudio()
    {
        if (rb == null || Keyboard.current == null) return;

        float speed = rb.linearVelocity.magnitude;

        // Fix 3: Upgraded old legacy Input.GetKey over to the New Input System device poll
        bool braking = Keyboard.current.spaceKey.isPressed && speed > 2f;

        if (braking && BrakeAudio != null)
        {
            if (!BrakeAudio.isPlaying)
                BrakeAudio.Play();
        }
        else if (BrakeAudio != null)
        {
            if (BrakeAudio.isPlaying)
                BrakeAudio.Stop();
        }

        if (Keyboard.current.hKey.wasPressedThisFrame && HornAudio != null)
        {
            HornAudio.Play();
        }
    }
}