using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer2D : MonoBehaviour
{
    private Rigidbody2D rb;

    private float movimentoX;
    private float velocidadeAtual;

    public float speed = 5f;
    public float jumpForce = 10f;

    public bool noChao = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocidadeAtual = speed;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movimentoX = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && noChao)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            velocidadeAtual = speed * 2f;
        else if (ctx.canceled)
            velocidadeAtual = speed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoX * velocidadeAtual, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Chao"))
            noChao = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Chao"))
            noChao = false;
    }
}