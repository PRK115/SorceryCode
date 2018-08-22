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
            GameObject conjured;
            conjured = Instantiate(prefab, SpawnPos, prefab.transform.rotation);
            conjured.transform.localScale = new Vector3(1,1,1) * 0.02f;
            Rigidbody rb;
            rb = conjured.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.useGravity = false;
            }
            target = conjured.GetComponent<Entity>();

            StartCoroutine(
                StartGradualAction(
                    timer =>
                    {
                        conjured.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer);
                    }, () =>
                    {
                        conjured.transform.localScale = new Vector3(1, 1, 1);
                        if (rb != null) rb.useGravity = true;
                    },
                    1.0f
                )
            );
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }


    public void Change(ChangeType type)
    {
        Changeable changeable = target.GetComponent<Changeable>();
        if (changeable == null)
        {
            Debug.LogError($"Cannot change entity {target} to ChangeType {type}");
            return;
        }
        if (!changeable.Resizable)
        {
            Debug.LogError("Can't change its size");
            return;
        }

        Vector3 finalSize;
        switch (type)
        {
            case ChangeType.Big:
                finalSize = target.transform.localScale * 3;
                break;
            case ChangeType.Small:
                finalSize = target.transform.localScale / 3;
                break;
            default:
                Debug.LogError($"Unsupported ChangeType {type}");
                return;
        }

        StartCoroutine(
            StartGradualAction(timer =>
                {
                    target.transform.localScale = Vector3.Lerp(
                        target.transform.localScale, finalSize, timer);
                }, () =>
                {
                    target.transform.localScale = finalSize;
                },
                1.0f
            )
        );
    }

    public void Move(MoveDirection direction, int distance)
    {
        if (distance <= 0 || distance > 4)
        {
            Debug.LogError("Can only move object with distance between 1 and 3");
            return;
        }

        Moveable moveable = target.GetComponent<Moveable>();
        if (moveable == null)
        {
            Debug.LogError($"Target is not moveable");
            return;
        }

        Vector3 finalPos = target.transform.position;
        switch (direction)
        {
            case MoveDirection.Left:
                finalPos.x -= 1.0f;
                break;
            case MoveDirection.Right:
                finalPos.x += 1.0f;
                break;
            case MoveDirection.Up:
                finalPos.y += 1.0f;
                break;
            case MoveDirection.Down:
                finalPos.y -= 1.0f;
                break;
        }

        StartCoroutine(
            StartGradualAction(timer =>
                {
                    target.transform.position = Vector3.Lerp(
                        target.transform.position, finalPos, timer);
                }, () =>
                {
                    target.transform.position = finalPos;
                },
                1.0f
            )
        );
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

    public bool IsMoveable(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Moveable moveable = prefab.GetComponent<Moveable>();
        return moveable != null;
    }

    private delegate void GradualAction(float timer);
    private IEnumerator StartGradualAction(GradualAction action, Action onFinished, float time)
    {
        int nounce = Interpreter.Inst.Nounce;
        float timer = 0.0f;
        while (timer < time)
        {
            action(timer);
            if (Interpreter.Inst.Nounce == nounce)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            else
            {
                break;
            }
        }
        onFinished();
    }
}