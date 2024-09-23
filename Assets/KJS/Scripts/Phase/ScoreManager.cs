using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        score = 0;
        UserApi.GetUserInfo(UserCache.GetInstance().Id, info =>
        {
            UserCache.GetInstance().Point = info.point;
        });
    }

    private void Update()
    {
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "보유 포인트: " + UserCache.GetInstance().Point.ToString();
    }
}