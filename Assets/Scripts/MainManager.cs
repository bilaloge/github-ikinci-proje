using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreName;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public string playerName;
    private string l_Name;
    private int l_Points;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
        if (StartMenu.Instance != null)
        {
            playerName = StartMenu.Instance.playerName;
        }
        LoadPoint();
        if (l_Points > m_Points)
        {
            ScoreName.text = "Best Score  : " + l_Name +" : " + l_Points;
        }
        else
        {
            ScoreName.text = "Best Score  : " + playerName + " : " + m_Points;
        }
    }
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
            }
        }
    }
    [System.Serializable]
    public class SaveData
    {
        public int m_Points;
        public string m_Name;
    }
    public void SavePoint()
    {
        SaveData data = new SaveData();
        data.m_Points = m_Points;
        data.m_Name = playerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.Json", json);
    }
    public void LoadPoint()
    {
        string path = Application.persistentDataPath + "/savefile.Json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            l_Points = data.m_Points;
            l_Name = data.m_Name;
        }
    }
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = "Score :" + m_Points;
    }
    public void GameOver()
    {
        if (m_Points > l_Points)
        {
            SavePoint();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
