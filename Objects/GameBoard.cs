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

        List<TileSinglePart> allParts = new List<TileSinglePart>();
        slot.GetTile().GetData().ListAllParts(ref allParts);

        foreach (TileSinglePart part in allParts)
        {
            collectedParts.Add(part);
            CheckPart(slot.GetTile(), part, part._type, ref collectedParts);
            DoStuffWithParts(ref collectedParts);
        }
    }



    private void CheckPart(Tile currentTile, TileSinglePart currentPart, Types.TileType typeToCheck, ref List<TileSinglePart> collectedParts )
    {
        BoardSlot tempSlot;
        Tile tempTile;
        TileSinglePart tempPart;

        List<Types.TileDirection> directions = new List<Types.TileDirection>();
        directions = Globals.GetFullDirectionCircle(Types.TileDirection.UP);

        foreach( Types.TileDirection dir in directions )
        {
            tempPart = currentTile.GetData().GetPartFromDir(dir);
            if (tempPart && tempPart._type == typeToCheck && !collectedParts.Contains(tempPart) && TilesManager.AreAdjacent(currentPart._direction, dir))
            {
                collectedParts.Add(tempPart);
                CheckPart(currentTile, tempPart, typeToCheck, ref collectedParts);
            }

            tempSlot = GetSlotByDirection(currentTile.owner, currentPart._direction);
            if (tempSlot && tempSlot.IsOccupied())
            {
                tempTile = tempSlot.GetTile();
                if (tempTile && CanGoToNeighbour(currentTile, tempTile, typeToCheck, currentPart._direction))
                {
                    tempPart = TilesManager.GetMirroredPart(tempTile, typeToCheck, currentPart._direction);
                    if (!collectedParts.Contains(tempPart))
                    {
                        collectedParts.Add(tempPart);
                        CheckPart(tempTile, tempPart, typeToCheck, ref collectedParts);
                    }
                }
            }
        }
    }

    private bool CanGoToNeighbour( Tile curTile, Tile tileToCheck, Types.TileType typeToCheck, Types.TileDirection direction )
    {
        if ( TilesManager.AreTypesSame(curTile, tileToCheck, direction) )
        {
            return true;
        }
        else
        {
            return false;
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


