using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeUI;

public class Projectile : MonoBehaviour {

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
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            Pop();
            Execute();
        }
		if(going)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            if(destroyTime <= 0)
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

        //cue.RunProgram();
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
