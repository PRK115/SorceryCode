using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Inst;

    public Vector3 SpawnPos;

    public Dictionary<EntityType, GameObject> prefabs;

    void Awake()
    {
        Inst = this;
    }

    public void Conjure(EntityType type)
    {
        GameObject prefab = prefabs[type];
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        if (conjurable != null)
        {
            // TODO
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }

    public void Enchant(EntityType type)
    {

    }

    public void Change(EntityType type)
    {

    }
}