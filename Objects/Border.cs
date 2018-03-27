using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour
{
    public Types.TileType bType;

    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        Recolor();
    }

    public void Recolor()
    {
        bType = TypesManager.GetRandomType();
        rend.color = TypesManager.GetColor(bType);
    }

}
