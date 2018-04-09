using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public static Sprite tileSprite;

    public static TileSinglePart partPrefab;

    private static Tile tilePrefab;

    private void Awake()
    {
        tilePrefab = Resources.Load<Tile>("tilePrefab");
        tileSprite = Resources.Load<Sprite>("tileSprite");
        partPrefab = Resources.Load<TileSinglePart>("partPrefab");
    }

    public static Tile CreateNewTile( Vector3 pos )
    {
        Tile newTile = Instantiate(tilePrefab, pos, Quaternion.identity);
        newTile.Init();
        return newTile;
    }

    public static void TransferTile( BoardSlot toSlot )
    {
        toSlot.tileInSlot = GameManager.baseTile;
        toSlot.tileInSlot.transform.position = toSlot.transform.position;
        toSlot.tileInSlot.transform.parent = toSlot.transform;
        toSlot.tileInSlot.owner = toSlot;

        GameManager.baseTile = CreateNewTile( GameManager.baseTilePos );
    }

    public static Types.TileDirection MirrorDirection( Types.TileDirection dir )
    {
        switch( dir )
        {
            case Types.TileDirection.UP:
                return Types.TileDirection.DOWN;
            case Types.TileDirection.DOWN:
                return Types.TileDirection.UP;
            case Types.TileDirection.RIGHT:
                return Types.TileDirection.LEFT;
            case Types.TileDirection.LEFT:
                return Types.TileDirection.RIGHT;
        }

        return Types.TileDirection.UP;
    }

    public static bool AreAdjacent( Types.TileDirection first, Types.TileDirection second )
    {
        switch( first )
        {
            case Types.TileDirection.UP:
            case Types.TileDirection.DOWN:
                return (second == Types.TileDirection.LEFT || second == Types.TileDirection.RIGHT);
            case Types.TileDirection.LEFT:
            case Types.TileDirection.RIGHT:
                return (second == Types.TileDirection.UP || second == Types.TileDirection.DOWN);
            default:
                return (first == second);
        }
    }

    public static TileSinglePart GetMirroredPart( Tile tileToCheck, Types.TileType type, Types.TileDirection dir )
    {
        return tileToCheck.GetData().GetPartFromDir(MirrorDirection(dir));
    }

    // CHECK ARE THE TYPES SAME WITHIN SAME TILE
    public static bool AreTypesSame( TileParts parts, Types.TileDirection first, Types.TileDirection second)
    {
         return parts.GetPartFromDir(first)._type == parts.GetPartFromDir(second)._type;
    }

    // CHECK IF TYPES FOR PARTS ARE SAME IN TWO DIFFERENT TILES
    public static bool AreTypesSame( Tile firstTile, Tile secondTile, Types.TileDirection direction )
    {
        TileParts firstData = firstTile.GetData();
        TileParts secondData = secondTile.GetData();
        bool ret = (firstData.GetPartFromDir(direction)._type == secondData.GetPartFromDir(MirrorDirection(direction))._type);
        return ret;
    }

    public static bool HasDataBeenChecked( BoardSlot slot, Types.TileDirection dir )
    {
        bool ret = slot.GetTile().GetData().GetPartFromDir( dir )._beenChecked;
        return ret;
    }

    public static void SetDataBeenChecked( BoardSlot slot, Types.TileDirection dir, bool value )
    {
        slot.GetTile().GetData().GetPartFromDir( dir )._beenChecked = value;
    }

    public static bool CanMoveToPart( Tile tileToCheck, TileSinglePart currentPart, Types.TileDirection tempDir)
    {
        bool result = AreAdjacent(currentPart._direction, tempDir);

        return result;
    }
}
