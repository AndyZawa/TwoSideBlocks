using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int colorsCount;
    private static GameBoard board;

    public static Tile baseTile;
    public static Vector3 baseTilePos = new Vector3(0f, -4.3f, 0);

    public static int ColorCount
    {
        get{ return colorsCount; }
        set
        {
            colorsCount = (colorsCount + 1 > TypesManager.dict_typeToColor.Count) ? colorsCount : value;
        }        
    }

    private void Start()
    {
        ColorCount = 4;
        baseTile = TilesManager.CreateNewTile( baseTilePos );
        board = FindObjectOfType<GameBoard>();
        Camera.main.backgroundColor = Globals.BACKGROUND;
    }

    public static void CheckSlot( BoardSlot slot )
    {
        board.StartCheck( slot );
    }
}
