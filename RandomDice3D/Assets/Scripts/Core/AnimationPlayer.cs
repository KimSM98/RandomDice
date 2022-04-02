using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation.Core;

public class AnimationPlayer : MonoBehaviour
{
    // Scriptable object인 AnimationType을 넣어야 한다.
    public AnimationType animType;
    
    private SpriteRenderer spriteRenderer;
    private Sprite[] animSprites;
    private Sprite initSprite;

    private float fps;
    WaitForSeconds frameRate;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initSprite = spriteRenderer.sprite;

        SetAnimationType(animType);
    }

    public IEnumerator PlayAnimation()
    {
        int currentSprNum = 0;

        while(currentSprNum < animSprites.Length)
        {
            spriteRenderer.sprite = animSprites[currentSprNum];
            currentSprNum++;

            yield return frameRate;
        }
    }

    public void SetAnimationType(AnimationType animType)
    {
        if (animType == null)
        {
            Debug.Log("Animation type is null.");
            return;
        }    

        animSprites = animType.sprites;
        fps = animType.fps;

        frameRate = new WaitForSeconds(1f / fps);
    }

    public void InitSprite()
    {
        spriteRenderer.sprite = initSprite;
    }

}
