using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneStock : MonoBehaviour
{
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
        {new RuneType(EntityType.Lion), 0},
        {new RuneType(ChangeType.Big), 0},
        {new RuneType(ChangeType.Small), 0},
        {new RuneType(RuneType.Direction.Up), 0},
        {new RuneType(RuneType.Direction.Down), 0},
        {new RuneType(RuneType.Direction.Left), 0},
        {new RuneType(RuneType.Direction.Right), 0},
    };

    public event Action<RuneType, int> OnRuneUpdate;

    public void AddRune(RuneType type)
    {
        RuneCount[type]++;
        Debug.Log($"rune count: {RuneCount[type]}");
        OnRuneUpdate(type, RuneCount[type]);
    }
    void DeductRune(RuneType type)
    {
        RuneCount[type]--;
        Debug.Log($"rune count: {RuneCount[type]}");
        OnRuneUpdate(type, RuneCount[type]);
    }
}
