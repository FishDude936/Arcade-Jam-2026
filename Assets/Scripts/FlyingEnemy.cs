using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlyingEnemy : MonoBehaviour
{
    // [SerializeField] float attentionRadius = 5f;
    // [SerializeField] float moveSpeed = 3f;
    [SerializeField] float knockbackStrength = 10f;
    [SerializeField] int scoreValue = 50;
    [SerializeField] int fireballRate = 4;
    [SerializeField] int fireballOffset = 0;
    [SerializeField] GameObject fireball;
    private float lastFireballTime = 0;
    private float baseSize;
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
        lastFireballTime = GameManager.instance.levelStartTime + fireballOffset;
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
        if (Mathf.FloorToInt(Time.time - lastFireballTime) >= fireballRate)
        {
            lastFireballTime = Time.time;
            GameObject fire = Instantiate(fireball);
            fire.transform.position = transform.Find("FireballStart").position;
            fire.transform.localEulerAngles = new Vector3(0, 0, player.transform.position.x > transform.position.x ? 0 : 180);
        }
        // if (Vector2.Distance(transform.position, player.transform.position) < attentionRadius && player.GetComponent<PlayerController>().canMove)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        // }
    }
}
