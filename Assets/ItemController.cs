// ItemController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float radius = 0.05f;  // 원의 반지름
    public float speed = 100.0f;   // 원을 따라 움직이는 속도

    private float angle = 0f;  // 현재 각도

    void Start()
    {
        // 오브젝트의 초기 위치를 기준으로 원을 그리기 위해
        // 초기 각도를 설정합니다.
        Vector3 initialPosition = transform.position;
        angle = Mathf.Atan2(initialPosition.z, initialPosition.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        // 각도를 라디안으로 변환
        float radianAngle = Mathf.Deg2Rad * angle;

        // x, z 좌표를 중심으로 하는 원 위의 점 계산
        float x = transform.position.x + Mathf.Cos(radianAngle) * radius;
        float z = transform.position.z + Mathf.Sin(radianAngle) * radius;

        // y 좌표는 고정
        float y = transform.position.y;

        // 새로운 위치를 설정
        transform.position = new Vector3(x, y, z);

        // 각도를 업데이트하여 움직임을 생성
        angle += speed * Time.deltaTime;

        // 각도를 360도로 유지
        if (angle > 360f)
        {
            angle -= 360f;
        }
    }
}