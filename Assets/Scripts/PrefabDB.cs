using System;
using UnityEngine;

[Serializable]
public class PrefabDictionary : SerializableDictionary<EntityType, GameObject> { }

public class PrefabDB : MonoBehaviour
{
    [SerializeField]
    private PrefabDictionary prefabs;

    public GameObject GetPrefab(EntityType type)
    {
        return prefabs[type];
    }
}