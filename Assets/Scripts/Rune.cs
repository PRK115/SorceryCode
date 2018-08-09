using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Consumable))]
public class Rune : MonoBehaviour, IConsumable {

    private GameObject stone;

    public enum RuneType { Entity, Adjective, Direction}
    public RuneType runeType;

    public EntityType entity;
    public enum Adjective { Big, Small}
    public Adjective adjective;
    public enum Direction { Up, Down, Left, Right}
    public Direction direction;

    float timeTillDestroy = 0.5f;

    private void Awake()
    {
        stone = transform.Find("stone").gameObject;
    }

    public void ConsumedBehaviour()
    {
        stone.SetActive(false);
        if (timeTillDestroy <= 0)
        {
            switch (runeType)
            {
                case RuneType.Entity:
                    AddRune(entity);
                    break;
                case RuneType.Adjective:
                    AddRune(adjective);
                    break;
                case RuneType.Direction:
                    AddRune(direction);
                    break;
            }
            Destroy(gameObject);
        }
        else timeTillDestroy -= Time.deltaTime;
    }

    void AddRune(EntityType entity)
    {

    }

    void AddRune(Adjective adjective)
    {

    }

    void AddRune(Direction direction)
    {

    }
}
