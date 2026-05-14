using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds3 = new(3);
    public static GameManager instance;
    [Header("Variables")]
    public int tempScore = 0;
    private int score = 0;
    private float lastInputTime = 0;
    // [Header("Object References")]
    
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InputSystem.onAnyButtonPress.Call(ChangeInputTime);
        } else
        {
            Destroy(gameObject);
        }
    }
    public void StartReset()
    {
        tempScore = 0;
        StartCoroutine(Reset());
    }
    IEnumerator Reset()
    {
        yield return _waitForSeconds3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartNext()
    {
        score += tempScore;
        tempScore = 0;
        StartCoroutine(Next());
    }
    IEnumerator Next()
    {
        yield return _waitForSeconds3;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == SceneManager.sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
    public string GetScoreText()
    {
        return (score + tempScore).ToString();
    }
    void ChangeInputTime(InputControl button)
    {
        Debug.Log($"{button.displayName} pressed at {Time.time}");
        lastInputTime = Time.time;
    }
    void Update()
    {
        if (Time.deltaTime - lastInputTime >= 180)
        {
            // input timer requirement for arcade machine
            Application.Quit();
        }
    }
}
