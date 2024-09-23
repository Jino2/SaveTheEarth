using UnityEngine;
using TMPro;  // TextMeshPro 관련 네임스페이스

public class Test_Score : MonoBehaviour
{
    public TMP_Text scoreText;  // TextMeshPro 컴포넌트를 연결하기 위한 변수
    public int score = 0;      // 점수 변수

    // Start is called before the first frame update
    void Start()
    {
        // 처음 시작할 때 점수를 초기화해서 TextMeshPro에 출력
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        // Q 키를 누르면 점수를 증가
        if (Input.GetKeyDown(KeyCode.Q))
        {
            score += 1;  // 점수를 1 증가
            UpdateScoreText();  // 텍스트를 갱신
        }
    }

    // 점수를 TextMeshPro UI에 업데이트하는 함수
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();  // 점수를 텍스트로 변환 후 출력
        }
    }
}
