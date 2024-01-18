// MinimapCameraController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    // 플레이어 오브젝트를 가리키는 변수
    public GameObject Player;

    // 카메라의 상대적인 x, y, z 좌표
    public float offsetX = 5.0f;
    public float offsetY = 100.0f;
    public float offsetZ = -10.0f;

    // 카메라 이동 속도
    public float CameraSpeed = 10.0f;

    // 플레이어의 현재 위치를 저장하는 변수
    Vector3 PlayerPos;

    // FixedUpdate 메서드는 고정된 주기로 호출되는 메서드
    // -> 게임이 불안정한 프레임 속도로 실행될 때도 물리 시뮬레이션은 안정적으로 작동
    void FixedUpdate()
    {
        // 플레이어의 위치에 상대적인 카메라의 위치를 계산
        PlayerPos = new Vector3(
            Player.transform.position.x + offsetX,
            Player.transform.position.y + offsetY,
            Player.transform.position.z + offsetZ
        );

        // 부드러운 이동을 위해 Lerp를 사용하여 카메라 위치 조절
        transform.position = Vector3.Lerp(transform.position, PlayerPos, Time.deltaTime * CameraSpeed);

        // 플레이어의 회전 값을 가져와 카메라의 회전에 적용
        Quaternion playerRotation = Player.transform.rotation;
        transform.rotation = Quaternion.Euler(90.0f, playerRotation.eulerAngles.y, 0.0f);
    }
}