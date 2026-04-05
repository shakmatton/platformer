using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    public float amplitude = 0.3f; // altura da oscilação
    public float frequencia = 2f;   // velocidade da oscilação

    private float startY;
    private Transform t;

    void Start()
    {
        t = GetComponent<Transform>();
        startY = t.position.y; // salva a posição original
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * frequencia) * amplitude;     // "Sin" varia entre -1 e 1; frequencia = velocidade ; amplitude = altura.
        t.position = new Vector3(t.position.x, newY, t.position.z);
    }   
}