using UnityEngine;
using System.Collections;

public class TileSinglePart : MonoBehaviour
{
    public Tile _owner;
    public Types.TileDirection _direction;
    public Types.TileType _type;
    public Color _color;
    public bool _beenChecked;

    public SpriteRenderer rend;

    public void Init(Tile owner, Types.TileDirection dir, Types.TileType type)
    {
        _owner = owner;
        _direction = dir;
        _type = type;
        _color = TypesManager.GetColor(_type);
        _beenChecked = false;
        rend.color = _color;

        transform.parent = owner.transform;
    }

    public void TEST_FUNC()
    {
        rend.color = Color.white;
    }
}
