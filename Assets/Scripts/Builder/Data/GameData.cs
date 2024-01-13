using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Prefabs")]
    public Box[] prefabBoxes;

    [Header("Levels")]
    public LevelData[] levels;
}
