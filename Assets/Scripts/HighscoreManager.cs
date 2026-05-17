using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.Collections;

public class HighscoreManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_3 = new WaitForSeconds(0.3f);
    [SerializeField] RectTransform scoreUI;
    [SerializeField] GameObject scorePrefab;
    [SerializeField] TMP_Text inputField;
    [SerializeField] char[] availableNameChars;
    private string filepath;
    void Awake()
    {
        filepath = Application.persistentDataPath + "/highscores.data";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveData data = LoadScores();
        // Debug.Log("Before Scores:");
        // LogScores(data);
        UpdateLeaderboard(data);
        if (GameManager.instance && GameManager.instance.GetScore() > 0)
        {
            StartCoroutine(UpdateScores(data));
        }
        // Debug.Log("After Scores:");
        // LogScores(data);
        SaveScores(data);
    }
    void SaveScores(SaveData data)
    {
        FileStream datastream = new(filepath, FileMode.Create);

        BinaryFormatter converter = new();
        converter.Serialize(datastream, data);

        datastream.Close();
    }
    IEnumerator UpdateScores(SaveData data)
    {
        yield return new WaitForEndOfFrame();
        int greaterIndex = -1;
        int i = 0;
        while (i < 10 && greaterIndex == -1)
        {
            if (data.highscores[i] < GameManager.instance.GetScore())
            {
                greaterIndex = i;
            }
            i++;
        }
        if (greaterIndex != -1)
        {
            inputField.transform.parent.gameObject.SetActive(true);
            int currLetter = 0;
            int currRow = 0;
            char[] name = new char[3];
            while (currRow < 3)
            {
                Vector2 movementVector = GameManager.instance.GetMovementVector();
                if (movementVector.y == 1)
                {
                    currLetter = (currLetter + 1) % 26;
                    yield return _waitForSeconds0_3;
                } else if (movementVector.y == -1)
                {
                    currLetter = currLetter == 0 ? 25 : currLetter - 1;
                    yield return _waitForSeconds0_3;
                }
                name[currRow] = availableNameChars[currLetter];
                if (movementVector.x == -1 && currRow != 0)
                {
                    currRow--;
                    currLetter = 0;
                    yield return _waitForSeconds0_3;
                } else if (movementVector.x == 1)
                {
                    currRow++;
                    currLetter = 0;
                    yield return _waitForSeconds0_3;
                }
                inputField.text = name.ArrayToString();
                yield return new WaitForEndOfFrame();
            }
            // string name = inputField.text;
            inputField.transform.parent.gameObject.SetActive(false);
            for (int j = 9; j > greaterIndex; j--)
            {
                data.highscores[j] = data.highscores[j - 1];
                data.names[j] = data.names[j - 1];
            }
            data.highscores[greaterIndex] = GameManager.instance.GetScore();
            data.names[greaterIndex] = name.ArrayToString();
            UpdateLeaderboard(data);
            SaveScores(data);
        }
    }
    SaveData LoadScores()
    {
        if (File.Exists(filepath))
        {
            FileStream datastream = new(filepath, FileMode.Open);

            BinaryFormatter converter = new();
            SaveData data = converter.Deserialize(datastream) as SaveData;

            datastream.Close();
            return data;
        } else
        {
            Debug.Log($"Data not found at {filepath}");
            return new SaveData();
        }
    }
    void UpdateLeaderboard(SaveData scores)
    {
        for (int i = 0; i < scoreUI.childCount; i++)
        {
            Destroy(scoreUI.GetChild(i).gameObject);
        }
        if (scores != null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (scores.highscores[i] > 0)
                {
                    GameObject scoreObject = Instantiate(scorePrefab, scoreUI);
                    TMP_Text scoreText = scoreObject.GetComponent<TMP_Text>();
                    scoreText.text = $"{scores.names[i]} - {scores.highscores[i]}";
                    scoreText.enabled = true;
                    scoreText.color = i == 0 ? Color.gold : i == 1 ? Color.silver : i == 2 ? Color.rosyBrown : Color.white;
                    scoreObject.transform.localPosition = new Vector3(scoreObject.transform.position.x, 400 - (100 * i), 0);
                }
            }
        }
    }
    void LogScores(SaveData scores)
    {
        for (int i = 0; i < 10; i++)
        {
            if (scores.highscores[i] > 0)
            {
                Debug.Log($"{scores.names[i]} - {scores.highscores[i]}");
            }
        }
    }
}
