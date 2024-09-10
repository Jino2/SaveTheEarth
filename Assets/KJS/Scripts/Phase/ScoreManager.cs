using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text scoreText;
    private int score;

    void Start()
    {
        scoreText = GetComponent<Text>();
        score = 0;
        UserApi.GetUserInfo("test", info =>
        {
            score = info.point;
            UpdateScoreText();

        });
        UpdateScoreText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            score++;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}