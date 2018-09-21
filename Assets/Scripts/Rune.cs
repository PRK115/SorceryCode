using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Consumable))]
public class Rune : MonoBehaviour, IConsumable {

    private GameObject stone;

    [SerializeField] private RuneType runeType;

    float timeTillDestroy = 0.5f;

    AudioSource sound;

    private void Awake()
    {
        stone = transform.Find("stone").gameObject;
        //sound = gameObject.AddComponent<AudioSource>();
        //sound.playOnAwake = false;
        //sound.clip = FindObjectOfType<PlayerCtrl>().runeGot;
    }

    public void ConsumedBehaviour()
    {
        stone.SetActive(false);
        GetComponent<Collider>().enabled = false;
        switch (runeType.type)
        {
            case RuneType.Type.Entity:
                RuneStock.Inst.AddRune(new RuneType(runeType.Entity));
                break;
            case RuneType.Type.Adjective:
                RuneStock.Inst.AddRune(new RuneType(runeType.adjective));
                break;
            case RuneType.Type.Direction:
                RuneStock.Inst.AddRune(new RuneType(runeType.direction));
                break;
        }

        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        //if (timeTillDestroy <= 0)
        //{
        //    Destroy(gameObject);
        //    yield break;
        //}
        //timeTillDestroy -= Time.deltaTime;
        //yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(timeTillDestroy);
        Destroy(gameObject);
        yield break;
    }
}
