using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComponentData : MonoBehaviour {

    public GameObject player;
    public GameObject stoneBlock;
    public GameObject metalBlock;
    public GameObject goal;

    public GameObject button;
    public GameObject key;

    public static MapComponentData instance = null;

    private void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

}
