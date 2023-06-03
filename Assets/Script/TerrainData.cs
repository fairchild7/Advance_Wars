using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Grass,
    Road,
    Tree,
    River,
    Mountain,
    Building
}

[CreateAssetMenu]
public class TerrainData : ScriptableObject
{
    public List<TerrainType> terrains;
} 