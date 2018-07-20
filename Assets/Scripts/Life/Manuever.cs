using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManueverType : MonoBehaviour {
    public abstract void Manuever(Control.Direction direction);
}
