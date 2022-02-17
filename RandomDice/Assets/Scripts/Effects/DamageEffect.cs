using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI damageText;
    private Color initColor;
    
    // Size pumping
    [SerializeField]
    private Vector3 sizeUpScale = new Vector3(1.1f, 1.1f, 1f);
    private Vector3 originScale;
    [SerializeField]
    private float sizeUpSpeed = 4f;


    private void Start()
    {
        initColor = damageText.color;
        originScale = transform.localScale;
        //displayWait = new WaitForSeconds(displayDuration);
    }

    public void DisplayDamage(float damage)
    {
        SetDamage(damage);
        StartCoroutine(DamageEffectDisplayer());
    }

    IEnumerator DamageEffectDisplayer()
    {
        yield return StartCoroutine(SizePumping());
        yield return StartCoroutine(FadeOut());

        gameObject.SetActive(false);
        ResetObject();
    }

    IEnumerator SizePumping()
    {
        float t = 0f;
        while(t < 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, sizeUpScale, t);
            t += Time.deltaTime * sizeUpSpeed;
            yield return null;
        }

        // Reset scale
        transform.localScale = originScale;
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color colorToChangeAlpha = initColor;

        while (t < 1f)
        {
            colorToChangeAlpha.a = Mathf.Lerp(1f, 0f, t += Time.deltaTime);
            damageText.color = colorToChangeAlpha;

            yield return null;
        }
    }

    private void SetDamage(float damage)
    {
        damageText.text = "<mspace=0.6em>" + damage.ToString() +"</mspace>";
    }

    private void ResetObject()
    {
        ObjectPool.instance.ReturnToPool("DamageEffect", this.gameObject);
        damageText.color = initColor;
        transform.localScale = originScale;
    }
}
