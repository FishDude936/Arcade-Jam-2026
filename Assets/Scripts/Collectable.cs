using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] int scoreValue = 10;
    [SerializeField] float frequency = 1, amplitude = 1;
    private Vector3 startPos;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound("EatFood");
            GameManager.instance.tempScore += scoreValue;
            Destroy(gameObject);
        }
    }
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.position = startPos + (amplitude * Mathf.Sin(Time.time * frequency) * Vector3.up);
    }
}
