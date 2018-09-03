using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfDetector : MonoBehaviour{

    EntityType myType;
    
    bool touchingSwitch;

    private void Awake()
    {
        myType = GetComponent<Entity>().EntityType;
    }

    public bool Detect(EntityType target)
    {
        
        //스위치 탐지
        if (target == EntityType.Switch)
        {
            return touchingSwitch;
        }

        //에너지 탐지
        else if (target == EntityType.FireBall)
        {
            //Organism, MoveableObject, Energy
            List<Collider> colliders = CheckSurroundings(13 << 9);

            for (int i = 0; i < colliders.Count; i++)
            {
                Flammable flammable = colliders[i].GetComponent<Flammable>();
                if (flammable != null)
                {
                    if(flammable.state == Flammable.State.Burning)
                    {
                        return true;
                    }
                }
            }
        }
        else if (target == EntityType.LightningBall)
        {
            //Organism, MoveableObject, Energy
            List<Collider> colliders = CheckSurroundings(13 << 9);

            for (int i = 0; i < colliders.Count; i++)
            {
                Conductor conductors = colliders[i].GetComponent<Conductor>();
                if (conductors != null)
                {
                    if (conductors.state == Conductor.State.Electrified)
                    {
                        return true;
                    }
                }
            }
        }

        //나머지
        else
        {
            List<Collider> colliders = CheckSurroundings(~(1<<12));
            for (int i = 0; i < colliders.Count; i++)
            {
                Entity entity = colliders[i].GetComponent<Entity>();
                if (entity != null)
                {
                    if (entity.EntityType == target)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private List<Collider> CheckSurroundings(int layerMask)
    {
        List<Collider> ColliderList = new List<Collider>();

        Collider[] Colliders = Physics.OverlapBox(transform.position + Vector3.up * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        for(int i = 0; i< Colliders.Length; i++)
        {
            ColliderList.Add(Colliders[i]);
        }
        Colliders = Physics.OverlapBox(transform.position + Vector3.down * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        for (int i = 0; i < Colliders.Length; i++)
        {
            ColliderList.Add(Colliders[i]);
        }
        Colliders = Physics.OverlapBox(transform.position + Vector3.left * 0.8f, new Vector3(0.1f, 0.3f, 0.5f), Quaternion.identity, layerMask);
        for (int i = 0; i < Colliders.Length; i++)
        {
            ColliderList.Add(Colliders[i]);
        }
        Colliders = Physics.OverlapBox(transform.position + Vector3.right * 0.8f, new Vector3(0.1f, 0.3f, 0.5f), Quaternion.identity, layerMask);
        for (int i = 0; i < Colliders.Length; i++)
        {
            ColliderList.Add(Colliders[i]);
        }
        return ColliderList;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.EntityType == EntityType.Switch)
        {
            touchingSwitch = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.EntityType == EntityType.Switch)
        {
            touchingSwitch = false;
        }
    }
}
