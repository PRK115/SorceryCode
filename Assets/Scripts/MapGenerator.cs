using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Texture2D map;
    public GameObject stoneBlock;
    public GameObject metalBlock;
    public GameObject goal;
    public GameObject player;

    private void Awake()
    {
        int mapWidth = map.width;
        int mapHeight = map.height;

        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                Color pixelColor = map.GetPixel(x, y);
                Debug.Log(x + "," + y + "  " + pixelColor);
                if(ObjectOfColor(pixelColor) != null)
                    Instantiate(ObjectOfColor(pixelColor), new Vector3(x-mapWidth/2, y-mapHeight/2, 0), gameObject.transform.rotation, gameObject.transform);
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

    GameObject ObjectOfColor(Color a)
    {
        if (ColorCloseEnough(a, Color.black))
            return stoneBlock;
        if (ColorCloseEnough(a, Color.gray))
            return metalBlock;
        if (ColorCloseEnough(a, Color.green))
            return goal;
        if (ColorCloseEnough(a, Color.red))
            return player;
        else
            return null;
    }
}
