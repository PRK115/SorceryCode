﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Changeable : MonoBehaviour, Attribute
{
    enum Results
    {
        Big, Small,
        Mouse, Lion,
        WoodBox, IronBox
    }

    List<Results> PossibleResults;
}