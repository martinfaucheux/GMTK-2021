using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] GameObject wavePrefab;

    [SerializeField] float spawnPeriod;
    [SerializeField] int preSpawnCount;
    [SerializeField] float spawnRandomFactor;
    [SerializeField] float waveDuration;

    [SerializeField] float xMax;
    [SerializeField] float yMax;

    private float nextSpawnTime;

    void Start(){

        for(int i = 0; i< preSpawnCount; i++ ){
            Spawn();
        }
    }

    void Update(){
        if(nextSpawnTime < Time.time){
            print("Spawn");
            Spawn();
            nextSpawnTime = GetNextSpawnTime();
        }
    }

    private void Spawn(){

        Vector3 newPos = new Vector3(
            Random.Range(-xMax, xMax), Random.Range(-yMax, yMax), 0f
        );

        GameObject wave = Instantiate(wavePrefab, newPos, Quaternion.identity, transform);


        // wave.transform.localScale = new Vector3(1, 0f, 1f);
        // LeanTween.scale(wave, Vector3.one, waveDuration).setLoopPingPong(1).setOnComplete(()=> Destroy(wave));
        
        // TODO: change color to no alpha
        // LeanTween.color(wave, )

        StartCoroutine(DelayDelete(wave));
    }

    private float GetNextSpawnTime(){
        return Time.time + spawnPeriod + spawnRandomFactor * Random.Range(-1f, 1f);
    }

    private IEnumerator DelayDelete(GameObject gameObject){
        yield return new WaitForSeconds(waveDuration * 2);
        Destroy(gameObject);
    }
}
