using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamLine : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float spawnPeriod = 0.1f;
    public int initSpawnCount = 5;
    public GameObject foamPrefab;
    private float _lastSpawnTime;
    private static string poolKey = "foamBubble";

    void Start()
    {
        for (int i = 0; i < initSpawnCount; i++)
            Spawn();
    }

    void Update()
    {
        if (Time.time > _lastSpawnTime + spawnPeriod)
            Spawn();
    }

    private void Spawn()
    {
        GameObject newObject = GameObjectPool.instance.GetOrCreate(poolKey);
        newObject.transform.position = GetRandomPos();
        newObject.transform.SetParent(transform);

        // This will trigger the OnEnable of the FoamBubble script
        newObject.SetActive(true);
        newObject.GetComponent<FoamBubble>().Animate();

        _lastSpawnTime = Time.time;
    }

    private Vector3 GetRandomPos()
    {
        float t = Random.Range(0f, 1f);
        return point1.position + t * (point2.position - point1.position);
    }
}
