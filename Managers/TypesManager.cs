using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TypesManager : MonoBehaviour
{
    public static Dictionary<Types.TileType, Color> dict_typeToColor;

    private void Awake()
    {
        dict_typeToColor = new Dictionary<Types.TileType, Color>
        {
            { Types.TileType.BLUE,              Globals.BLUE},
            { Types.TileType.GREEN,             Globals.GREEN},
            { Types.TileType.RED,               Globals.RED},
            //{ Types.TileType.PINK,              Globals.PINK},
            //{ Types.TileType.ORANGE,            Globals.ORANGE},
            //{ Types.TileType.PURPLE,            Globals.PURPLE},
            { Types.TileType.YELLOW,            Globals.YELLOW},
            //{ Types.TileType.BROWN,             Globals.BROWN},
        };
    }
    // Returns color of given type
    public static Color GetColor(Types.TileType type)
    {
        Color newCol;
        dict_typeToColor.TryGetValue(type, out newCol);
        //TO DO LATER - IMPLEMENT HANDLING CASE OF NOT FINDING VALUE
        return newCol;
    }

    public static Types.TileType GetRandomType()
    {
        float rand = Random.Range(0, GameManager.ColorCount);
        return (Types.TileType)rand;
    }
}
