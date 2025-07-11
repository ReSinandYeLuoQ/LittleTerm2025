using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] monsterPrefabs; // Index: 0=Lv1, 1=Lv2, 2=Lv3
    public float spawnInterval = 2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        if (!player) return;

        Vector3 spawnPos = Vector3.zero;
        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < 10; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 15f;
            float dx = Mathf.Abs(offset.x);
            float dy = Mathf.Abs(offset.y);

            if ((dx >= 10f && dx <= 15f) || (dy >= 5f && dy <= 10f))
            {
                spawnPos = playerPos + new Vector3(offset.x, offset.y, 0);
                break;
            }
        }

        int level = GameTimeManager.GetMonsterLevel();
        GameObject prefab = monsterPrefabs[level - 1];
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
