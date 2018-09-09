using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeUI;

public class Projectile : MonoBehaviour {

    public enum State
    {
        Flying, Arrival, Explode, Aftermath
    }
    private State state = State.Flying;

    Vector3 destination;
    float travelTime;
    public Vector3 Destination { set { destination = value; } }
    float speed = 7.2f;

    GameObject aura;
    GameObject hit;
    GameObject fail;

    AudioSource sound;

    float destroyTime = 0.5f;

    //CommandManager commandManager;

    Entity touchingEntity;

	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
        aura = transform.Find("aura").gameObject;
        hit = transform.Find("hit").gameObject;
        fail = transform.Find("puff").gameObject;
        travelTime = Vector3.Distance(transform.position, destination) / speed;
        //commandManager = FindObjectOfType<CommandManager>();

        transform.LookAt(destination);
	}
	
	// Update is called once per frame
	void Update () {
	    if (state == State.Flying)
	    {
            if (travelTime <= 0)
            {
                transform.position = destination;
                state = State.Arrival;
            }
            else
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                travelTime -= Time.deltaTime;
            }
        }
	    else if (state == State.Arrival)
	    {
            aura.SetActive(false);
            hit.SetActive(true);
            Execute();
	        state = State.Aftermath;
	    }

        else if (state == State.Explode)
        {
            sound.Play();
            aura.SetActive(false);
            fail.SetActive(true);
            state = State.Aftermath;
        }
        else if (state == State.Aftermath)
	    {
            if (destroyTime <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                destroyTime -= Time.deltaTime;
            }
	    }
	}

    void Execute()
    {
        if (touchingEntity == null)
        {
            CommandManager.Inst.ExecuteCode(null, transform.position);
        }
        else
        {
            CommandManager.Inst.ExecuteCode(touchingEntity, touchingEntity.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        //Debug.Log(otherEntity);
        //Debug.Log(other.name);
        if(otherEntity != null)
        {
            //Debug.Log(otherEntity);
            if (otherEntity.blockProjectiles)
            {
                state = State.Explode;
            }

            else
            {
                touchingEntity = otherEntity;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if (otherEntity != null)
        {
            touchingEntity = null;
        }
    }
}
