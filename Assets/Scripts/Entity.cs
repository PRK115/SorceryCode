﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Direction { None, Left, Right, Up, Down, LeftUp, RightUp }

public enum EntityType
{
    CastleBlock = 0,
    MetalBlock,
    WoodBlock,
    WoodBox = 8,
    IronBox,
    Rune = 16,
    Goal = 32,
    Switch,
    Key,
    LockedDoor = 40,
    RotatingDoor,
    SlidingDoor,
    Lion = 64,
    Mouse,
    FireBall = 96,
    LightningBall
}

public class Entity : MonoBehaviour
{
    public EntityType EntityType;
    public bool isMovable;
    public bool IsButtonPushable;
    public bool blockProjectiles;
    public bool occupySpace;
}
