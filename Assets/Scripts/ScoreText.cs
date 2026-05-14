using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    TMP_Text textObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textObject = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = GameManager.instance.GetScoreText();
    }
}
