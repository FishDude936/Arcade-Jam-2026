using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1);
    [Header("Variables")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpHeight = 2f;
    // [SerializeField] int attackStrength = 5;
    public bool canMove = true;
    public bool invulnurable = true;
    public bool isGrounded = false;
    [Header("Object References")]
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint2D contact = collision.GetContact(i);
                if (contact.point.y < transform.position.y)
                {
                    isGrounded = true;
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // for (int i = 0; i < collision.contactCount; i++)
            // {
            //     ContactPoint2D contact = collision.GetContact(i);
            //     if (contact.point.y < transform.position.y)
            //     {
            //         isGrounded = true;
            //     }
            // }
            isGrounded = false;
        }
    }
    // void Update()
    // {
    //     RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, GetComponent<Collider2D>().bounds.size, 0, Vector2.up);
    //     foreach (RaycastHit2D hit in hits)
    //     {
    //         if (hit.transform.gameObject.CompareTag("Ground") && transform.InverseTransformPoint(hit.point).y < 0)
    //         {
    //             isGrounded = true;
    //             return;
    //         }
    //     }
    //     isGrounded = false;
    // }
    public void Move(Vector2 moveVector)
    {
        if (canMove)
        {
            rb.linearVelocityX = moveVector.x * moveSpeed;
        }
        if (rb.linearVelocityX > 0)
        {
            transform.localScale = Vector3.one;
        } else if (rb.linearVelocityX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void Jump()
    {
        if (isGrounded && canMove)
        {
            isGrounded = false;
            rb.linearVelocityY = 0;
            rb.AddForceY(Mathf.Sqrt(jumpHeight * -Physics.gravity.y * 2), ForceMode2D.Impulse);
        }
    }
    public void StartAttack()
    {
        if (!transform.Find("Weapon").gameObject.activeSelf)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        transform.Find("Weapon").gameObject.SetActive(true);
        yield return _waitForSeconds1;
        transform.Find("Weapon").gameObject.SetActive(false);
    }
    public void StartKnockback(Vector2 force)
    {
        if (canMove)
        {
            StartCoroutine(Knockback(force));
        }
    }
    IEnumerator Knockback(Vector2 force)
    {
        canMove = false;
        rb.linearVelocity = force;
        yield return _waitForSeconds1;
        canMove = true;
    }
}
