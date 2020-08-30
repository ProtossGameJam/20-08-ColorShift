using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer), typeof(Tilemap))]
public class TileMapShadowGenerator : MonoBehaviour
{
    [Header("This Component is simply copy object and set mask and color.", order = 0)]
    [Header("Apply to FakeWall_Lower", order = 1)]
    [Header("Use Context menu to generate")]
    public Color shadowColor = Color.gray;

    [ContextMenu("Generate")]
    void Generate()
    {

        GameObject shadowObject = Instantiate(gameObject, transform.parent);

        shadowObject.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        shadowObject.GetComponent<Tilemap>().color = shadowColor;

        shadowObject.name = gameObject.name + "_Shadow";
    }

}
