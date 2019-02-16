using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfDetector : MonoBehaviour{

    EntityType myType;

    new Collider collider;
    Vector3 colliderCenter;
    Vector3 realCenter => colliderCenter + transform.position;
    Vector3 boxSize;
    Vector3 realBoxSize => boxSize * transform.localScale.x;
    float capsuleHeight;
    float realCapsuleHeight => capsuleHeight * transform.localScale.y;
    float capsuleRadius;
    float realCapsuleRadius => capsuleRadius * transform.localScale.x;
    System.Type colliderType;

    private void Awake()
    {
        colliderType = typeof (int);
        myType = GetComponent<Entity>().EntityType;
        collider  = GetComponent<Collider>();
        colliderType = collider.GetType();
        if(colliderType == typeof(BoxCollider))
        {
            var c = (BoxCollider) collider;
            boxSize = c.size;
            colliderCenter = c.center;
        }
        else if(colliderType == typeof(CharacterController))
        {
            var c = (CharacterController)collider;
            capsuleHeight = c.height;
            capsuleRadius = c.radius;
            colliderCenter = c.center;
        }
        CheckSurroundings(~0);
    }

    public bool Detect(EntityType target)
    {
        //스위치 탐지
        if (target == EntityType.Switch)
        {
            if (colliderType == typeof(BoxCollider))
                return Physics.CheckBox(realCenter, realBoxSize/2, Quaternion.identity, 1024);
            else if (colliderType == typeof(CharacterController))
                return Physics.CheckCapsule(realCenter + Vector3.up * realCapsuleHeight / 2, realCenter + Vector3.down * realCapsuleHeight / 2, realCapsuleRadius, 1024);
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
        Collider[] Colliders;

        if (colliderType == typeof(BoxCollider))
        {
            var halfWidth = realBoxSize.x / 2;
            var halfHeight = realBoxSize.y / 2;
            Colliders = Physics.OverlapBox(realCenter + Vector3.up * (halfHeight + 0.3f), new Vector3(halfWidth, 0.1f, 0.5f), Quaternion.identity, layerMask);
            for (int i = 0; i < Colliders.Length; i++)
            {
                ColliderList.Add(Colliders[i]);
            }
            Colliders = Physics.OverlapBox(realCenter + Vector3.down * (halfHeight + 0.3f), new Vector3(halfWidth, 0.1f, 0.5f), Quaternion.identity, layerMask);
            for (int i = 0; i < Colliders.Length; i++)
            {
                ColliderList.Add(Colliders[i]);
            }
            Colliders = Physics.OverlapBox(realCenter + Vector3.left * (halfWidth + 0.3f), new Vector3(0.1f, halfHeight, 0.5f), Quaternion.identity, layerMask);
            for (int i = 0; i < Colliders.Length; i++)
            {
                ColliderList.Add(Colliders[i]);
            }
            Colliders = Physics.OverlapBox(realCenter + Vector3.right * (halfWidth + 0.3f), new Vector3(0.1f, halfHeight, 0.5f), Quaternion.identity, layerMask);
            for (int i = 0; i < Colliders.Length; i++)
            {
                ColliderList.Add(Colliders[i]);
            }
        }
        else if (colliderType == typeof(CharacterController))
        {
            Colliders = Physics.OverlapCapsule(realCenter + Vector3.up * capsuleHeight / 2, realCenter - Vector3.up * capsuleHeight / 2, realCapsuleRadius * 1.2f);

            ColliderList.AddRange(Colliders);
            ColliderList.Remove(collider);
        }
        return ColliderList;
    }
}
