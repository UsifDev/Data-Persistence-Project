
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public Text bestScoreAndNameText;
    public string currName = "";
    public string playerName = "Name";
    public int highScore = 0;
    public InputField nameInput;
    public bool hasEnteredName = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        hasEnteredName = DataManager.Instance.hasEnteredName;
        playerName = DataManager.Instance.playerName;
        highScore = DataManager.Instance.highScore;
        bestScoreAndNameText.text = "Best Score: " + playerName + " : " + highScore;

        if (!hasEnteredName)
        {
            nameInput.gameObject.SetActive(true);
            nameInput.ActivateInputField();
        }
    }

    public void SubmitName(string name)
    {
        currName = nameInput.text;
        hasEnteredName = true;
        DataManager.Instance.hasEnteredName = true;
        nameInput.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space) && hasEnteredName)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }
    }

    public void Exit()
    {
        DataManager.Instance.SaveInfo();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        // change highscore and the name of the score holder to this new name and score
        if(m_Points > highScore){
            playerName = currName;
            highScore = m_Points;
            DataManager.Instance.playerName = playerName;
            DataManager.Instance.highScore = highScore;
            bestScoreAndNameText.text = "Best Score: " + playerName + " : " + highScore;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }


}
