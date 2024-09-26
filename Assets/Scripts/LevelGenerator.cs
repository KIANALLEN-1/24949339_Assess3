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
     float columns() {return map.GetLength(1);} //y
     float rows() {return map.GetLength(0);} //x
    
    
    void Start()
    {
        
        mainCam.transform.position = new Vector3(2*((columns() * TileSize) / 2 - TileSize) , 
            -2*((rows() * TileSize) / 2 - TileSize),-127); //centres cam 
        
        mainCam.orthographicSize = 2*(GetMapLength(columns(), rows()) * TileSize / 2 + 1*TileSize);
        
        PlaceTiles();
    }

    private void PlaceTiles()
    {
        for (int y = 0; y < rows(); y++)
        {
            for (int x = 0; x < columns(); x++)
            {
                int tileType = map[y, x];
                int rotation = 0;
                int flip = 0;

                bool left = (x > 0 && CanConnect(tileType, map[y, x - 1]));
                bool right = (x < columns() - 1 && CanConnect(tileType, map[y, x + 1]));
                bool top = (y > 0 && CanConnect(tileType, map[y - 1, x]));
                bool bottom = (y < rows() - 1 && CanConnect(tileType, map[y + 1, x]));
                
                
                
                
                switch (tileType)
                {
                    case 0:
                        break;
                    case 1:
                        if (bottom && right) 
                        {
                            rotation = 0;
                        } else if (right && top)
                        {
                            rotation = 90;
                        } else if (top && left)
                        {
                            rotation = 180;
                        } else if (left && bottom)
                        {
                            rotation = 270;
                        } 
                        break;
                    case 2:
                        if (top && bottom)
                        {
                            rotation = 0;
                        } else if (left && right)
                        {
                            rotation = 90;
                        }
                        break;
                    case 3:
                        if (bottom && right) 
                        {
                            rotation = 0;
                        } else if (right && top)
                        {
                            rotation = 90;
                        } else if (top && left)
                        {
                            rotation = 180;
                        } else if (left && bottom)
                        {
                            rotation = 270;
                        } 
                        break;
                    case 4:
                        if (top && bottom)
                        {
                            rotation = 0;
                        } else if (left && right)
                        {
                            rotation = 90;
                        }
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        if (!top)
                        {
                            rotation = 0;
                            if (map[y, x - 1]==5)
                            {
                                
                            }
                        }
                        //flip = 180;
                        break;
                }
                
                Instantiate(tilePrefabs[tileType], new Vector3(x*TileSize,-y*TileSize), new Quaternion(0,flip,rotation,1));

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        //outside walls connections
        if ((type1 == 1 || type1 == 2 || type1 == 7) && (type2 == 1 || type2 == 2 || type2 == 7)) { return true; }
        if ((type1 == 3 || type1 == 4))
        {
            
        }
        if (type1 == 7)
        {
            
        }
        
        return false;
    }
}
