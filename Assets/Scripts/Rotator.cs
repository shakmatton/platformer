using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotateSpeed = 50f;
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
