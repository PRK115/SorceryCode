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

    public void Conjure(EntityType type, Vector3 position)
    {
        GameObject prefab = prefabs[type];
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        if (conjurable != null)
        {
            Instantiate(prefab, position, prefabs[type].transform.rotation);
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }

    public void Enchant(EntityType type)
    {
        
    }

    public void Change(GameObject target, EntityType type)
    {
        Changeable changable = target.GetComponent<Changeable>();
        GameObject result = prefabs[type];

        if (changable != null)
        {

        }
        else
        {
            Debug.LogError("it's not changeable");
        }
    }

    public void Change(GameObject target, Rune.Adjective adjective)
    {
        Changeable changable = target.GetComponent<Changeable>();
        if (changable != null)
        {
            if (changable.Resizable)
            {
                switch (adjective)
                {
                    case Rune.Adjective.Big:
                        target.transform.localScale *= 3;
                        break;
                    case Rune.Adjective.Small:
                        target.transform.localScale /= 3;
                        break;
                }
            }
            else
            {
                Debug.LogError("Can't change its size");
            }
        }
        else
        {
            Debug.LogError("it's not changeable");
        }
       
    }
}