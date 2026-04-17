using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{    
    public Transform groundCheck;
    // Since groundCheck is not a sprite, lighting, effect, UI toolkit etc,
    // it is just going to be transform.

    public float groundCheckRadius = 0.2f;
    // This checks how big the cirle that checks if you are touching the floor is.

    public LayerMask groundLayer;
    // A layer which is ground and public so you can layer ground objects "ground".    

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

    [Header("Shooting")]                    // configuração dos tiros dados pelo personagem (ver também função HandleShooting mais ao final do script)
    public GameObject bulletPrefab;         // a bala em si
    // public Vector3 firePoint;               // o local de onde o tiro sai
    public float fireRate;                  // a taxa de repetição de tiros
    private float fireTimer;                // o tempo de duração do tiro
    private bool isFiring = false;          // ação de tiro desativada por padrão

    [Header("Wall Sliding")]
    public float checkDistance = 0.3f;  // 0.45f
    public float wallSlideSpeed = 2f;
    private bool isTouchingWall;
    private bool isWallSliding;

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

    public void OnFire(InputAction.CallbackContext ctx)         // gerencia ação de tiro por botão pressionado (configurado em Input Actions)
    {
        if (ctx.performed)
            isFiring = true;
        else if (ctx.canceled)
            isFiring = false;
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
        else
        {
            if (isWallSliding)                              // animação do player escorregando pela parede (estilo MegaMan X).
            {
                animator.Play("Player_WallSlide");
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

            if (rb.linearVelocityY < 0)             // controla velocidade de queda do personagem mais realista
            {
                rb.gravityScale = 3f;
            }
            else
            {
                rb.gravityScale = 2f;
            }

            HandleWallSlide(movimentoX);             // método que gerencia o escorregar do personagem pela parede (estilo MegaMan X)
        }
        
        HandleShooting();                       // método de tiro chamado dentro de Update.
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoX * velocidadeAtual, rb.linearVelocity.y);
        isTouchingWall = Physics2D.Raycast(transform.position, spriteRenderer.flipX ? Vector2.left : Vector2.right, checkDistance, groundLayer);
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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

    private void HandleShooting()                           // configura o método de atirar do personagem
    {
        fireTimer -= Time.deltaTime;

        if (isFiring && fireTimer <= 0f)                    // permite novo tiro com o botão do mouse e somente depois do tempo de vida da última bala esgotar
        {
            Shoot();                                        // chama o método de tiro 
            fireTimer = fireRate;                           // timer da bala atualizado por meio do tempo da taxa de tiro
        }
    }

    private void Shoot()                                    // método de tiro: configura o objeto bala, com seu prefab, posição e rotação
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position /* + firePoint */, Quaternion.identity);       // gera o objeto bala com essas configurações

        Bullet bulletScript = bullet.GetComponent<Bullet>();            //  bulletScript referencia o objeto bullet (bala)

        if (spriteRenderer.flipX)                                       // configura direção do tiro, a partir da direção do personagem
        {
            bulletScript.setDirection(Vector2.left);
        }
        else
        {
            bulletScript.setDirection(Vector2.right);
        }
    }

    private void HandleWallSlide(float movementoX)                                           // método que gerencia o escorregar do personagem pela parede (estilo MegaMan X)
    {
        if (isTouchingWall && !noChao && movementoX != 0 && rb.linearVelocityY < 0)
        {
            isWallSliding = true;
            rb.linearVelocityY = -wallSlideSpeed;
        }
        else
        {
            isWallSliding = false;
        }
    }
}