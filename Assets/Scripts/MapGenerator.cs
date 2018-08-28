using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Texture2D map;

    public GameObject player;
    public GameObject stoneBlock;
    public GameObject metalBlock;
    public GameObject goal;

    public GameObject button;
    public GameObject key;

    GameObject stageCamera;
    public bool resizeCamera;

    //private void Awake()
    //{
    //    GameObject player;
    //    GameObject stoneBlock;
    //    GameObject metalBlock;
    //    GameObject goal;

    //    GameObject button;
    //    GameObject key;
    //}

    private void Start()
    {
        stageCamera = GameObject.Find("Main Camera");
        int imageWidth = map.width;
        int imageHeight = map.height;


        int lowest = imageHeight;
        int highest = 0;
        //int leftMost;

        if (transform.childCount == 0)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    Color pixelColor = map.GetPixel(x, y);
                    GameObject pixelObject = ObjectOfColor(pixelColor);
                    if (pixelObject != null)
                    {
                        Instantiate(pixelObject, new Vector3(x - imageWidth / 2, y), pixelObject.transform.rotation, gameObject.transform);
                        if (y < lowest)
                            lowest = y;
                        if (y > highest)
                            highest = y;
                    }
                }
            }
            int mapHeight = highest - lowest;
            Debug.Log(mapHeight);
            if(resizeCamera)
                stageCamera.GetComponent<Camera>().orthographicSize = mapHeight / 2;
            transform.Translate(new Vector3(0, -imageHeight / 2, 0));
        }
    }

    bool ColorCloseEnough(Color a, Color b)
    {
        Vector3 A = new Vector3(a.r, a.g, a.b);
        Vector3 B = new Vector3(b.r, b.g, b.b);

        if (Vector3.Distance(A,B) < 0.1)
        {
            return true;
        }
        else
            return false;
    }

    GameObject ObjectOfColor(Color pixelColor)
    {
        if (ColorCloseEnough(pixelColor, Color.black))
            return stoneBlock;
        if (ColorCloseEnough(pixelColor, Color.gray))
            return metalBlock;
        if (ColorCloseEnough(pixelColor, Color.yellow))
            return key;
        if (ColorCloseEnough(pixelColor, Color.blue))
            return button;
        if (ColorCloseEnough(pixelColor, Color.green))
            return goal;
        if (ColorCloseEnough(pixelColor, Color.red))
            return player;
        else
            return null;
    }
}
