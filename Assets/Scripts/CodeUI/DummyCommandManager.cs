using System;
using UnityEngine;

namespace CodeUI
{
    public class DummyCommandManager : ICommandManager
    {
        public void Conjure(EntityType type)
        {
            Debug.Log($"{Enum.GetName(type.GetType(), type)} Conjured!");
        }

        public void Change(ChangeType type)
        {
            Debug.Log($"{Enum.GetName(type.GetType(), type)} Changed!");
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

        public bool IsChangeable(EntityType type)
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
    }
}