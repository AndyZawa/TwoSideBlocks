using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BoardSlot owner;
    public TileParts parts;

    private bool rotating;
    private Vector3 targetRotation;

    public void Init()
    {
        parts._up       = Instantiate(TilesManager.partPrefab, transform.position + new Vector3(0, 0.24f, 0), Quaternion.Euler(0f, 0f, 180f));
        parts._up.Init(this, Types.TileDirection.UP, (Types.TileType)Random.Range( 0, GameManager.ColorCount ));

        parts._right    = Instantiate(TilesManager.partPrefab, transform.position + new Vector3(0.24f, 0, 0), Quaternion.Euler(0f, 0f, 90f));
        parts._right.Init(this, Types.TileDirection.RIGHT, (Types.TileType)Random.Range(0, GameManager.ColorCount));

        parts._down     = Instantiate(TilesManager.partPrefab, transform.position + new Vector3(0, -0.24f, 0), Quaternion.Euler(0f, 0f, 0));
        parts._down.Init(this, Types.TileDirection.DOWN, (Types.TileType)Random.Range(0, GameManager.ColorCount));

        parts._left     = Instantiate(TilesManager.partPrefab, transform.position + new Vector3(-0.24f, 0, 0), Quaternion.Euler(0f, 0f, -90));
        parts._left.Init(this, Types.TileDirection.LEFT, (Types.TileType)Random.Range(0, GameManager.ColorCount));
    }

    public void DestroyTile()
    {
        Destroy(gameObject);
    }

    public TileParts GetData()
    {
        return parts;
    }

    public void Rotate()
    {
        if (!rotating)
        {
            rotating = true;
            targetRotation = transform.rotation.eulerAngles + new Vector3(0, 0, GameConsts.ROTATION_FINAL);

            StartCoroutine(StartRotation(targetRotation, GameConsts.ROTATION_TIME));
            ChangeTypesClockwise();
        }
    }

    private void ChangeTypesClockwise()
    {
        TileSinglePart tempPart = parts._up;

        parts._up = parts._left;
        parts._up._direction = Types.TileDirection.UP;

        parts._left = parts._down;
        parts._left._direction = Types.TileDirection.LEFT;

        parts._down = parts._right;
        parts._down._direction = Types.TileDirection.DOWN;

        parts._right = tempPart;
        parts._right._direction = Types.TileDirection.RIGHT;
    }

    IEnumerator StartRotation( Vector3 targetRot, float time )
    {
        float elapsedTime = 0.0f;
        Vector3 startingPosition = transform.localPosition;

        Quaternion startingRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler( targetRot );
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
            // Rotations
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
        }

        rotating = false;
        GameManager.CheckSlot(transform.parent.GetComponent<BoardSlot>());
    }
}

public struct TileParts
{
    public TileSinglePart _up;
    public TileSinglePart _right;
    public TileSinglePart _down;
    public TileSinglePart _left;

    public TileSinglePart GetPartFromDir( Types.TileDirection dir )
    {
        List<TileSinglePart> parts = new List<TileSinglePart> { _up, _right, _down, _left };
        foreach( TileSinglePart part in parts )
        {
            if( part._direction == dir )
            {
                return part;
            }
        }

        return null;        
    }

    public void ListAllParts( ref List<TileSinglePart> partsListed )
    {
        partsListed.Add(_up);
        partsListed.Add(_right);
        partsListed.Add(_down);
        partsListed.Add(_left);
    }
}