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

    //GameObject player = MapComponentData.instance.player;
    //GameObject stoneBlock = MapComponentData.instance.stoneBlock;
    //GameObject metalBlock = MapComponentData.instance.metalBlock;
    //GameObject goal = MapComponentData.instance.goal;

    //GameObject button = MapComponentData.instance.goal;
    //GameObject key = MapComponentData.instance.key;

    private void Start()
    {
        stageCamera = GameObject.Find("Main Camera");
        int mapWidth = map.width;
        int mapHeight = map.height;
        stageCamera.GetComponent<Camera>().orthographicSize = map.height / 2;

        if (transform.childCount == 0)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Color pixelColor = map.GetPixel(x, y);
                    if (ObjectOfColor(pixelColor) != null)
                        Instantiate(ObjectOfColor(pixelColor), new Vector3(x - mapWidth / 2, y - mapHeight / 2, 0), gameObject.transform.rotation, gameObject.transform);
                }
            }
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
