﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : MonoBehaviour
{
    public Sprite defaultSpr;
    public Sprite[] explosionSpr;

    [SerializeField]
    private float animSpeed = 0.05f;
    private WaitForSeconds explosionAnimSpeed;
    
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        explosionAnimSpeed = new WaitForSeconds(animSpeed);
    }

    public IEnumerator PlayExplosionAnim()
    {
        int sprNum = 0;
        while(sprNum < explosionSpr.Length)
        {
            spriteRenderer.sprite = explosionSpr[sprNum];
            yield return explosionAnimSpeed;

            sprNum++;
        }
        spriteRenderer.sprite = defaultSpr;
        
        yield return null;
    }
}
