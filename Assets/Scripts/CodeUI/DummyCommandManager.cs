using System;
using UnityEngine;

namespace CodeUI
{
    public class DummyCommandManager : ICommandManager
    {
        public static DummyCommandManager Inst = new DummyCommandManager();

        public void Conjure(EntityType type)
        {
            Debug.Log($"{type.ToString()} Conjured!");
        }

        public void Change(ChangeType type)
        {
            Debug.Log($"Changed to {type.ToString()}!");
        }

        public void Change(EntityType entity)
        {
            Debug.Log($"Changed to {entity.ToString()}!");
        }

        public void Move(MoveDirection direction, int distance)
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
    }
}