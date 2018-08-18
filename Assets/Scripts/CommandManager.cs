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

    float delay = 1;

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
            StartCoroutine(GradualConjure(prefab));
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }

    IEnumerator GradualConjure(GameObject prefab)
    {
        GameObject conjured;
        conjured = Instantiate(prefab, SpawnPos, prefab.transform.rotation);
        conjured.transform.localScale = new Vector3(1,1,1) * 0.02f;
        Rigidbody rb;
        rb = conjured.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = false;
        }
        for(int i = 1; i < 50; i++)
        {
            conjured.transform.localScale = new Vector3(1, 1, 1) * 0.02f * i;
            yield return new WaitForSeconds(0.02f);
        }
        target = conjured.GetComponent<Entity>();
        if (rb != null)
        {
            rb.useGravity = true;
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

    IEnumerator GradualGrowth()
    {
        for(int i = 1; i <= 10; i++)
        {
            target.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.1f);
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