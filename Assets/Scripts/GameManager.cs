using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds3 = new(3);
    public InputSystem_Actions m_actions;
    private InputSystem_Actions.UIActions m_UI;
    public static GameManager instance;
    [Header("Variables")]
    public int lives = 3;
    public int tempScore = 0;
    private int score = 0;
    private int oneUps = 1;
    private float lastInputTime = 0;
    public float levelStartTime;
    // [Header("Object References")]
    
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InputSystem.onAnyButtonPress.Call(ChangeInputTime);
            m_actions = new();
            m_UI = m_actions.UI;
            m_UI.Enable();
            m_UI.MiddleClick.started += ctx => Application.Quit();
            m_UI.StartGame.started += ctx => StartGame();
        } else
        {
            Destroy(gameObject);
        }
    }
    public void StartReset()
    {
        AudioManager.instance.PlaySound("GameOver");
        StartCoroutine(Reset());
    }
    IEnumerator Reset()
    {
        yield return _waitForSeconds3;
        if (lives == 0)
        {
            SceneManager.LoadScene(0);
        } 
        else {
            tempScore = 0;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void StartNext()
    {
        AudioManager.instance.PlaySound("LevelClear");
        tempScore += Mathf.CeilToInt(1000/(Time.time - levelStartTime));
        score += tempScore;
        tempScore = 0;
        if (score >= 1000 * oneUps)
        {
            lives++;
            oneUps++;
        }
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
        levelStartTime = Time.time;
    }
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            score = 0;
            tempScore = 0;
            lives = 3;
            SceneManager.LoadScene(1);
            levelStartTime = Time.time;
        }
    }
    public int GetScore()
    {
        return score + tempScore;
    }
    public Vector2 GetMovementVector()
    {
        return m_UI.NameSelect.ReadValue<Vector2>();
    }
    void ChangeInputTime(InputControl button)
    {
        Debug.Log($"{button.displayName} pressed at {Time.time}");
        lastInputTime = Time.time;
    }
    void Update()
    {
        float deltaInputTime = Time.deltaTime - lastInputTime;
        if (deltaInputTime >= 180)
        {
            // input timer requirement for arcade machine
            Application.Quit();
        }
    }
}
