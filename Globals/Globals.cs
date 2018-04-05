using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Globals
{
    //COLORS
    public static Color BOARD_SLOT = HexToColor("909abaff");
    public static Color BLUE = HexToColor("3898ffff");
    public static Color GREEN = HexToColor("83e04cff");
    public static Color RED = HexToColor("e14141ff");
    public static Color YELLOW = HexToColor("fff275ff");
    public static Color ORANGE = HexToColor("ffbf36ff");
    public static Color PURPLE = HexToColor("bf3fb3ff");
    public static Color PINK = HexToColor("ff80aaff");
    public static Color BROWN = HexToColor("7a213aff");

    public static Color BACKGROUND = HexToColor("7884ABFF");

    public static string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    public static List< Types.TileDirection > GetFullDirectionCircle( Types.TileDirection startingDir )
    {
        List<Types.TileDirection> dirList = new List<Types.TileDirection>();

        int dirCount = System.Enum.GetValues(typeof(Types.TileDirection)).Length;
        int i = (int)startingDir;

        while( dirList.Count != dirCount )
        {
            if( !dirList.Contains( (Types.TileDirection)i ) )
            {
                dirList.Add((Types.TileDirection)i);
            }

            if ( i + 1 >= dirCount )
            {
                i = 0;
            }
            else
            {
                i++;
            }
        }

        return dirList;
    }

    public static Types.TileDirection GetNextDirection(Types.TileDirection current)
    {
        if( (int)current >= System.Enum.GetValues( typeof(Types.TileDirection) ).Length )
        {
            return (Types.TileDirection)0;
        }
        else
        {
            return (Types.TileDirection)((int)current + 1);
        }
    }
}
