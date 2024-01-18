// PlayerCameraController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    // 플레이어의 Transform을 저장할 변수
    Transform playerTransform;

    // 초기 상대 위치(offset)를 저장할 변수
    Vector3 offset;

    // 카메라 이동을 부드럽게 하기 위한 변수
    public float smoothSpeed = 0.5f;

    // 시야에서의 Y 좌표에 더해질 값
    public float offsetY = -3f;

    // Y 좌표의 최소 및 최대 값
    public float minY = -5.5f;
    public float maxY = 2f;

    // 스크립트가 활성화될 때 호출되는 Awake 메서드
    void Awake()
    {
        // 플레이어를 찾아서 Transform 저장
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 초기 offset 설정
        offset = playerTransform.position - transform.position;

        // 초기 카메라 위치 설정
        Vector3 initialPosition = playerTransform.position - playerTransform.forward * offset.magnitude;
        initialPosition.y = Mathf.Clamp(initialPosition.y - offsetY, minY, maxY);
        transform.position = initialPosition;

        // 초기 카메라 회전 설정
        transform.LookAt(playerTransform);
    }

    // 다른 업데이트가 끝난 후에 Update가 적용됨 -> 좀 더 부드러운 카메라 모션 유도
    void LateUpdate()
    {
        // 부드러운 이동을 위해 Lerp 사용
        Vector3 desiredPosition = playerTransform.position - playerTransform.forward * offset.magnitude;

        // Y 값을 최소 및 최대 값으로 제한 후, 시야에서의 Y 좌표에 offsetY를 빼줌
        desiredPosition.y = Mathf.Clamp(desiredPosition.y - offsetY, minY, maxY);

        // 부드러운 이동 적용
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 플레이어의 위치를 바라보는 정면으로 회전 조절
        transform.LookAt(playerTransform);
    }
}