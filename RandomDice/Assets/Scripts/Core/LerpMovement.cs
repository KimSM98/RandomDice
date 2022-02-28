using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    private bool isMove = true;

    public IEnumerator UniformMotion(Vector2 startPos, Vector2 endPos, float lerpDuration = 1f, float speedWeight = 1f)
    {
        float timeElapsed = 0f;
        isMove = true;

        while (timeElapsed / lerpDuration < 1f && isMove)
        {
            timeElapsed += speedWeight * Time.deltaTime;

            transform.position = Vector2.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            yield return null;
        }
    }

    public IEnumerator AcceleratedMotion(Vector2 endPos, float lerpDuration = 1f, float speedWeight = 1f)
    {
        float timeElapsed = 0f;
        isMove = true;

        while (timeElapsed / lerpDuration < 1f && isMove)
        {
            timeElapsed += speedWeight * Time.deltaTime;

            transform.position = Vector2.Lerp(transform.position, endPos, timeElapsed / lerpDuration);
            yield return null;
        }
    }

    public void SetMoving(bool condition)
    {
        isMove = condition;
    }
}
