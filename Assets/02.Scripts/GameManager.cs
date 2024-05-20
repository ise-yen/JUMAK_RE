using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 게임 제한 시간 2분 40초
    public Text timeText;
    [SerializeField]
    float GameTime = 160f;

    bool isTimer = true;
    bool GameOver = false;
    int min;
    int sec;

    public Text scoreText;
    public int GameScore = 0;

    public Text finalScoreText;

    public GameObject textCanvas;
    public GameObject gameOverCanvas;
    
    PlayerSkill playerskill;

    private void Awake()
    {
        instance = this;
        playerskill = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkill>();
    }

    private void Start()
    {
        gameOverCanvas.SetActive(false);
        textCanvas.SetActive(true);
    }

    void Update()
    {
        StartCoroutine(GameTimer());
        UIScore();
        if(GameOver == true)
        {
            gameOverCanvas.SetActive(true);
            finalScoreText.text = GameScore.ToString("0");
            textCanvas.SetActive(false);

        }

    }

    void UIScore()
    {
        GameScore = playerskill.score;
        scoreText.text = GameScore.ToString("0");
    }

    IEnumerator GameTimer()
    {
        if (GameTime <= 0)
        {
            isTimer = false;
            GameOver = true;
        }

        if (isTimer)
        {
 
            min = Mathf.FloorToInt(GameTime / 60);
            sec = Mathf.FloorToInt(GameTime % 60);
            timeText.text = min.ToString("00") + ":" + sec.ToString("00");
            if (GameTime < 10f)
            {
                timeText.color = Color.red;
            }
            else
            {
                timeText.color = Color.white;
            }
            GameTime -= Time.deltaTime;
            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
