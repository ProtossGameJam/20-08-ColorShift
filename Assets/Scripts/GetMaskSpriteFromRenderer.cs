using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteMask),typeof(SpriteRenderer))]
public class GetMaskSpriteFromRenderer : MonoBehaviour
{
    new SpriteRenderer renderer;
    SpriteMask mask;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();

        mask.sprite = renderer.sprite;
    }

    private void OnAnimatorMove()
    {
        mask.sprite = renderer.sprite;
    }
}
