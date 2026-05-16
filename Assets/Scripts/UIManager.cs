using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Object References")]
    private PlayerController player;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Transform hearts;
    void Update()
    {
        scoreText.text = GameManager.instance.GetScore().ToString();
        for (int i = 1; i < 4; i++)
        {
            hearts.Find(i.ToString()).gameObject.SetActive(GameManager.instance.lives >= i);
        }
    }
}
