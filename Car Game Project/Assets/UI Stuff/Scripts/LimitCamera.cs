using UnityEngine;

public class CameraLimit : MonoBehaviour
{

    public GameObject Player;

    private void LateUpdate()
    {
       transform.position = new Vector3(Player.transform.position.x, 65, Player.transform.position.z);
    }
}

