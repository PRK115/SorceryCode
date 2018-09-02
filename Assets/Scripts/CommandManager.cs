using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeUI;
using UnityEngine;

public class CommandManager : MonoBehaviour, ICommandManager
{
    public static CommandManager Inst;

    private PrefabDB prefabDB;

    private StmtListBlock program;
    [SerializeField] private CodeUIElement codeUIElement;

    float delay = 1;

    void Awake()
    {
        Inst = this;
        prefabDB = GetComponent<PrefabDB>();
        program = codeUIElement.Program;
    }

    public void ExecuteCode(Entity target, Vector3 location)
    {
        var code = Compiler.Compile(program);
        Interpreter.Inst.Execute(new EvalContext{Target = target, Location = location}, code);
    }

    public void Conjure(EvalContext context, EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        if (conjurable != null)
        {
            GameObject conjured = Instantiate(prefab, context.Location, prefab.transform.rotation);
            Entity conjuredEntity = conjured.GetComponent<Entity>();
            if (conjuredEntity == null)
            {
                Debug.LogError("Conjured entity does not have Entity component!");
                return;
            }
            context.Target = conjuredEntity;

            if(type == EntityType.FireBall || type == EntityType.LightningBall)
            {
            }
            else
            {
                conjured.transform.localScale = new Vector3(1, 1, 1) * 0.02f;
                Rigidbody rb = conjured.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                StartCoroutine(
                    StartGradualAction(
                        timer =>
                        {
                            conjured.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer);
                        }, () =>
                        {
                            conjured.transform.localScale = new Vector3(1, 1, 1);
                            if (rb != null) rb.isKinematic = false;
                        },
                        1.0f
                    )
                );
            }
        }
        else
        {
            Debug.LogError($"Cannot conjure entity {type}");
        }
    }


    public void Change(EvalContext context, ChangeType type)
    {
        Entity target = context.Target;
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

        Rigidbody rb = target.GetComponent<Rigidbody>();

        Vector3 finalSize;
        switch (type)
        {
            case ChangeType.Big:
                if (!changeable.big)
                {
                    target.GetComponent<ContactDetector>().CheckSurroundingObstacles();
                    rb.isKinematic = true;
                    changeable.BePushed();
                    finalSize = target.transform.localScale * 3;
                    changeable.big = true;
                }
                else
                {
                    finalSize = target.transform.localScale;
                }
                break;
            case ChangeType.Small:
                if (changeable.big)
                {
                    finalSize = target.transform.localScale / 3;
                    rb.isKinematic = false;
                    changeable.big = false;
                }
                else
                {
                    finalSize = target.transform.localScale;
                }
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
                    //Debug.Log(rb.isKinematic);
                }, () =>
                {
                    target.transform.localScale = finalSize;
                    changeable.AdjustPosition();
                },
                1.0f
            )
        );
    }

    public void Change(EvalContext context, EntityType entity)
    {
        Entity target = context.Target;
        Changeable changeable = target.GetComponent<Changeable>();
        if (changeable == null)
        {
            Debug.LogError($"Cannot change entity {target} to Entity {entity}");
            return;
        }

        Destroy(target.gameObject);
        GameObject prefab = prefabDB.GetPrefab(entity);
        target = Instantiate(prefab, target.transform.position, prefab.transform.rotation).GetComponent<Entity>();
        context.Target = target; // Assign new target (because object has changed)

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;

        StartCoroutine(
            StartGradualAction(timer =>
            {
                target.transform.localScale = Vector3.Lerp(
                    new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer);
            }, () =>
            {
                target.transform.localScale = new Vector3(1, 1, 1);
                if (rb != null) rb.useGravity = true;
            },
            1.0f)
        );
    }

    public void Move(EvalContext context, MoveDirection direction, int distance)
    {
        if (distance <= 0 || distance > 4)
        {
            Debug.LogError("Can only move object with distance between 1 and 3");
            return;
        }

        Entity target = context.Target;
        Moveable moveable = target.GetComponent<Moveable>();
        if (moveable == null)
        {
            Debug.LogError($"Target is not moveable");
            return;
        }

        Rigidbody rb = target.GetComponent<Rigidbody>();

        rb.isKinematic = true;

        Vector3 finalPos = target.transform.position;
        switch (direction)
        {
            case MoveDirection.Left:
                finalPos.x -= distance;
                moveable.XTendency = -distance;
                break;
            case MoveDirection.Right:
                finalPos.x += distance;
                moveable.XTendency = distance;
                break;
            case MoveDirection.Up:
                finalPos.y += distance;
                moveable.YTendency = distance;
                break;
            case MoveDirection.Down:
                finalPos.y -= distance;
                moveable.YTendency = -distance;
                break;
        }
        StartCoroutine
        (
            StartGradualAction
            (
                timer => { }
                , () =>
                {
                    target.transform.position = new Vector3(Mathf.Round(target.transform.position.x), Mathf.Round(target.transform.position.y), 0);
                    rb.isKinematic = false;
                    moveable.XTendency = moveable.YTendency = 0;
                }
                , 1.0f
            )
        );

        //StartCoroutine(
        //    StartGradualAction(timer =>
        //        {
        //            target.transform.position = Vector3.Lerp(
        //                target.transform.position, finalPos, timer);
        //        }, () =>
        //        {
        //            target.transform.position = finalPos;
        //        },
        //        1.0f
        //    )
        //);
    }

    public bool IsConjurable(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        return conjurable != null;
    }

    public bool IsSizeChangeable(EntityType type)
    {
        GameObject prefab = prefabDB.GetPrefab(type);
        Changeable changeable = prefab.GetComponent<Changeable>();
        return changeable != null && changeable.Resizable;
    }

    public bool IsChangeable(EntityType from, EntityType to)
    {
        return (prefabDB.GetPrefab(from).GetComponent<Changeable>() != null && prefabDB.GetPrefab(to).GetComponent<Changeable>() != null);
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