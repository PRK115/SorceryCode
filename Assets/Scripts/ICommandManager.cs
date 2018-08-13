using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface ICommandManager
{
    void Conjure(EntityType type);
    void Change(ChangeType type);

    bool IsConjurable(EntityType type);
    bool IsChangeable(EntityType type);
}