using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] RectTransform scoreUI;
    [SerializeField] GameObject scorePrefab;
    [SerializeField] TMP_InputField inputField;
    private string filepath;
    void Awake()
    {
        filepath = Application.persistentDataPath + "/highscores.data";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveData data = LoadScores();
        Debug.Log("Before Scores:");
        LogScores(data);
        data = UpdateScores(data);
        Debug.Log("After Scores:");
        LogScores(data);
        UpdateLeaderboard(data);
        SaveScores(data);
    }
    void SaveScores(SaveData data)
    {
        FileStream datastream = new(filepath, FileMode.Create);

        BinaryFormatter converter = new();
        converter.Serialize(datastream, data);

        datastream.Close();
    }
    SaveData UpdateScores(SaveData data)
    {
        if (GameManager.instance && GameManager.instance.GetScore() > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                if (data.highscores[i] < GameManager.instance.GetScore())
                {
                    inputField.transform.parent.gameObject.SetActive(true);
                    // while (inputField.text.Length < 3)
                    // {
                    //     inputField.ActivateInputField();
                    // }
                    // string name = inputField.text;
                    string name = "JEM";
                    inputField.transform.parent.gameObject.SetActive(false);
                    for (int j = 9; j > i; j--)
                    {
                        data.highscores[j] = data.highscores[j - 1];
                        data.names[j] = data.names[j - 1];
                    }
                    data.highscores[i] = GameManager.instance.GetScore();
                    data.names[i] = name;
                    return data;
                }
            }
        }
        return data;
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
                    scoreObject.GetComponent<TMP_Text>().text = $"{scores.names[i]} - {scores.highscores[i]}";
                    scoreObject.GetComponent<TMP_Text>().enabled = true;
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
