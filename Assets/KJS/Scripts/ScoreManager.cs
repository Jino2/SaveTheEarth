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
        // ScoreManager ��ũ��Ʈ�� �Ҵ�� ���� ������Ʈ���� Text ������Ʈ�� ã���ϴ�.
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
