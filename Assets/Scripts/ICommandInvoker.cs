using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface ICommandInvoker
{
    void Conjure(EntityType type);
    void Change(EntityType type);

    bool IsConjurable(EntityType type);
    bool IsChangable(EntityType type);
}