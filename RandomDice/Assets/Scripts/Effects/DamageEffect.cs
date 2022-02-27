using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Animation.Effects;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI damageText;
    private Color initColor;
    
    // Size pumping
    [SerializeField]
    private Vector3 pulseScale = new Vector3(1.1f, 1.1f, 1f);
    private Vector3 originScale;
    [SerializeField]
    private float pulseDuration = 1f;


    private void Start()
    {
        initColor = damageText.color;
        originScale = transform.localScale;
    }

    public void DisplayDamage(float damage)
    {
        SetDamage(damage);
        StartCoroutine(DamageEffectDisplayer());
    }

    IEnumerator DamageEffectDisplayer()
    {
        yield return StartCoroutine(AnimationEffect.Pulse(transform, pulseScale, pulseDuration));
        yield return StartCoroutine(AnimationEffect.FadeOut(damageText));

        gameObject.SetActive(false);
        ResetObject();
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
