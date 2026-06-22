using UnityEngine;
using UnityEngine.InputSystem;

public class CarSoundSystem : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource IdleAudio;
    public AudioSource DriveStartAudio;
    public AudioSource DriveEndAudio;
    public AudioSource BrakeAudio;
    public AudioSource HornAudio;

    [Header("Engine Pitch Settings")]
    public float MinPitch = 0.8f;
    public float MaxPitch = 2.5f;

    [Header("Brake Fade Smoothness")]
    public float brakeFadeSpeed = 5f;

    private Rigidbody rb;
    private float targetBrakeVolume = 0f;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("CarSoundSystem could not find a Rigidbody on this object or any parent objects!", this);
        }

        if (IdleAudio != null && !IdleAudio.isPlaying) IdleAudio.Play();

        if (DriveStartAudio != null)
        {
            DriveStartAudio.volume = 0f;
            if (!DriveStartAudio.isPlaying) DriveStartAudio.Play();
        }

        if (DriveEndAudio != null)
        {
            DriveEndAudio.volume = 0f;
            if (!DriveEndAudio.isPlaying) DriveEndAudio.Play();
        }

        if (BrakeAudio != null)
        {
            BrakeAudio.volume = 0f;
            BrakeAudio.loop = true;
            BrakeAudio.Play();
        }
    }

    void Update()
    {
        UpdateEngineSound();
        HandleAudio();
    }

    void UpdateEngineSound()
    {
        if (rb == null || IdleAudio == null || DriveStartAudio == null || DriveEndAudio == null) return;

        float speed = rb.linearVelocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / 40f);

        IdleAudio.pitch = Mathf.Lerp(MinPitch, 1.2f, normalizedSpeed);
        DriveStartAudio.pitch = Mathf.Lerp(MinPitch, MaxPitch, normalizedSpeed);
        DriveEndAudio.pitch = Mathf.Lerp(MinPitch, MaxPitch, normalizedSpeed);

        bool isAccelerating = Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;

        if (speed > 0.1f)
        {
            IdleAudio.volume = Mathf.MoveTowards(IdleAudio.volume, 0f, 5f * Time.deltaTime);

            if (isAccelerating)
            {
                DriveStartAudio.volume = Mathf.MoveTowards(DriveStartAudio.volume, normalizedSpeed * 0.67f, 5f * Time.deltaTime);
                DriveEndAudio.volume = Mathf.MoveTowards(DriveEndAudio.volume, 0f, 5f * Time.deltaTime);
            }
            else
            {
                DriveEndAudio.volume = Mathf.MoveTowards(DriveEndAudio.volume, normalizedSpeed * 0.67f, 5f * Time.deltaTime);
                DriveStartAudio.volume = Mathf.MoveTowards(DriveStartAudio.volume, 0f, 5f * Time.deltaTime);
            }
        }
        else
        {
            IdleAudio.volume = Mathf.MoveTowards(IdleAudio.volume, 1f, 5f * Time.deltaTime);
            DriveStartAudio.volume = Mathf.MoveTowards(DriveStartAudio.volume, 0f, 5f * Time.deltaTime);
            DriveEndAudio.volume = Mathf.MoveTowards(DriveEndAudio.volume, 0f, 5f * Time.deltaTime);
        }
    }

    void HandleAudio()
    {
        if (rb == null || Keyboard.current == null) return;
        float speed = rb.linearVelocity.magnitude;
        bool isPressingBrake = Keyboard.current.spaceKey.isPressed && speed > 0.5f;

        if (isPressingBrake && BrakeAudio != null)
        {
            targetBrakeVolume = 1f;
        }
        else
        {
            targetBrakeVolume = 0f;
        }
        if (BrakeAudio != null)
        {
            BrakeAudio.volume = Mathf.MoveTowards(BrakeAudio.volume, targetBrakeVolume, brakeFadeSpeed * Time.deltaTime);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame && HornAudio != null)
        {
            HornAudio.Play();
        }
    }
}