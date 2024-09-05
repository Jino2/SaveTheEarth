using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. UI;

public class ScoreManager : MonoBehaviour
{
    private Text scoreText;
    private int score;

    void Start()
    {
        // ScoreManager 스크립트가 할당된 게임 오브젝트에서 Text 컴포넌트를 찾습니다.
        scoreText = GetComponent<Text>();
        score = 0;
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
