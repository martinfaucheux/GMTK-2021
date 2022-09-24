using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandRandomizer : MonoBehaviour
{
    public GameObject[] sandPlacementPrefabs;
    public string containerName = "SandPlacement";

    public void Place()
    {
        GameObject prefab = sandPlacementPrefabs[Random.Range(0, sandPlacementPrefabs.Length)];
        Quaternion rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 4));

        GameObject.Instantiate(prefab, transform.position, rotation, transform);

    }

    public void Delete()
    {
        Transform toDestroy = null;
        foreach (Transform childTransform in transform)
        {
            if (childTransform.name == containerName)
                toDestroy = childTransform;
        }
        if (toDestroy != null)
            Destroy(toDestroy.gameObject);
    }
}
