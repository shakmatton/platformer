using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private float interval = 2f;
    private float timer;
    private SpriteRenderer[] sprites;
    private Collider2D col;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            foreach (SpriteRenderer s in sprites)
            {
                s.enabled = !s.enabled;
            }

            if (col != null)
            {
                col.enabled = !col.enabled;
            }
            
            timer = 0f;
        }
    }
}
