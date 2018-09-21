using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommandManager
{
    void Conjure(EvalContext context, EntityType type);
    void Change(EvalContext context, ChangeType type);
    void Change(EvalContext context, EntityType type);
    void Move(EvalContext context, RuneType.Direction direction, int distance);

    bool IsConjurable(EntityType type);
    bool IsSizeChangeable(EntityType type);
    bool IsChangeable(EntityType from, EntityType to);
    bool IsMoveable(EntityType type);

    bool Sense(EvalContext context, EntityType entityToBeDetected);
}