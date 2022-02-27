using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation.Effects;
using TMPro;

public class AnimationTest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public TextMeshPro tmpText;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimationEffect.FadeOut(spriteRenderer, 2f));
        StartCoroutine(AnimationEffect.Pulse(transform, new Vector3(1.1f, 1.1f, 1f)));
    }


}
