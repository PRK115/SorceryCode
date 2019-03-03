using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RuneStockDictionary: SerializableDictionary<RuneType, int> { }

public class RuneStock : MonoBehaviour
{
    public static RuneStock Inst;

    [SerializeField]
    public RuneStockDictionary runeStockDictionary;

    private void Awake()
    {
        Inst = this;

        //runeStockDictionary.CopyFrom(RuneCount);

        runeStockDictionary.Add(new RuneType(EntityType.CastleBlock), 0);
        runeStockDictionary.Add(new RuneType(EntityType.WoodBlock), 0);
        runeStockDictionary.Add(new RuneType(EntityType.MetalBlock), 0);
        runeStockDictionary.Add(new RuneType(EntityType.Switch), 0);
        runeStockDictionary.Add(new RuneType(EntityType.SlidingDoor), 0);
        runeStockDictionary.Add(new RuneType(EntityType.WoodBox), 0);
        runeStockDictionary.Add(new RuneType(EntityType.IronBox), 0);
        runeStockDictionary.Add(new RuneType(EntityType.FireBall), 0);
        runeStockDictionary.Add(new RuneType(EntityType.LightningBall), 0);
        runeStockDictionary.Add(new RuneType(EntityType.Witch), 0);
        runeStockDictionary.Add(new RuneType(EntityType.Lion), 0);
        runeStockDictionary.Add(new RuneType(EntityType.Mouse), 0);
        runeStockDictionary.Add(new RuneType(ChangeType.Big), 0);
        runeStockDictionary.Add(new RuneType(ChangeType.Small), 0);
        runeStockDictionary.Add(new RuneType(RuneType.Direction.Up), 0);
        runeStockDictionary.Add(new RuneType(RuneType.Direction.Down), 0);
        runeStockDictionary.Add(new RuneType(RuneType.Direction.Left), 0);
        runeStockDictionary.Add(new RuneType(RuneType.Direction.Right), 0);
    }


    public Dictionary<RuneType, int> RuneCount = new Dictionary<RuneType, int>()
    {
        {new RuneType(EntityType.CastleBlock), 0},
        {new RuneType(EntityType.WoodBlock), 0},
        {new RuneType(EntityType.MetalBlock), 0},
        {new RuneType(EntityType.Switch), 0},
        {new RuneType(EntityType.WoodBox), 0},
        {new RuneType(EntityType.IronBox), 0},
        {new RuneType(EntityType.FireBall), 0},
        {new RuneType(EntityType.LightningBall), 0},
        {new RuneType(EntityType.Witch), 0 },
        {new RuneType(EntityType.Lion), 0},
        {new RuneType(EntityType.Mouse), 0},
        {new RuneType(ChangeType.Big), 0},
        {new RuneType(ChangeType.Small), 0},
        {new RuneType(RuneType.Direction.Up), 0},
        {new RuneType(RuneType.Direction.Down), 0},
        {new RuneType(RuneType.Direction.Left), 0},
        {new RuneType(RuneType.Direction.Right), 0},
    };

    public event Action<RuneType, int, bool> OnRuneUpdate;

    public void AddRune(RuneType type)
    {
        runeStockDictionary[type]++;
        //Debug.Log($"rune count: {runeStockDictionary[type]}");
        OnRuneUpdate(type, runeStockDictionary[type], true);
    }
    public void DeductRune(RuneType type)
    {
        runeStockDictionary[type]--;
        //Debug.Log($"rune count: {runeStockDictionary[type]}");
        OnRuneUpdate(type, runeStockDictionary[type], false);
    }

    public void ReturnRune(RuneType runeType)
    {
        StartCoroutine(DelayedReturnRune(runeType));
    }

    public IEnumerator DelayedReturnRune(RuneType runeType)
    {
        yield return new WaitForUpdate();
        if(RuneStock.Inst != null)
            AddRune(runeType);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
