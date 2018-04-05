using UnityEngine;
using System.Collections;

public class BoardSlot : MonoBehaviour
{
    private new SpriteRenderer renderer;

    public int colPos;
    public int rowPos;

    public Tile tileInSlot;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize( int x, int y, Transform newParent )
    {
        renderer.sprite = TilesManager.tileSprite;
        renderer.color = Globals.BOARD_SLOT;

        colPos = x;
        rowPos = y;
        transform.parent = newParent;

        ResetSlot();
    }

    private void OnMouseDown()
    {
        if( !IsOccupied() )
        {
            TilesManager.TransferTile( this );
        }
        else
        {
            tileInSlot.Rotate();
        }
    }

    public void ResetSlot()
    {
        tileInSlot = null;
    }

    public Tile GetTile()
    {
        return tileInSlot;
    }

    // Check if there is something in given slot
    public bool IsOccupied()
    {
        return (tileInSlot != null) ? true : false;
    }
}
