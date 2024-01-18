// GameDirector.cs
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    // 게임 오브젝트 및 변수 선언
    GameObject timeText;
    GameObject startTimeText;

    float startTime = 3.0f;
    public float time = 50.0f;
    public float score = 0.0f;

    public TextMeshProUGUI startUIText;
    public TextMeshProUGUI EndText;
    public TextMeshProUGUI finalstatsText;
    public TextMeshProUGUI finishText;
    Slider hpBarSlider;

    public float curHealth = 0.0f;
    public float maxHealth = 100.0f;
    private bool startTextCoroutineStarted = false;

    // 게임 종료 화면을 보여주는 메서드
    public void ShowGameOver()
    {
        // "Game Over" 텍스트를 활성화
        EndText.gameObject.SetActive(true);
        score = calc(curHealth, time);

        // 현재 hp과 time을 표시
        EndText.text = "Game Over";
        finalstatsText.text = "Health : " + curHealth.ToString() + "\nTime : " + time.ToString("F1") + "\nScore :" + score.ToString();

        // 게임을 멈춤
        Time.timeScale = 0f;

        // 게임 재시작을 위해 R 키 입력 대기
        StartCoroutine(WaitForKeyPress());
    }

    // 타이머 업데이트 메서드
    public void timer()
    {
        time -= Time.deltaTime;
        string second = (time % 60).ToString("00");
        startTimeText.GetComponent<TextMeshProUGUI>().text = second;
        timeText.GetComponent<TextMeshProUGUI>().text = time.ToString("F1") + " s";
    }

    // 시작 메시지를 숨기는 코루틴(실행을 일시 중지하고 나중에 다시 시작할 수 있는 특별한 종류의 함수)
    IEnumerator HideStartTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        startUIText.text = "";
    }

    // 체력 체크 및 게임 종료 여부 확인
    public void CheckHp()
    {
        if (hpBarSlider != null)
        {
            hpBarSlider.value = curHealth / maxHealth;
            if (curHealth <= 0)
            {
                ShowGameOver();
            }
        }
    }

    // 플레이어가 데미지를 받는 메서드
    public void Damage(float damage)
    {
        if (maxHealth == 0 || curHealth <= 0)
            return;
        curHealth -= damage;
        CheckHp();
        if (curHealth <= 0)
        {
            // 플레이어 사망 또는 처리할 내용 추가
            ShowGameOver();
        }
    }

    // 플레이어가 Cone 아이템을 획득할 때 호출되는 메서드
    public void GetCone()
    {
        Damage(20.0f);
        CheckHp();
    }

    // 플레이어가 Bomb 아이템을 획득할 때 호출되는 메서드
    public void GetBomb()
    {
        time -= 5.0f;
        UpdateTimeText();
    }

    // 플레이어가 Balloon 아이템을 획득할 때 호출되는 메서드
    public void GetBalloon()
    {
        time += 5.0f;
        UpdateTimeText();
    }

    // 시간 업데이트 및 UI 갱신 메서드
    void UpdateTimeText()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = time.ToString("F1") + " s";
    }

    float calc(float hp, float time)
    {
        return hp * 10.0f + time * 5.5f;
    }

    // 게임 성공 화면을 보여주는 메서드
    public void Finish()
    {
        finishText.gameObject.SetActive(true);
        score = calc(curHealth, time);

        // 현재 체력과 시간을 표시
        finishText.text = "Game Success!";
        finalstatsText.text = "Health : " + curHealth.ToString() + "\nTime : " + time.ToString("F1") + "\nScore :" + score.ToString();

        // 게임을 멈춤
        Time.timeScale = 0f;
        // 게임 재시작을 위해 R 키 입력 대기
        StartCoroutine(WaitForKeyPress());
    }

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        timeText = GameObject.Find("Time");
        startTimeText = GameObject.Find("StartTime");
        hpBarSlider = GameObject.Find("Slider").GetComponent<Slider>();

        startUIText = GameObject.Find("StartText").GetComponent<TextMeshProUGUI>();

        // 추가된 부분
        finishText = GameObject.Find("finishText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;

            string second = startTime.ToString("0");
            startTimeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", second);

            if (startTime <= 0 && !startTextCoroutineStarted)
            {
                startTimeText.SetActive(false);
                startUIText.text = "start!";

                StartCoroutine(HideStartTextAfterDelay(1.0f));  // 1초 후에 숨김
                startTextCoroutineStarted = true;
            }
        }
        else
        {
            // startTime이 0 이하인 경우에만 timer 함수 호출
            timer();

            // time이 0이 되면 ShowGameOver() 호출
            if (time <= 0)
            {
                ShowGameOver();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            // 게임 재시작을 위해 R 키 입력 대기
            StartCoroutine(WaitForKeyPress());
        }
    }

    // R 키 입력을 대기하는 코루틴(실행을 일시 중지하고 나중에 다시 시작할 수 있는 특별한 종류의 함수)
    IEnumerator WaitForKeyPress()
    {
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null; // 현재 프레임에서 일시 중지하고 다음 프레임으로 넘어감
        }

        // R 키를 누르면 현재 씬을 다시 로드
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}