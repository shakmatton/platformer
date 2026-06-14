using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*public class SpeedPad : MonoBehaviour
{
    public float upForce;
    public float forwardForce;

    private Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();

            rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * forwardForce, ForceMode2D.Impulse);
        }
    }
}*/

public class SpeedPad : MonoBehaviour
{
    public float upForce;
    public float forwardForce;

    private Vector2 vectorXY;

    private Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            Rigidbody2D rb = collision.transform.GetComponent<Rigidbody2D>();

            rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);

            player.speedPadForce = forwardForce;
            player.speedBoost = true;
            Invoke("DisableSpeedBoost", 0.3f);
        }
    }

    private void DisableSpeedBoost()
    {
        player.speedBoost = false;
    }
}