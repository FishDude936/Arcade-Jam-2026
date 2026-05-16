using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    private static readonly int IsAttackingHash = Animator.StringToHash("isAttacking");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly WaitForSeconds _waitForSeconds1 = new(1);
    [Header("Variables")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpHeight = 2f;
    // [SerializeField] int attackStrength = 5;
    public bool canMove = true;
    public bool canAttack = true;
    public bool isGrounded = false;
    [SerializeField] LayerMask groundLayer;
    private float scale;
    [Header("Object References")]
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D cldr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cldr = GetComponent<Collider2D>();
        scale = transform.localScale.x;
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         // for (int i = 0; i < collision.contactCount; i++)
    //         // {
    //         //     ContactPoint2D contact = collision.GetContact(i);
    //         //     if (contact.point.y < transform.position.y)
    //         //     {
    //         //         isGrounded = true;
    //         //     }
    //         // }
    //         isGrounded = true;
    //     }
    // }
    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         // for (int i = 0; i < collision.contactCount; i++)
    //         // {
    //         //     ContactPoint2D contact = collision.GetContact(i);
    //         //     if (contact.point.y < transform.position.y)
    //         //     {
    //         //         isGrounded = true;
    //         //     }
    //         // }
    //         isGrounded = false;
    //     }
    // }
    void Update()
    {
        // RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, GetComponent<Collider2D>().bounds.size, 0, Vector2.up);
        // foreach (RaycastHit2D hit in hits)
        // {
        //     if (hit.transform.gameObject.CompareTag("Ground") && transform.InverseTransformPoint(hit.point).y < 0)
        //     {
        //         isGrounded = true;
        //         return;
        //     }
        // }
        // isGrounded = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
        isGrounded = hit && cldr.IsTouching(hit.collider);
    }
    public void Move(Vector2 moveVector)
    {
        if (canMove)
        {
            rb.linearVelocityX = moveVector.x * moveSpeed;
            animator.SetBool(IsWalkingHash, true);
        }
        if (rb.linearVelocityX > 0)
        {
            transform.localScale = Vector3.one * scale;
        } else if (rb.linearVelocityX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1) * scale;
        } else
        {
            animator.SetBool(IsWalkingHash, false);
        }
    }
    public void Jump()
    {
        if (isGrounded && canMove)
        {
            // isGrounded = false;
            rb.linearVelocityY = 0;
            rb.AddForceY(Mathf.Sqrt(jumpHeight * -Physics.gravity.y * 2), ForceMode2D.Impulse);
        }
    }
    public void StartAttack()
    {
        
        if (!animator.GetBool(IsAttackingHash) && canAttack)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        animator.SetBool(IsAttackingHash, true);
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool(IsAttackingHash, false);
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
