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
    void Conjure(EntityType type);
    void Change(ChangeType type);
    void Change(EntityType type);
    void Move(MoveDirection direction, int distance);

    bool IsConjurable(EntityType type);
    bool IsSizeChangeable(EntityType type);
    bool IsChangeable(EntityType from, EntityType to);
    bool IsMoveable(EntityType type);
}