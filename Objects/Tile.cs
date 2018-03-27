using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData data;

    public SpriteRenderer leftRend;
    public SpriteRenderer rightRend;

    private bool rotated;
    private Vector3 targetRotation;

    public Tile( TileData newData )
    {
        SetTileData(newData);
    }

    public void SetTileData( TileData newData )
    {
        data = newData;
        leftRend.color = data.leftColor;
        rightRend.color = data.rightColor;
    }

    public TileData GetTileData()
    {
        return data;
    }

    public void DestroyTile()
    {
        Destroy(gameObject);
    }

    public void Rotate()
    {
        targetRotation = (!rotated) ? new Vector3(0, 0, GameConsts.ROTATION_FINAL) : new Vector3( 0, 0, 0 );

        StartCoroutine( StartRotation( targetRotation, GameConsts.ROTATION_TIME ) );
        rotated = !rotated;
        ReverseType();
    }

    private void ReverseType()
    {
        Types.TileType temp = data.leftType;
        data.leftType = data.rightType;
        data.rightType = temp;
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
    }
}

public struct TileData
{
    // Left Side
    public Types.TileType leftType;
    public Color leftColor;

    // Right Side
    public Types.TileType rightType;
    public Color rightColor;

    public TileData( Types.TileType lType, Types.TileType rType )
    {
        leftType = lType;
        leftColor = TypesManager.GetColor(lType);

        rightType = rType;
        rightColor = TypesManager.GetColor(rType);
    }

    public bool IsSolid()
    {
        return (leftType == rightType) ? true : false;
    }
}
