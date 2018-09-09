using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Consumable))]
public class Rune : MonoBehaviour, IConsumable {

    private GameObject stone;

    RuneStock stock;

    [SerializeField] private RuneType runeType;

    float timeTillDestroy = 0.5f;

    AudioSource sound;

    private void Awake()
    {
        stock = FindObjectOfType<RuneStock>();
        stone = transform.Find("stone").gameObject;
        //sound = gameObject.AddComponent<AudioSource>();
        //sound.playOnAwake = false;
        //sound.clip = FindObjectOfType<PlayerCtrl>().runeGot;
    }

    public void ConsumedBehaviour()
    {
        stone.SetActive(false);
        //sound.Play();
        if (timeTillDestroy <= 0 && stock != null)
        {
            switch (runeType.type)
            {
                case RuneType.Type.Entity:
                    stock.AddRune(runeType.Entity);
                    break;
                case RuneType.Type.Adjective:
                    stock.AddRune(runeType.adjective);
                    break;
                case RuneType.Type.Direction:
                    stock.AddRune(runeType.direction);
                    break;
            }
            Destroy(gameObject);
        }
        else timeTillDestroy -= Time.deltaTime;
    }
}
