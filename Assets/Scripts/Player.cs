using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int coins;
    
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

    private AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip damageClip;

    public Image healthImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        velocidadeAtual = speed;
        jumps = extraJumps;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        audioSource = GetComponent<AudioSource>();
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
            PlaySFX(jumpClip);
        }
        else if (jumps > 0)
        {
            // Pulo extra no ar — consome um do contador
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // zera Y antes de aplicar a força
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumps--;
            PlaySFX(jumpClip);
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
        if (movimentoX != 0)
        {
            if (movimentoX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

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

        if (transform.position.y < -22)         // personagem morre se cair para além dessa distância vertical
        {
            Die();
        }   
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoX * velocidadeAtual, rb.linearVelocity.y);       
    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (transform.position.y > enemy.transform.position.y + 0.3f)               // se altura do jogador for maior que altura do inimigo + 0.3f, player está mais acima do inimigo.
            {
                Destroy(enemy.gameObject);                                              // inimigo é destruído ao estilo Mario (jogador pula em cima do inimigo e mata ele)
                return;                                                                 // sai do método sem processar os demais if's abaixo.
            }
        }

        if (col.gameObject.CompareTag("Chao"))
        {
            noChao = true;
            jumps = extraJumps; // reseta ao tocar o chão
        }
        
        if (col.gameObject.CompareTag("Damage"))
        {
            PlaySFX(damageClip);
            health -= 25;
            StartCoroutine(BlinkRed());
            healthImage.fillAmount = health / 100f;
            

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

    public void PlaySFX(AudioClip audioClip, float volume = 1f)      // public allows this method to be seen and be used by other classes
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;            // see class Coins (in Unity, drag your sound into coin prefab coin clip field)
        audioSource.Play();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpPowerUp")
        {
            extraJumps = 2;
            Destroy(collision.gameObject);
        }
    }    
}