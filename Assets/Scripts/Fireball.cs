using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 2f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.lives--;
            if (GameManager.instance.lives == 0)
            {
                AudioManager.instance.PlaySound("GameOver");
                Destroy(collision.gameObject);
                GameManager.instance.StartReset();
            } else
            {
                AudioManager.instance.PlaySound("PlayerHit");
            }
        }
        if (!collision.GetComponent<FlyingEnemy>())
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(transform.localEulerAngles.z == 0 ? moveSpeed : -moveSpeed, 0);
    }
}
