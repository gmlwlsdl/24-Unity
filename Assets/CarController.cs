// CarController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CarController : MonoBehaviour
{
    // 연결할 GameObject와 설정할 변수들
    GameObject director;
    public float moveSpeed = 5.0f;
    public float acceleration = 2.0f;
    public float deceleration = 5.0f;
    public float maxSpeed = 10.0f;
    public float rotationSpeed = 100.0f;

    private float currentSpeed = 0.0f;
    private float horizontalInput = 0.0f;
    private float verticalInput = 0.0f;

    // 차량의 바퀴 변수들
    public Transform FrontLeftWheel;
    public Transform FrontRightWheel;
    public Transform RearLeftWheel;
    public Transform RearRightWheel;

    public TextMeshProUGUI EndText;

    // 차량이 일정 시간동안 정지하는지 확인하는 변수
    bool isCarFrozen = false;

    // 차량이 도착 지점에 도달했을 때의 목표 오브젝트
    public GameObject targetObject;

    public GameObject Wall2;

    // 트리거 이벤트 처리
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            // Bomb 과 충돌 시 게임 디렉터에게 폭탄 획득 알림
            this.director.GetComponent<GameDirector>().GetBomb();
            UnityEngine.Debug.Log("밤이랑 부딫혔다!");
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Balloon")
        {
            // Balloon 과 충돌 시 게임 디렉터에게 풍선 획득 알림
            this.director.GetComponent<GameDirector>().GetBalloon();
            UnityEngine.Debug.Log("벌룬이랑 부딫혔다!");
            Destroy(other.gameObject);
        }
        else if (Wall2 != null && other.gameObject == Wall2)
        {
            this.director.GetComponent<GameDirector>().ShowGameOver();
            UnityEngine.Debug.Log("헉");
        }
        else if (other.gameObject.tag == "Wall")
        {
            // 벽과 충돌 시 현재 위치에서 반대 방향으로 밀어냄
            Vector3 pushDirection = transform.position - other.transform.position;
            transform.position += pushDirection.normalized * 1.0f; // 원하는 만큼 밀어냄
        }
        else if (targetObject != null && other.gameObject == targetObject)
        {
            // 목표 지점에 도달 시 게임 디렉터에게 게임 완료 알림
            this.director.GetComponent<GameDirector>().Finish();
            UnityEngine.Debug.Log("목표 지점에 도착했다!");
        }
        else
        {
            // Cone 과 충돌 시 게임 디렉터에게 꼬깔이 획득 알림
            this.director.GetComponent<GameDirector>().GetCone();
            UnityEngine.Debug.Log("꼬깔이랑 부딫혔다!");
            Destroy(other.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 디렉터 오브젝트 찾기 및 목표 지점 오브젝트 초기화
        this.director = GameObject.Find("GameDirector");
        targetObject = GameObject.Find("prop_finishline");
        Wall2 = GameObject.Find("prop_adbox_C");

        /* 차량을 일정 시간동안 정지하는 
        코루틴(실행을 일시 중지하고 나중에 다시 시작할 수 있는 특별한 종류의 함수) 실행 */
        StartCoroutine(FreezeCarForSeconds(3.0f));
    }

    /* 차량을 일정 시간동안 정지시키는
    코루틴(실행을 일시 중지하고 나중에 다시 시작할 수 있는 특별한 종류의 함수) */
    IEnumerator FreezeCarForSeconds(float seconds)
    {
        isCarFrozen = true;

        yield return new WaitForSeconds(seconds);
        // 차량이 출발하고 일정 시간(seconds에 지정된 값)이 지난 후에 정지

        isCarFrozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 차량이 정지 중이 아니라면 입력 처리 및 이동 로직 수행
        if (!isCarFrozen)
        {
            // 입력 변수 초기화
            horizontalInput = 0.0f;
            verticalInput = 0.0f;

            // W, S, A, D 키 입력에 따라 수평 및 수직 입력 값 설정
            if (Input.GetKey(KeyCode.W))
            {
                verticalInput = 1.0f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                verticalInput = -1.0f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1.0f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1.0f;
            }

            // 목표 속도 설정 및 현재 속도 업데이트
            float targetSpeed = verticalInput * maxSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

            // 이동 방향 설정
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, currentSpeed).normalized;

            // 이동
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // 차량 회전 처리
            RotateCar(horizontalInput);
        }
    }

    // 차량 회전 메서드
    void RotateCar(float input)
    {
        // 차량 전체 회전
        transform.Rotate(Vector3.up * input * rotationSpeed * Time.deltaTime);

        // 바퀴 회전
        float wheelRotation = input * rotationSpeed * Time.deltaTime;
        RotateWheel(FrontLeftWheel, wheelRotation);
        RotateWheel(FrontRightWheel, wheelRotation);
        RotateWheel(RearLeftWheel, wheelRotation);
        RotateWheel(RearRightWheel, wheelRotation);
    }

    // 바퀴 회전 메서드
    void RotateWheel(Transform wheelTransform, float rotationAmount)
    {
        wheelTransform.Rotate(Vector3.right * rotationAmount);
    }
}