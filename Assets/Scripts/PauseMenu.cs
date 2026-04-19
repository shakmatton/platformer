using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;            // container do Pause Menu
    private bool isPausing = false;

    /* Observação: EventSystem.current.SetSelectedGameObject(null); usado no ResumeButton() e no bloco de despause do OnPause(). 

    Problema: tecla Enter ficava travada após sair da tela de pausa.

    Explicação: enquanto a tela de pausa está ativa, o EventSystem do Unity foca automaticamente um dos botões da UI. Ao desativar o container, os botões somem, mas o EventSystem
    mantém uma referência interna a esse botão fantasma. Com isso, qualquer tecla configurada como Submit na UI (incluindo Enter) era consumida pelo EventSystem antes de chegar ao OnPause,
    impedindo reabrir a pausa. 
    
    Solução: limpar o objeto selecionado aqui garante que o EventSystem libere o foco ao sair da tela de pausa.    */

    public void OnPause(InputAction.CallbackContext ctx)            // evento que sinaliza flag de pausa quando Enter é pressionado 
    {
        // Só age no momento exato do clique (performed), ignora held/canceled
        if (!ctx.performed) return;

        isPausing = !isPausing; // toggle

        container.SetActive(isPausing);
        Time.timeScale = isPausing ? 0 : 1;         // jogo é "congelado" ou "descongelado" a depender se o jogo foi pausado ou despausado

        if (!isPausing)
            EventSystem.current.SetSelectedGameObject(null); // limpa foco ao despausar por Enter
    
}

    public void ResumeButton()               // sai da pausa e retorna ao jogo
    {
        isPausing = false;                  
        container.SetActive(false);          // desativa tela de pausa
        Time.timeScale = 1;                  // "descongela" tempo

        EventSystem.current.SetSelectedGameObject(null); // limpa foco ao despausar por botão
    
}

    public void MainMenuButton()             // método do menu principal que carrega cena
    {
        Time.timeScale = 1;                  // importante resetar antes de trocar de cena!
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");     // conduz ao menu inicial do jogo
    }
}