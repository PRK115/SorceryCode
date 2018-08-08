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
        // TODO: 인터프레터 테스트를 위해 임시로 이렇게 구현해놓았음
        Debug.Log($"{type.GetType().Name} Conjured!");
        /*
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
        */
    }

    public void Enchant(EntityType type)
    {

    }

    public void Change(EntityType type)
    {

    }

    // TODO: 인터프레터 테스트를 위해 임시로 이렇게 구현해놓았음
    public bool IsConjurable(EntityType type)
    {
        /*
        GameObject prefab = prefabs[type];
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        return conjurable != null;
        */
        switch (type)
        {
            case EntityType.Lion:
            case EntityType.Mouse:
                return true;
        }

        return false;
    }
}