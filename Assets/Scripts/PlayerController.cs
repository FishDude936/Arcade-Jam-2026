using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] int attackStrength = 5;
    public bool invulnurable = true;
    public bool isGrounded = false;
    [Header("Object References")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector3(0.5f, 1.1f, 1), 0, Vector2.up);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Ground") && transform.InverseTransformPoint(hit.point).y < 0)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
        // isGrounded = rb.totalForce.y == 0;
    }
    public void Move(Vector2 moveVector)
    {
        rb.linearVelocityX = moveVector.x * moveSpeed;
        // RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector3(1.1f, 0.5f, 1), 0, Vector2.up);
        // foreach (RaycastHit2D hit in hits)
        // {
        //     if (hit.transform.gameObject.CompareTag("Ground"))
        //     {
        //         if (rb.linearVelocityX > 0 && transform.InverseTransformPoint(hit.point).x > 0)
        //         {
        //             // going right and hit.point is to the right
        //             rb.linearVelocityX = 0;
        //         } else if (rb.linearVelocityX < 0 && transform.InverseTransformPoint(hit.point).x < 0)
        //         {
        //             rb.linearVelocityX = 0;
        //         }
        //     }
        // }
        if (rb.linearVelocityX > 0)
        {
            sprite.flipX = false;
        } else if (rb.linearVelocityX < 0)
        {
            sprite.flipX = true;
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocityY = 0;
            rb.AddForceY(Mathf.Sqrt(jumpHeight * -Physics.gravity.y * 2), ForceMode2D.Impulse);
        }
    }
    public void Attack()
    {
        
    }
}
