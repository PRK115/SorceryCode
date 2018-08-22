using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Consumable))]
public class Rune : MonoBehaviour, IConsumable {

    private GameObject stone;

    PlayerCtrl player;

    public enum RuneType { Entity, Adjective, Direction}
    public RuneType runeType;

    public EntityType entity;
    public ChangeType adjective;
    public enum Direction { Up, Down, Left, Right}
    public Direction direction;

    float timeTillDestroy = 0.5f;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCtrl>();
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
        player.EntityRunes.Add(entity);
    }

    void AddRune(ChangeType adjective)
    {
        player.AdjRunes.Add(adjective);
    }

    void AddRune(Direction direction)
    {
        player.DirectionRunes.Add(direction);
    }
}
