using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text live_counter;
    void Update()
    {
        scoreText.text = GameManager.instance.GetScore().ToString();
        live_counter.text = $"x{GameManager.instance.lives}";
    }
}
