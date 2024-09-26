using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;
    public Camera mainCam;
    private const float TileSize = 3.2f;
    private GameObject tileParent;
    private GameObject topLeftQuadrant;
    public GameObject originalMap;
    public Sprite tile5;
    public Sprite tile6;
    private bool onceBool = true;

    private readonly int[,] map =
    {
        { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4 },
        { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4 },
        { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0 },
        { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0 },
    };
     float Columns() {return map.GetLength(1);} //y
     float Rows() {return map.GetLength(0);} //x
    
    
    void Start()
    {
        Destroy(originalMap);
        mainCam.transform.position = new Vector3(2*((Columns() * TileSize- TileSize) / 2 ) , 
            -2*((Rows() * TileSize - TileSize)/ 2) ,-127); //centres cam 
        
        mainCam.orthographicSize = (GetMapLength(Columns(), Rows()) * TileSize+ 0.5f * TileSize);
        
        tileParent = new GameObject();
        topLeftQuadrant = new GameObject();
        topLeftQuadrant.name = "TopLeftQuadrant";
        topLeftQuadrant.transform.SetParent(tileParent.transform);
        PlaceTiles();
        
        
        GameObject topRightQuadrant = Instantiate(topLeftQuadrant, new Vector3(Columns()*TileSize*2-TileSize,0,0), quaternion.identity);
        topRightQuadrant.transform.localScale = new Vector3(-1, 1, 1);
    }

    private void PlaceTiles()
    {
        for (int y = 0; y < Rows(); y++)
        {
            for (int x = 0; x < Columns(); x++)
            {
                int tileType = map[y, x];
                float rotation = 0.0f;
                float flip = 0.0f;
                float spin = 0.0f;

                bool left = (x > 0 && CanConnect(tileType, map[y, x - 1]));
                bool right = (x < Columns() - 1 && CanConnect(tileType, map[y, x + 1]));
                bool top = (y > 0 && CanConnect(tileType, map[y - 1, x]));
                bool bottom = (y < Rows() - 1 && CanConnect(tileType, map[y + 1, x]));
                
                
                switch (tileType)
                {
                    case 0:
                        break;
                    case 1:
                        if (bottom && right)  rotation = 0.0f;
                        else if (right && top) rotation = 90.0f;
                        else if (top && left)  rotation = 180.0f;
                        else if (left && bottom) rotation = 270.0f;
                        break;
                    case 2:
                        if (top && bottom) rotation = 0.0f;
                        else if (left && right) rotation = 90.0f;
                        //failsafe
                        else if (top || bottom) rotation = 0.0f;
                        else if (left || right) rotation = 90.0f;
                        break;
                    case 3:
                        if (right && top) rotation = 90.0f;
                        else if (top && left) rotation = 180.0f;
                        else if (left && bottom) rotation = 270.0f;
                        else if (bottom && right)  rotation = 0.0f;
                        else if (top && x >= Columns()-0.5) rotation = 90.0f;
                        break;
                    case 4:
                        if (top && bottom) rotation = 0.0f;
                        else if (left && right) rotation = 90.0f;
                        else if (left || right) rotation = 90.0f;
                        else if (top || bottom) rotation = 0.0f;
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        if (!top)
                        {
                            rotation = 0;
                            if (x > 0 && map[y, x - 1]==2 || x < Columns() && map[y, x + 1]==4)
                            {
                                flip = 0.0f;
                            } else if (x > 0 && map[y, x - 1] == 4 || x < Columns() &&  map[y, x + 1] == 2)
                            {
                                flip = 180.0f;
                            }
                        } else if (!left)
                        {
                            rotation = 90.0f;
                            if (y > 0 && map[y-1, x ]==2 || y < Rows() && map[y+1, x]==4)
                            {
                                spin = 180.0f;
                            } else if (y > 0 && map[y-1, x] == 4 || y < Rows() && map[y+1, x] == 2)
                            {
                                spin = 0.0f;
                            }
                        } else if (!bottom)
                        {
                            rotation = 180.0f;
                            if (x > 0 && map[y, x - 1]==2 || x < Columns() &&  map[y, x + 1]==4)
                            {
                                flip = 180.0f;
                            } else if (x > 0 && map[y, x - 1] == 4 || x < Columns() &&  map[y, x + 1] == 2)
                            {
                                flip = 0.0f;
                            }
                        } else if (!right)
                        {
                            rotation = 270.0f;
                            if (y > 0 && map[y-1, x ]==2 || y < Rows() && map[y+1, x]==4)
                            {
                                spin = 0.0f;
                            } else if (y > 0 && map[y-1, x] == 4 || y < Rows() && map[y+1, x] == 2)
                            {
                                spin = 180.0f;
                            }
                        }
                        break;
                }
                
                
                //instantiate tiletype(0 through 7) at location x,y(in array) times by tilesize with rotation and flip for tile type 7
                Quaternion tileRotation = Quaternion.Euler(spin, flip, rotation);
                GameObject tile = Instantiate(tilePrefabs[tileType], new Vector3(x*TileSize,-y*TileSize), tileRotation);
                Debug.Log("type: " + tileType + "r: "+ rotation + ", f: "+flip+",s: " + spin + ", coords: " + x +","+ y);
                tile.transform.SetParent(topLeftQuadrant.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onceBool)
        {
            foreach (Transform tile in topLeftQuadrant.transform)
            {
                SpriteRenderer tileType = tile.GetComponent<SpriteRenderer>();
                if (tileType.sprite == tile5 || tileType.sprite == tile6)
                {
                    
                }
            }

            onceBool = false;
        }
    }

    float GetMapLength(float w, float l)
    {
        if (l > w*1.875) {
            return l; }
        else {
            return w; }
    }

    bool CanConnect(int type1, int type2)
    {
        if (type1 == 0 || type2 == 0) { return false; } //empty space
        
        //wall connections
        if ((type1 == 1 || type1 == 2) && (type2 == 1 || type2 == 2 || type2 == 7)) { return true; }
        if ((type1 == 3 || type1 == 4) && (type2 == 3 || type2 == 4 || type2 == 7)) { return true; }
        if ((type1 == 7) && (type2 == 1 || type2 == 2 || type2 == 3 || type2 == 4)) { return true; }
        return false;
    }
}
