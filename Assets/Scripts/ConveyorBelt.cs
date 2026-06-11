using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float beltSpeed = 3f;    // positivo = direita, negativo = esquerda

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            player.currentBeltSpeed = beltSpeed;    // avisa o player a velocidade da esteira
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            player.currentBeltSpeed = 0f;           // player saiu da esteira, zera o efeito
    }
}