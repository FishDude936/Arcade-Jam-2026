using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bounds : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.instance.StartReset();
            Destroy(this);
        }
    }
}
