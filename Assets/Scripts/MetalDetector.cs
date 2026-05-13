using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class MetalDetector : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float moveSpeed = 3f;
    float baseSize;
    //private bool isGrounded = false;
    [Header("Object References")]
    private Rigidbody2D rb;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().invulnurable = false;
            Destroy(this);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x)
        {
            rb.linearVelocityX = -moveSpeed;
            transform.localScale = new Vector3(-1, 1, 1) * baseSize;
        } else
        {
            rb.linearVelocityX = moveSpeed;
            transform.localScale = Vector3.one * baseSize;
            
        }
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position - new Vector3(0, 0.1f, 0), transform.localScale, 0, Vector2.up);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Ground") && transform.InverseTransformPoint(hit.point).y < 0)
            {
                Jump();
            }
        }
    }
    void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForceY(Mathf.Sqrt(jumpHeight * -Physics.gravity.y * 2), ForceMode2D.Impulse);
    }
}
