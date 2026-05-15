using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] float attentionRadius = 5f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float knockbackStrength = 10f;
    [SerializeField] int scoreValue = 50;
    float baseSize;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.lives--;
            if (GameManager.instance.lives == 0)
            {
                Destroy(collision.gameObject);
                GameManager.instance.StartReset();
            } else
            {
                AudioManager.instance.PlaySound("PlayerHit");
                collision.GetComponent<PlayerController>().StartKnockback((transform.position - collision.transform.position).normalized * -knockbackStrength);
            }
        }
        if (collision.gameObject.CompareTag("Weapon"))
        {
            AudioManager.instance.PlaySound("DragonKilled");
            GameManager.instance.tempScore += scoreValue;
            Destroy(gameObject);
        }
    }
    void Start()
    {
        baseSize = transform.localScale.x;
    }
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Destroy(this);
            return;
        }
        transform.localScale = (player.transform.position.x > transform.position.x) ? Vector3.one * baseSize : new Vector3(-1, 1, 1) * baseSize;
        // if (Vector2.Distance(transform.position, player.transform.position) < attentionRadius && player.GetComponent<PlayerController>().canMove)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        // }
    }
}
