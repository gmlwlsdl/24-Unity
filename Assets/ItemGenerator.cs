// ItemGenerator.cs
using System.Collections;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    // Game Object로 아이템 Prefab들을 할당
    public GameObject bombPrefab;
    public GameObject balloonPrefab;

    // 각 아이템이 생성될 x, z 좌표 범위
    public Vector2 spawnRangeX1 = new Vector2(-157.4f, 159.4f);
    public Vector2 spawnRangeZ1 = new Vector2(-35.29f, -21.35f);
    public Vector2 spawnRangeX2 = new Vector2(0f, 5f);
    public Vector2 spawnRangeZ2 = new Vector2(0f, 5f);

    // 고정된 y 좌표
    public float fixedY = 79.5f;

    // 각 범위별 초기에 생성할 아이템 개수
    public int initialItemCountPerRange = 20;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 아이템 생성
        SpawnInitialItems(spawnRangeX1, spawnRangeZ1);
        SpawnInitialItems(spawnRangeX2, spawnRangeZ2);
    }

    // 초기 아이템 생성 메서드
    void SpawnInitialItems(Vector2 xRange, Vector2 zRange)
    {
        for (int i = 0; i < initialItemCountPerRange; i++)
        {
            SpawnRandomItem(xRange, zRange);
        }
    }

    // 랜덤한 아이템 생성 메서드
    void SpawnRandomItem(Vector2 xRange, Vector2 zRange)
    {
        // x 및 z 좌표를 지정된 범위에서 랜덤하게 설정
        float randomX = UnityEngine.Random.Range(xRange.x, xRange.y);
        float randomZ = UnityEngine.Random.Range(zRange.x, zRange.y);

        // 고정된 y 좌표로 아이템 생성
        Vector3 spawnPosition = new Vector3(randomX, fixedY, randomZ);

        // 두 가지 아이템 중에서 랜덤으로 선택
        GameObject selectedPrefab = UnityEngine.Random.Range(0f, 1f) > 0.5f ? bombPrefab : balloonPrefab;

        // 선택된 아이템 Prefab을 인스턴스화하여 생성
        Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
    }
}
