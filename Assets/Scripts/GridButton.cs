using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridButton : MonoBehaviour {

    public GameObject gridPrefab;
    private GameObject grid;

    private void Awake()
    {
        grid = Instantiate(gridPrefab);
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GridOnOff);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GridOnOff();
        }
    }

    public void GridOnOff()
    {
        if(grid != null)
            grid.SetActive(!grid.activeInHierarchy);
    }
}
