using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public GameObject trailPrefab; // 残像用のプレハブ
    public float spawnInterval = 0.1f; // 残像生成間隔
    public float trailDuration = 1.0f; // 残像の表示時間

    private float _lastSpawnTime;

    private void Update()
    {
        // 残像生成間隔ごとに新しい残像を生成
        if (Time.time - _lastSpawnTime > spawnInterval)
        {
            GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            Destroy(trail, trailDuration); // 残像を一定時間後に削除
            _lastSpawnTime = Time.time;
        }
    }
}