using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Platformer");
        Time.timeScale = 1;   // Jogo sai da pausa e fica ativo de novo
    }

}
