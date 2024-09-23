using UnityEngine;
using UnityEngine.UI;

public class DarkenScreen : MonoBehaviour
{
    public Image darkPanel;            // 어둡게 할 패널 이미지
    public Test_Score scoreScript;     // 점수를 참조할 Test_Score 스크립트

    // Start에서 패널의 투명도를 0.7로 설정
    void Start()
    {
        if (darkPanel != null)
        {
            Color color = darkPanel.color;
            color.a = 0.7f; // 기본 투명도를 0.7로 설정
            darkPanel.color = color;
        }
    }

    // Update는 매 프레임마다 호출
    void Update()
    {
        if (scoreScript != null)
        {
            // Test_Score 스크립트의 score 변수를 가져옴
            int score = scoreScript.score;

            // 점수에 따라 투명도를 조절
            if (score >= 10 && score < 20)
            {
                Darken(0.4f); // 투명도를 80%로 설정 (덜 투명)
            }
            else if (score >= 20)
            {
                Darken(0f); // 투명도를 100%로 설정 (완전히 어둡게)
            }
            else
            {
                Darken(0.8f); // 기본 투명도 70%로 설정
            }
        }
    }

    // 이 메서드를 호출하면 알파값을 조절하여 화면을 어둡게 또는 밝게 조절
    public void Darken(float alpha)
    {
        if (darkPanel != null)
        {
            Color color = darkPanel.color;
            color.a = Mathf.Clamp01(alpha); // 알파값을 0~1로 설정하여 투명도 조절
            darkPanel.color = color;
        }
    }
}