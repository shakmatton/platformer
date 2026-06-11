using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall = 0.3f;   // tempo antes da queda (ajuste no Inspector)
    private bool isFalling = false;       // evita acionar a queda mais de uma vez    

    private void OnCollisionEnter2D(Collision2D collision)                  // se houver colisão com a plataforma...    
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)        // ... causada pelo player, e se a plataforma não estiver em queda livre...
        {
            isFalling = true;                   // indica que a plataforma agora está caindo
            Invoke("Fall", timeBeforeFall);     // chama Fall() após "timeBeforeFall" segundos
        }
    }

    private void Fall()
    {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();        // adiciona física e faz plataforma cair
        rb.freezeRotation = true;                                       // impede que haja rotação da plataforma durante a queda

        Destroy(transform.parent.gameObject, 3f);   // acessa e destrói o objeto pai após 3s ("Container" é o filho e "FallingPlatform" o é pai)
    }
}