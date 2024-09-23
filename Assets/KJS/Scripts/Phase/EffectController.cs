using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectController : MonoBehaviour
{
    public Test_Score phaseController; // PhaseController 스크립트 참조
    public TextMeshProUGUI statusText; // TMP Text 오브젝트
    public GameObject panel; // 패널 오브젝트 (public으로 할당)
    public float textDisplayDuration = 2.0f; // 텍스트가 표시되는 시간 (초 단위)

    public int primaryActivateScore;  // 첫 번째 점수에서 텍스트와 패널을 출력할 점수
    public int secondaryActivateScore; // 두 번째 점수에서 텍스트와 패널을 출력할 점수

    private float textTimer; // 텍스트와 패널이 사라지기까지 남은 시간
    private bool hasDisplayedPrimaryText = false; // 첫 번째 점수에서 텍스트와 패널이 이미 출력되었는지 여부
    private bool hasDisplayedSecondaryText = false; // 두 번째 점수에서 텍스트와 패널이 이미 출력되었는지 여부

    private Color textColor;

    private void Start()
    {
        // 게임 시작 시 텍스트와 패널 비활성화
        statusText.gameObject.SetActive(false);
        panel.SetActive(false);
        textTimer = 0f;

        // HTML 색상 코드 #11235A을 Color로 변환하여 텍스트에 적용
        if (ColorUtility.TryParseHtmlString("#03AED2", out textColor))
        {
            statusText.color = textColor;
        }
    }

    private void Update()
    {
        // 현재 점수를 가져옴
        int currentScore = phaseController.score;

        // 첫 번째 점수에 도달했고, 텍스트와 패널이 아직 출력되지 않았다면
        if (currentScore >= primaryActivateScore && !hasDisplayedPrimaryText)
        {
            // 텍스트와 패널을 출력하고 타이머 시작
            statusText.gameObject.SetActive(true);
            panel.SetActive(true);
            statusText.text = "바다의 기름때가 줄어들었습니다.";
            textTimer = textDisplayDuration; // 텍스트를 일정 시간 동안 표시
            hasDisplayedPrimaryText = true; // 텍스트와 패널이 한 번 출력되었음을 기록
        }

        // 두 번째 점수에 도달했고, 텍스트와 패널이 아직 출력되지 않았다면
        if (currentScore >= secondaryActivateScore && !hasDisplayedSecondaryText)
        {
            // 텍스트와 패널을 출력하고 타이머 시작
            statusText.gameObject.SetActive(true);
            panel.SetActive(true);
            statusText.text = "바다의 기름때가 또 줄어들었습니다.";
            textTimer = textDisplayDuration; // 텍스트를 일정 시간 동안 표시
            hasDisplayedSecondaryText = true; // 텍스트와 패널이 한 번 출력되었음을 기록
        }

        // 텍스트와 패널이 활성화된 경우, 타이머가 줄어들며 시간이 지나면 비활성화
        if (statusText.gameObject.activeInHierarchy && panel.activeInHierarchy)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0)
            {
                statusText.gameObject.SetActive(false);
                panel.SetActive(false);
            }
        }
    }
}