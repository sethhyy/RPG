using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string PlayerName;
    public string Gender;
    public string CharacterGender;

    [Header("Stats")]
    public int MaxHearts = 5;
    public int CurrentHearts;
    public int level = 1;
    public int gold = 0;

    [Header("Visuals")]
    public Color ClothingColor;

    [Header("Persistence")]
    public List<ScenePosition> LastScenePositions = new List<ScenePosition>();
    public Vector3 LastOutsidePosition;
    public bool HasOutsidePosition;

    public PlayerData()
    {
        CurrentHearts = MaxHearts;
        level = 1;
        gold = 0;
    }

    public void SetScenePosition(string scene, Vector3 pos)
    {
        foreach (var sp in LastScenePositions)
        {
            if (sp.sceneName == scene) { sp.position = pos; return; }
        }
        LastScenePositions.Add(new ScenePosition { sceneName = scene, position = pos });
    }

    public bool TryGetScenePosition(string scene, out Vector3 pos)
    {
        foreach (var sp in LastScenePositions)
        {
            if (sp.sceneName == scene) { pos = sp.position; return true; }
        }
        pos = Vector3.zero;
        return false;
    }
}