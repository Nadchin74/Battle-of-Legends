using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 150f; // Швидкість польоту вгору
    public float lifetime = 1f;    // Час життя тексту (1 секунда)

    void Start()
    {
        // Автоматично видаляємо текст через 1 секунду, щоб не засмічувати пам'ять
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Кожного кадру піднімаємо текст рівно вгору
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}