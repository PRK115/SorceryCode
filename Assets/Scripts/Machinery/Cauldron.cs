using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour {
    [SerializeField]
	List<GameObject> source = new List<GameObject>();
	private GameObject lid;
	private GameObject steam;
	private Moveable lidMoveable;

	private float duration = 0f;

	public float height;


	public bool on;


	private float startY;


	// Use this for initialization
	void Start () {
		lid = transform.parent.transform.Find("cauldron_LID").gameObject;
		steam = transform.parent.transform.Find("steam").gameObject;
		lidMoveable = lid.GetComponent<Moveable>();
		steam.SetActive(false);
		startY = lid.transform.position.y;
	}

	void Update(){
		if(on)
		{

				lidMoveable.gravitated = false;
				steam.SetActive(true);		
				lid.GetComponent<Rigidbody>().isKinematic = true;
				lidMoveable.YTendency = (startY+height-lid.transform.position.y)*height*2.0f;
				duration += Time.deltaTime;
            List<GameObject> imsi = new List<GameObject>();
            imsi.AddRange(source);
                foreach(GameObject fuel in source)
            {
                if (fuel == null || !fuel.activeInHierarchy)
                    imsi.Remove(fuel);
            }
            source.Clear();
            source.AddRange(imsi);
            if (source.Count == 0)
            {
                TurnOff();
            }
        }
		else
		{
			lidMoveable.gravitated = true;
			steam.SetActive(false);
			duration = 0;
		}

	}
	

	void OnTriggerEnter(Collider other)
	{
		GameObject intruder = other.gameObject;
		Flammable flammable = intruder.GetComponent<Flammable>();
		PureEnergy pureEnergy = intruder.GetComponent<PureEnergy>();
		if(flammable!=null&& flammable.state == Flammable.State.Burning)
		{
			source.Add(intruder);
		}
		if(pureEnergy!=null&& pureEnergy.duration>0)
		{
			source.Add(intruder);
		}
		if(source.Count>0)
		{
			TurnOn();
		}

	}

	void OnTriggerStay(Collider other)
	{
		GameObject intruder = other.gameObject;
		Flammable flammable = intruder.GetComponent<Flammable>();
		PureEnergy pureEnergy = intruder.GetComponent<PureEnergy>();
		if(flammable!=null&&flammable.state!=Flammable.State.Burning)
		{
			source.Remove(intruder);
		}
		if(pureEnergy!=null&& pureEnergy.duration<=0)
		{
			source.Remove(intruder);
		}
		if(flammable!=null&& flammable.state == Flammable.State.Burning && (!source.Remove(intruder)))
		{
			source.Add(intruder);
		}
		else if(flammable!=null&& flammable.state == Flammable.State.Burning)
		{
			source.Add(intruder);
		}
		if(source.Count==0)
		{
			TurnOff();
		}
		else
		{
			TurnOn();
		}
	}
	void OnTriggerExit(Collider other)
	{
		GameObject intruder = other.gameObject;
		Flammable flammable = intruder.GetComponent<Flammable>();
		PureEnergy pureEnergy = intruder.GetComponent<PureEnergy>();
		if(flammable!=null&& flammable.state == Flammable.State.Burning)
		{
			source.Remove(intruder);
		}
		if(pureEnergy!=null&& pureEnergy.duration>0)
		{
			source.Remove(intruder);
		}
		if(source.Count==0)
		{
			TurnOff();
		}
	}
	void TurnOn()
	{
		on = true;
		steam.SetActive(true);		
		lid.GetComponent<Rigidbody>().isKinematic = true;
		lidMoveable.gravitated = false;
	}

	void TurnOff()
	{
		on = false;
		steam.SetActive(false);
		duration = 0f;
		lidMoveable.gravitated = true;
	}


}