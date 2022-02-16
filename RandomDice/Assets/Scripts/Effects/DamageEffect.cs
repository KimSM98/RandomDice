using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI damageText;
    private Color initColor;
    
    [SerializeField]
    private float displayDuration = 0.1f;
    private WaitForSeconds displayWait;

    private void Start()
    {
        initColor = damageText.color;
        displayWait = new WaitForSeconds(displayDuration);
    }

    public void DisplayDamage(float damage)
    {
        SetDamage(damage);
        StartCoroutine(DamageEffectDisplayer());
    }

    IEnumerator DamageEffectDisplayer()
    {
        yield return displayWait;
        yield return StartCoroutine(FadeOut());

        gameObject.SetActive(false);
        ResetObject();
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
    }
}
