using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuButton : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
