using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelZone
{
    public Transform container;
    public int minLevel;
    public Color buttonColor;
    public Color buttonTextColor;
    public Material treePalmMaterial;
    public Material treeTrunkMaterial;

}

public class LevelButtonInitializer : MonoBehaviour
{
    [SerializeField] Transform _buttonContainer;
    [SerializeField] GameObject _levelButtonPrefab;
    [SerializeField] List<LevelZone> _levelZones;

    void Start()
    {
        // Clean pre-placed buttons
        foreach (LevelZone levelZone in _levelZones)
        {
            foreach (Transform childTransform in levelZone.container)
                Destroy(childTransform.gameObject);
        }

        int zoneIndex = 0;
        for (int sceneId = 1; sceneId < LevelLoader.instance.maxLevelId; sceneId++)
        {
            if (_levelZones.Count > zoneIndex + 1 && _levelZones[zoneIndex + 1].minLevel == sceneId)
                zoneIndex += 1;

            LevelZone levelZone = _levelZones[zoneIndex];
            GameObject newButton = Instantiate(_levelButtonPrefab, levelZone.container);
            newButton.GetComponent<LevelButton>().InitializeButton(sceneId, levelZone);
        }
    }
}
