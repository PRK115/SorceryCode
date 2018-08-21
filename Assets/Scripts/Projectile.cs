using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeUI;

public class Projectile : MonoBehaviour {

    public enum State
    {
        Flying, Explode, Aftermath
    }
    private State state = State.Flying;

    Vector3 destination;
    public Vector3 Destination { set { destination = value; } }
    float speed = 5f;

    GameObject aura;
    GameObject hit;

    bool going = true;
    float destroyTime = 0.5f;

    CommandManager commandManager;
    CodeUIElement cue;

    Entity touchingEntity;

	// Use this for initialization
	void Start () {
        aura = transform.Find("aura").gameObject;
        hit = transform.Find("hit").gameObject;

        commandManager = FindObjectOfType<CommandManager>();
        cue = FindObjectOfType<CodeUIElement>();

        transform.LookAt(destination);
	}
	
	// Update is called once per frame
	void Update () {
	    if (state == State.Flying)
	    {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                state = State.Explode;
            }
	    }
	    else if (state == State.Explode)
	    {
	        Pop();
	        Execute();
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

    void Pop()
    {
        aura.SetActive(false);
        hit.SetActive(true);

        going = false;
    }

    void Execute()
    {
        commandManager.SpawnPos = destination;
        commandManager.SetFocusedEntity(touchingEntity);

        GameStateManager.instance.ExecuteCode();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if(otherEntity != null)
        {
            if (otherEntity.blockProjectiles)
            {
                Pop();
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
