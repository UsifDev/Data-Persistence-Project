using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string currName = "";
    public string playerName = "Name";
    public int highScore = 0;
    public bool hasEnteredName = false;
    public static DataManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInfo();
    }

    void Start()
    {
        hasEnteredName = DataManager.Instance.hasEnteredName;
        playerName = DataManager.Instance.playerName;
        highScore = DataManager.Instance.highScore;
        currName = DataManager.Instance.currName;
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
    }
    public void SaveInfo()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highScore = data.highScore;
        }
    }

}
