using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{ 
        public Rigidbody target;

        public float maxSpeed = 0.0f; // The maximum speed of the target ** IN KM/H **

        public float minSpeedArrowAngle;
        public float maxSpeedArrowAngle;

    [Header("UI")]
        public TMP_Text speedLabel; // The label that displays the speed;
        public RectTransform arrow; // The arrow in the speedometer

    private float speed = 0.0f;
    private float smoothedSpeed = 0.0f;
    public float smoothTime = 0.15f; // adjust this — higher = more delay

    private void Update()
    {
        speed = target.linearVelocity.magnitude * 3.6f;

        smoothedSpeed = Mathf.Lerp(smoothedSpeed, speed, Time.deltaTime / smoothTime);

        if (speedLabel != null)
            speedLabel.text = ((int)smoothedSpeed).ToString();
        if (arrow != null)
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, smoothedSpeed / maxSpeed));
    }

}
