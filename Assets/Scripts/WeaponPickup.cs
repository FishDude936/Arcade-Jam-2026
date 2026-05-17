using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    Vector3 startPos;
    bool goingRight = false;
    [SerializeField] float frequency = 1, amplitude = 1;
    [SerializeField] float moveSpeed = 2f;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().canAttack = true;
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Ground"))
        {
            goingRight = !goingRight;
        }
    }
    void Start()
    {
        startPos = transform.position;
        // random starting direction
        goingRight = Random.Range(0, 1) == 0;
    }
    void FixedUpdate()
    {
        Vector3 newPos = new(transform.position.x + (goingRight ? moveSpeed : -moveSpeed) * Time.deltaTime, startPos.y + amplitude * Mathf.Sin(Time.time * frequency));
        transform.position = newPos;
    }
}
