using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStock : MonoBehaviour {

    public Dictionary<EntityType, int> EntityRuneStock = new Dictionary<EntityType, int>();
    public Dictionary<ChangeType, int> ChangetypeRuneStock = new Dictionary<ChangeType, int>();
    public Dictionary<RuneType.Direction, int> DirectionRuneStock = new Dictionary<RuneType.Direction, int>();

    public void AddRune(EntityType entity)
    {
        EntityRuneStock[entity]++;
    }
    public void AddRune(ChangeType adjective)
    {
        ChangetypeRuneStock[adjective]++;
    }

    public void AddRune(RuneType.Direction direction)
    {
        DirectionRuneStock[direction]++;
    }

    void DeductRune(EntityType entity)
    {
        EntityRuneStock[entity]--;
    }
    void DeductRune(ChangeType adjective)
    {
        ChangetypeRuneStock[adjective]--;
    }

    void DeductRune(RuneType.Direction direction)
    {
        DirectionRuneStock[direction]--;
    }
}
