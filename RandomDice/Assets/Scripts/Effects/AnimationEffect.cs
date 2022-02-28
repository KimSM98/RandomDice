using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Animation.Effects
{
    public class AnimationEffect
    {
        public static IEnumerator FadeOut(SpriteRenderer rendererToApply, float duration = 1f)
        {
            float t = 0f;

            while (t / duration < 1f)
            {
                Color colorToChangeAlpha = rendererToApply.color;
                
                colorToChangeAlpha.a = Mathf.Lerp(1f, 0f, t / duration);
                t += Time.deltaTime;

                rendererToApply.color = colorToChangeAlpha;

                yield return null;
            }
        }

        public static IEnumerator FadeOut(TextMeshProUGUI tmpColor, float duration = 1f)
        {
            float t = 0f;

            while (t / duration < 1f)
            {
                Color colorToChangeAlpha = tmpColor.color;
                
                colorToChangeAlpha.a = Mathf.Lerp(1f, 0f, t / duration);
                t += Time.deltaTime;

                tmpColor.color = colorToChangeAlpha;

                yield return null;
            }
        }

        // 커졌다가 다시 작아지는 효과
        public static IEnumerator Pulse(Transform transform, Vector3 scaleUpTo, float duration = 1f)
        {
            Vector3 originScale = transform.localScale;
            duration /= 2f;

            float t = 0f;
            while (t / duration < 1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, scaleUpTo, t / duration);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while (t / duration < 1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, originScale, t / duration);
                t += Time.deltaTime;
                yield return null;
            }

            // Reset scale
            transform.localScale = originScale;
        }

        public static IEnumerator GatheringObjects(List<GameObject> objectList, Vector2 gatheringPos, float duration)
        {
            float t = 0f;
            while (t / duration < 1f)
            {
                t += Time.deltaTime;

                foreach (GameObject enemy in objectList)
                {
                    enemy.transform.position = Vector3.Lerp(enemy.transform.position, gatheringPos, t / duration);
                }

                yield return null;
            }
        }
    }
}