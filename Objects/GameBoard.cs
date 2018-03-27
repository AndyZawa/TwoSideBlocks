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

    public void CheckSlot(BoardSlot slot)
    {
        BoardSlot leftSlot, rightSlot;
        leftSlot = GetSlot(slot.colPos - 1, slot.rowPos);
        rightSlot = GetSlot(slot.colPos + 1, slot.rowPos);

        TileData checkedTileData = slot.GetTile().GetTileData();

        if ( leftSlot && rightSlot )
        {
            if (leftSlot.IsOccupied() && rightSlot.IsOccupied())
            {
                TileData leftTileData = leftSlot.GetTile().GetTileData();
                TileData rightTileData = rightSlot.GetTile().GetTileData();                

                if (leftTileData.rightType == checkedTileData.leftType && rightTileData.leftType == checkedTileData.rightType)
                {
                    Debug.Log("Match found! Slot emptied!");
                    RotateAdjacentSlots( slot, Types.TileCheck.VERTICAL );
                    RotateAdjacentSlots(slot, Types.TileCheck.HORIZONTAL);

                    slot.GetTile().DestroyTile();
                }
            }
        }
        else if( !leftSlot && rightSlot )
        {
            TileData rightTileData = rightSlot.GetTile().GetTileData();

            if (rightTileData.leftType == checkedTileData.rightType && checkedTileData.leftType == leftBorder.bType)
            {
                RotateAdjacentSlots(slot, Types.TileCheck.VERTICAL);
                RotateAdjacentSlots(slot, Types.TileCheck.HORIZONTAL);
                slot.GetTile().DestroyTile();
            }
        }
        else if( leftSlot && !rightSlot )
        {
            TileData leftTileData = leftSlot.GetTile().GetTileData();

            if( leftTileData.rightType == checkedTileData.leftType && checkedTileData.rightType == rightBorder.bType )
            {
                RotateAdjacentSlots(slot, Types.TileCheck.VERTICAL);
                RotateAdjacentSlots(slot, Types.TileCheck.HORIZONTAL);
                slot.GetTile().DestroyTile();
            }
        }

        ChangeBorderColors();
    }

    private void ChangeBorderColors()
    {
        leftBorder.Recolor();
        rightBorder.Recolor();
    }

    private void RotateAdjacentSlots(BoardSlot slot, Types.TileCheck checkType)
    {
        BoardSlot one, two;

        switch (checkType)
        {
            case Types.TileCheck.HORIZONTAL:
                one = GetSlot(slot.colPos - 1, slot.rowPos);
                two = GetSlot(slot.colPos + 1, slot.rowPos);
                break;
            case Types.TileCheck.VERTICAL:
                one = GetSlot(slot.colPos, slot.rowPos + 1);
                two = GetSlot(slot.colPos, slot.rowPos - 1);
                break;
            default:
                one = two = null;
                break;
        }

        if( one && one.IsOccupied() )
        {
            if( one.GetTile().GetTileData().IsSolid() )
            {
                one.GetTile().DestroyTile();
            }

            one.GetTile().Rotate();
        }

        if( two && two.IsOccupied() )
        {
            if (two.GetTile().GetTileData().IsSolid())
            {
                two.GetTile().DestroyTile();
            }
            two.GetTile().Rotate();
        }
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
}
