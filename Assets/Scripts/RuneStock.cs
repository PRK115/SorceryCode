using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStock : MonoBehaviour {

    public Dictionary<EntityType, int> EntityRuneStock = new Dictionary<EntityType, int>
    {
        {EntityType.CastleBlock, 0 },
        {EntityType.WoodBlock, 0 },
        {EntityType.MetalBlock, 0 },

        {EntityType.Switch , 0},

        {EntityType.WoodBox, 0 },
        {EntityType.IronBox, 0 },

        {EntityType.FireBall, 0 },
        {EntityType.LightningBall, 0 },

        {EntityType.Lion, 0 },
        {EntityType.Mouse, 0 }
    };

    public Dictionary<ChangeType, int> ChangetypeRuneStock = new Dictionary<ChangeType, int>
    {
        {ChangeType.Big, 0 },
        {ChangeType.Small, 0 }
    };

    public Dictionary<RuneType.Direction, int> DirectionRuneStock = new Dictionary<RuneType.Direction, int>
    {
        {RuneType.Direction.Up , 0 },
        {RuneType.Direction.Down, 0 },
        {RuneType.Direction.Left, 0 },
        {RuneType.Direction.Right, 0 }
    };

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
