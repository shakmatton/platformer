using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public string nextLevelName;

    public void LoadNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
        Time.timeScale = 1;   // Jogo sai da pausa e fica ativo de novo
    }
}
