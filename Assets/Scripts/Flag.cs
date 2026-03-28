using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject WinUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // se personagem tocar na bandeira...
        {
            Time.timeScale = 0;         // Jogo pausa
            WinUI.SetActive(true);      // Mostra tela de vitória
        }
    }
}
