using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public void StartGame()
    {
        SceneManager.LoadScene("Platformer");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // para o Play Mode no Editor
        #else
        Application.Quit(); // fecha a aplicação na build final
        #endif

        /* No jogo, o botão de Quit, cujo comportamento é o de Application.Quit(), simplesmente não funciona dentro do Editor do Unity. 
           Ele só tem efeito em uma build real do jogo (Windows, Mac, Linux etc). 
           No Editor, a chamada é ignorada silenciosamente, sem erro nenhum, o que dá a impressão de que o botão está quebrado.

           Então, para testar o botão ainda no Editor, o workaround padrão é usar UnityEditor.EditorApplication.isPlaying = false, que interrompe o Play Mode.
           Como essa classe só existe no Editor e não na build final, é preciso cercá-la com uma diretiva de compilação condicional:

           O #if UNITY_EDITOR garante que o código do Editor seja compilado apenas quando se está rodando dentro do Unity,
           e o #else garante que na build o Application.Quit() funcione normalmente. Quando você gerar a build e testar, o botão vai fechar o jogo como esperado.  */
    }
}
