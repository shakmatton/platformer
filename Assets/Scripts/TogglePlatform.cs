using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public float onDuration = 3f;
    public float offDuration = 2f;

    public bool StartOn = true;

    private SpriteRenderer spriteRenderer;
    private Collider2D coll;

    private bool isOn;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        Toggle();
        isOn = StartOn;        
    }

    private void Toggle()
    {
        coll.enabled = isOn;
        spriteRenderer.sprite = isOn ? onSprite : offSprite;
        timer = isOn ? onDuration : offDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isOn = !isOn;
            Toggle();
        }
    }
}
