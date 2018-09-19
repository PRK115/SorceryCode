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
        //Entity target = context.Target;
        
        GameObject prefab = prefabDB.GetPrefab(type);
        Conjurable conjurable = prefab.GetComponent<Conjurable>();
        if (conjurable != null)
        {
            if (prefab.GetComponent<Entity>().occupySpace)
            {
                //Debug.Log("겹치는 거 소환");
                Collider[] hits = Physics.OverlapBox(context.Location, Vector3.one * 0.4f, Quaternion.identity, 2105);
                for (int i = 0; i < hits.Length; i++)
                {
                    //Debug.Log("체크");
                    Entity e = hits[i].GetComponent<Entity>();
                    if (e != null && e.occupySpace)
                    {
                        //Debug.Log("겹침");
                        return;
                    }
                }
            }

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
                //Rigidbody rb = conjured.GetComponent<Rigidbody>();
                Moveable moveable = conjured.GetComponent<Moveable>();
                if (moveable != null)
                {
                    moveable.Ungravitate();
                }

                StartCoroutine(
                    StartGradualAction(
                        timer =>
                        {
                            conjured.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer);
                            //Debug.Log(moveable);
                        }, () =>
                        {
                            conjured.transform.localScale = new Vector3(1, 1, 1);
                            if (moveable != null) moveable.Gravitate();
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
            //Debug.LogError($"Cannot change entity {target} to ChangeType {type}");
            return;
        }
        if (!changeable.Resizable)
        {
            Debug.LogError("Can't change its size");
            return;
        }

        //Rigidbody rb = target.GetComponent<Rigidbody>();
        Moveable moveable = target.GetComponent<Moveable>();
        ContactDetector cd = target.GetComponent<ContactDetector>();

        Vector3 finalSize = Vector3.one;
        switch (type)
        {
            case ChangeType.Big:
                if (!changeable.big)
                {
                    cd.CheckSurroundingObstacles();
                    if (changeable.IsConfined()) return;
                    moveable.Ungravitate();
                    finalSize = target.transform.localScale * 3;
                    changeable.changing = true;
                    changeable.big = true;
                    changeable.SwellSound();
                }
                else
                {
                    return;
                }
                break;
            case ChangeType.Small:
                if (changeable.big)
                {
                    moveable.Ungravitate();
                    finalSize = target.transform.localScale / 3;
                    changeable.changing = true;
                    changeable.big = false;
                    changeable.ShrinkSound();
                }
                else
                {
                    return;
                }
                break;
            default:
                Debug.LogError($"Unsupported ChangeType {type}");
                return;
        }

        Vector3 originalPosition = target.transform.position;
        Vector3 finalPosition = (type == ChangeType.Big) ? changeable.BePushed(originalPosition) : originalPosition;

        StartCoroutine(
        StartGradualAction(timer =>
        {
            target.transform.position = Vector3.Lerp(
                  originalPosition, finalPosition, timer
                  );

            target.transform.localScale = Vector3.Lerp(
                    target.transform.localScale, finalSize, timer);

                //Debug.Log($"({Mathf.Round(10 * (-5 - target.transform.position.x)/timer)/10}, { (target.transform.position.y + 2) / timer} kinematic:{target.GetComponent<Rigidbody>().isKinematic} { moveable.YTendency })");
            }, () =>
            {
                target.transform.localScale = finalSize;
                changeable.changing = false;
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

        //Rigidbody rb = target.GetComponent<Rigidbody>();

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

        moveable.Ungravitate();

        StartCoroutine
        (
            StartGradualAction
            (
                timer => { }
                , () =>
                {
                    target.transform.position = new Vector3(Mathf.Round(target.transform.position.x), Mathf.Round(target.transform.position.y), 0);
                    moveable.XTendency = moveable.YTendency = 0;
                    moveable.Gravitate();
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

    public bool Sense(EvalContext context, EntityType entityToBeDetected)
    {
        IfDetector detector = context.Target.GetComponent<IfDetector>();
        if(detector == null)
            detector = context.Target.gameObject.AddComponent<IfDetector>();
        return detector.Detect(entityToBeDetected);
    }
}