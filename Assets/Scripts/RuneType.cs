using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RuneType
{
    public enum Type { Entity, Adjective, Direction }
    public Type type;

    [SerializeField]
    private EntityType entity;
    public EntityType Entity
    {
        get
        {
            if (type != Type.Entity)
            {
                throw new Exception($"Tried to get invalid field in RuneType (Type {type})");
            }
            return entity;
        }
    }
    public ChangeType adjective;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;

    public RuneType(EntityType entity)
    {
        this.type = Type.Entity;

        this.entity = entity;
        this.adjective = ChangeType.Big;
        this.direction = Direction.Up;
    }
    public RuneType(ChangeType adjective)
    {
        this.type = Type.Adjective;

        this.entity = EntityType.CastleBlock;
        this.adjective = adjective;
        this.direction = Direction.Up;
    }
    public RuneType(Direction direction)
    {
        this.type = Type.Direction;

        this.entity = EntityType.CastleBlock;
        this.adjective = ChangeType.Big;
        this.direction = direction;
    }
}
