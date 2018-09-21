using System;
using UnityEngine;

namespace CodeUI
{
    public class DummyCommandManager : ICommandManager
    {
        public static DummyCommandManager Inst = new DummyCommandManager();

        public void Conjure(EvalContext context, EntityType type)
        {
            Debug.Log($"{type.ToString()} Conjured!");
        }

        public void Change(EvalContext context, ChangeType type)
        {
            Debug.Log($"Changed to {type.ToString()}!");
        }

        public void Change(EvalContext context, EntityType entity)
        {
            Debug.Log($"Changed to {entity.ToString()}!");
        }

        public void Move(EvalContext context, RuneType.Direction direction, int distance)
        {
            Debug.Log($"Moved to {direction.ToString()} by distance {distance}");
        }

        public bool IsConjurable(EntityType type)
        {
            switch (type)
            {
                case EntityType.Lion:
                case EntityType.Mouse:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsSizeChangeable(EntityType type)
        {
            switch (type)
            {
                case EntityType.WoodBox:
                case EntityType.IronBox:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsChangeable(EntityType from, EntityType to)
        {
            return true;
        }

        public bool IsMoveable(EntityType type)
        {
            return true;
        }

        public bool Sense(EvalContext context, EntityType entityToBeDetected)
        {
            return true;
        }
    }
}