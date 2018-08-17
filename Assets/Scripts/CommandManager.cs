using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CommandManager : MonoBehaviour, ICommandManager
{
    public static CommandManager Inst;

    public Vector3 SpawnPos;

    private PrefabDB prefabDB;

    private Entity target;

    void Awake()
    {
        Inst = this;
        prefabDB = GetComponent<PrefabDB>();
    }

    // 현재 주문이 걸린 Entity를 이 함수를 통해 세팅 가능.
    public void SetFocusedEntity(Entity focusedEntity)
    {
        this.target = focusedEntity;
    }

    public void Conjure(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        if (conjurable != null)
        {
            // TODO(경록): 여기를 구현하면 됨.
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }

    IEnumerator GradualConjure(GameObject prefab)
    {
        Instantiate(prefab, SpawnPos, prefab.transform.rotation);
        for(int i = 0; i < 100; i++)
        {

        }

        yield return null;
    }

    public void Change(ChangeType type)
    {
        Changeable changeable = target.GetComponent<Changeable>();
        if (changeable != null)
        {
            // TODO(경록): 여기를 구현하면 됨.
            if (changeable.Resizable)
            {
                switch (type)
                {
                    case ChangeType.Big:
                        target.transform.localScale *= 3;
                        break;
                    case ChangeType.Small:
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
            Debug.LogError($"Cannot change entity {target} to ChangeType {type}");
        }
    }

    public bool IsConjurable(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        return conjurable != null;
    }

    public bool IsChangeable(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Changeable changeable = prefab.GetComponent<Changeable>();
        return changeable != null;
    }
}