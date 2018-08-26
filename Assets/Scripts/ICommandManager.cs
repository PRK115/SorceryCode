using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum MoveDirection
{
    Left, Right, Up, Down
}

public interface ICommandManager
{
    void Conjure(Vector3 location, EntityType type);
    void Change(Entity target, ChangeType type);
    void Change(Entity target, EntityType type);
    void Move(Entity target, MoveDirection direction, int distance);

    bool IsConjurable(EntityType type);
    bool IsSizeChangeable(EntityType type);
    bool IsChangeable(EntityType from, EntityType to);
    bool IsMoveable(EntityType type);
}