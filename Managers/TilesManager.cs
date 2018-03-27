using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public static Sprite tileSprite;

    private static Tile tilePrefab;

    private void Awake()
    {


        tilePrefab = Resources.Load<Tile>("tilePrefab");
        tileSprite = Resources.Load<Sprite>("tileSprite");
    }

    public static Tile CreateNewTile( Vector3 pos )
    {
        Tile newTile = Instantiate(tilePrefab, pos, Quaternion.identity);
        newTile.SetTileData( TypesManager.RandomizeTile());
        return newTile;
    }

    public static void TransferTile( BoardSlot toSlot )
    {
        toSlot.tileInSlot = Instantiate(tilePrefab, toSlot.transform.position, Quaternion.identity);
        toSlot.tileInSlot.SetTileData(GameManager.baseTile.GetTileData());
        GameManager.baseTile.SetTileData( TypesManager.RandomizeTile() );
    }
}
