using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class MovePlayer2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private float movimentoX;
    private float velocidadeAtual;

    public float speed = 5f;
    public float jumpForce = 10f;

    private bool noChao = false;

    public int extraJumps = 1;      // configurável no Inspector
    private int jumps;              // contador atual de pulos extras restantes
    
    public int health = 100;
    private SpriteRenderer spriteRenderer;

    public Image HealthImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        velocidadeAtual = speed;
        jumps = extraJumps;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movimentoX = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (noChao)
        {
            // Pulo normal do chão — reseta o contador de extras
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumps = extraJumps;
        }
        else if (jumps > 0)
        {
            // Pulo extra no ar — consome um do contador
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // zera Y antes de aplicar a força
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumps--;
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            velocidadeAtual = speed * 2f;
        else if (ctx.canceled)
            velocidadeAtual = speed;
    }

    private void Update()
    {            
        AtualizarAnimacao();        
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoX * velocidadeAtual, rb.linearVelocity.y);
    }

    private void AtualizarAnimacao()
    {
        if (noChao)
        {
            if (movimentoX == 0)
                animator.Play("Player_Idle");
            else if (velocidadeAtual > speed)
                animator.Play("Player_Run");
            else
                animator.Play("Player_Walk");
        }
        else if (rb.linearVelocity.y > 0)
        {
            animator.Play("Player_Jump");
        }
        else
        {
            animator.Play("Player_Fall");
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Chao"))
        {
            noChao = true;
            jumps = extraJumps; // reseta ao tocar o chão
        }
        
        if (col.gameObject.CompareTag("Damage"))
        {
            health -= 25;
            StartCoroutine(BlinkRed());
            HealthImage.fillAmount = health / 100f;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Chao"))
            noChao = false;
    }
    
    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Platformer");
    }
    
}