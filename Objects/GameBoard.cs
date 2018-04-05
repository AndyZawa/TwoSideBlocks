using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Board setup")]
    public int rowSize;
    public int colSize;

    [Header("Prefabs")]
    public BoardSlot slotPrefab;

    [Header("Borders")]
    public Border leftBorder;
    public Border rightBorder;


    private BoardSlot[,] boardPos;

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        boardPos = new BoardSlot[colSize, rowSize];
        for( int x = 0; x < colSize; x++ )
        {
            for( int y = 0; y < rowSize; y ++ )
            {
                SpawnBoardSlot(x, y);
            }
        }
    }

    private void SpawnBoardSlot( int colPos, int rowPos )
    {
        BoardSlot newSlot = Instantiate(slotPrefab, GetWorldPosition(colPos + transform.position.x, rowPos + transform.position.y), Quaternion.identity);
        newSlot.Initialize(colPos, rowPos, transform );
        boardPos[colPos, rowPos] = newSlot;
    }

    private Vector3 GetWorldPosition( float x, float y )
    {
        return new Vector3((x - colSize / 2), (y - rowSize / 2), 1f);
    }

    private BoardSlot GetSlot( int colPos, int rowPos )
    {
        if ( (colPos < 0 || colPos >= colSize ) || (rowPos < 0 || rowPos >= rowSize ))
        {
            return null;
        }
        else
        {
            return boardPos[colPos, rowPos];
        }
    }

    private BoardSlot GetSlotByDirection(BoardSlot curSlot, Types.TileDirection dir)
    {
        BoardSlot newSlot;
        switch (dir)
        {
            case Types.TileDirection.UP:
                newSlot = GetSlot(curSlot.colPos, curSlot.rowPos + 1);
                break;
            case Types.TileDirection.RIGHT:
                newSlot = GetSlot(curSlot.colPos + 1, curSlot.rowPos);
                break;
            case Types.TileDirection.DOWN:
                newSlot = GetSlot(curSlot.colPos, curSlot.rowPos - 1);
                break;
            case Types.TileDirection.LEFT:
                newSlot = GetSlot(curSlot.colPos - 1, curSlot.rowPos);
                break;
            default:
                newSlot = null;
                break;
        }

        return newSlot;
    }

    public void ClearBoard()
    {
        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                if (boardPos[x, y].IsOccupied())
                {
                    boardPos[x, y].GetTile().DestroyTile();
                    boardPos[x, y].ResetSlot();
                }
            }
        }
    }

    public void StartCheck( BoardSlot slot )
    {
        List<TileSinglePart> collectedParts = new List<TileSinglePart>();
        CheckTile( slot.GetTile(), Types.TileDirection.UP, ref collectedParts );
    }

    public void CheckTile(Tile tile, Types.TileDirection direction, ref List<TileSinglePart> collectedParts )
    {
        TileParts allParts = tile.GetData();

        List<Types.TileDirection> directions = Globals.GetFullDirectionCircle( direction ); // UP is always a default starting point

        foreach( Types.TileDirection dir in directions )
        {
            TileSinglePart singlePart = allParts.GetPartFromDir(dir);

            LookForType(tile, singlePart, singlePart._type, ref collectedParts);

            DoStuffWithParts(ref collectedParts);
        }
    }

    private void LookForType( Tile tile, TileSinglePart singlePart, Types.TileType type, ref List<TileSinglePart> collectedParts )
    {
        List<Types.TileDirection> directions = Globals.GetFullDirectionCircle(singlePart._direction);
        TileSinglePart tempPart;

        foreach( Types.TileDirection dir in directions )
        {
            tempPart = tile.GetData().GetPartFromDir(dir);
            if( tempPart._type == type && !collectedParts.Contains( singlePart )/*Lacking adjancet type implementation*/ )
            {
                // Add to list
                collectedParts.Add(tempPart);
                CheckNeighbourTile(tile, type, dir, ref collectedParts);
            }
        }
    }

    private void CheckNeighbourTile( Tile tileToCheck, Types.TileType type, Types.TileDirection direction, ref List<TileSinglePart> collectedParts )
    {
        BoardSlot tempSlot = GetSlotByDirection( tileToCheck.owner, direction);
        if( tempSlot && tempSlot.IsOccupied() )
        {
            CheckTile(tempSlot.tileInSlot, TilesManager.MirrorDirection( direction ), ref collectedParts );
        }
    }

    private void DoStuffWithParts( ref List<TileSinglePart> parts )
    {
        if (parts.Count > 1)
        {
            foreach (TileSinglePart part in parts)
            {
                part.TEST_FUNC();
            }
        }

        parts.Clear();
    }

    public void DEBUG_TEST1()
    {
        ClearBoard();
    }
}


