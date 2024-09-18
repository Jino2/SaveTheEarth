using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 싱글톤으로 관리
    public static LoginManager lobbyUI;

    // UI 재입력
    public GameObject panel_Login;
    public GameObject panel_Logout;
    public GameObject panel_Logouting;
    public GameObject panel_Main;
    public GameObject panel_MainLogout;
    public GameObject panel_Remain;
    public GameObject panel_ServiceExit;

    public Button btn_Check;
    // 이미지 게임오브젝트?
    public Image img_Logining;

    //// InputField를 연결할 변수
    //public InputField inputField_ID;
    //// 출력할 Text를 연결할 변수
    //public Text outputText_ID;        

    private void Awake()
    {
        if (lobbyUI == null)
        {
            lobbyUI = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // btn_Check의 클릭 이벤트에 Login_Check 메서드 등록
        btn_Check.onClick.AddListener(Login_Check);
    }

    void Update()
    {
        
    }

    // 씬 딜레이 주기
    public void SceneDelay(float time)
    {
        Invoke("SceneChange", time);
    }

    // 씬 이동 하는 함수 
    public void SceneChange()
    {        
        SceneManager.LoadScene("HyeJi");
    }

    // 패널을 활성화/비활성화하는 함수
    private void ShowPanel(GameObject panel, bool show)
    {
        panel.SetActive(show);
    }

    // 메인 메뉴 -> 로그인 눌렀을 때
    public void Main_Login()
    {
        // 메인 메뉴 판넬 꺼지고
        ShowPanel(panel_Main, false);
        // 로그인 버튼 누르면 로그인 화면으로 감
        ShowPanel(panel_Login, true);
    }

    // 로그인 -> 확인 버튼 눌렀을 때
    public void Login_Check()
    {
        // 로그인중 ... 어쩌고 이미지 뜨게하고
        img_Logining.enabled = true;

        // 입력된 텍스트를 출력 텍스트에 설정
        //outputText_ID.text = inputField_ID.text;

        // 로그인 판넬 꺼버리고
        ShowPanel(panel_Login, false);
        // 로그인 성공 Remain 판넬 키자
        ShowPanel(panel_Remain, true);

        // 2초 지나면 다음 씬으로 넘어가게 하고 싶음
        SceneDelay(2f);
    }

    // 메인 메뉴 -> 서비스 나가기 눌렀을 때
    public void Main_ServiceExit()
    {
        // 서비스 나가기 버튼 누르면 메인 메뉴 판넬 꺼지고
        ShowPanel(panel_Main, false);
        // 서비스 나가기 진짜함? 이 판넬 뜸
        ShowPanel(panel_ServiceExit, true);
    }


    // 나가기 버튼 활성화 (임시 false)
    public void Exit_btn()
    {
        // 누르면 나가
    }
}
