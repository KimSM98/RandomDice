using System.Collections;
using UnityEngine;
using System;

public class LerpMovement : MonoBehaviour
{
    public enum MovementType
    {
        None,
        UniformMotion,
        DeceleratedMotion
    }
    MovementType movementType = MovementType.None;

    protected bool isMovementActive = true;

    Action<Transform, float, float> lerpMoving;
    private delegate float UsingRatioOfDistance(Vector2 moveTo, float speed);
    UsingRatioOfDistance usingRatioOfDistance;

    float timeElapsed;
    Vector2 startPos;

    public IEnumerator MoveLerp(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        if (movementType == MovementType.None) yield break;

        timeElapsed = 0f;
        startPos = transform.position;

        while (timeElapsed / lerpDuration < 1f && isMovementActive)
        {
            speedWeight = usingRatioOfDistance?.Invoke(endPoint.transform.position, speedWeight) ?? speedWeight;
            timeElapsed += speedWeight * Time.deltaTime;

            lerpMoving?.Invoke(endPoint, lerpDuration, speedWeight);
            yield return null;
        }
    }

    public void SetMovementType(MovementType type)
    {
        switch(type)
        {
            case MovementType.UniformMotion:
                movementType = MovementType.UniformMotion;
                UseUnifomMotion();
                break;

            case MovementType.DeceleratedMotion:
                movementType = MovementType.DeceleratedMotion;
                UseDeceleratedMotion();
                break;

            default:
                movementType = MovementType.None;
                break;
        }
    }

    public void UseUnifomMotion()
    {
        lerpMoving = mUniformMotion;
    }

    public void UseDeceleratedMotion()
    {
        lerpMoving = mDeceleratedMotion;
    }

    public void UseRatioOfDistanceToSpeedWeight()
    {
        usingRatioOfDistance = CalculateRatioOfDistance;
    }

    public void mUniformMotion(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        transform.position = Vector2.Lerp(startPos, endPoint.position, timeElapsed / lerpDuration);
    }

    public void mDeceleratedMotion(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        transform.position = Vector2.Lerp(transform.position, endPoint.position, timeElapsed / lerpDuration);
    }

    // 등속 운동
    public IEnumerator UniformMotion(Vector2 startPos, Vector2 endPos, float lerpDuration = 1f, float speedWeight = 1f)
    {
        float timeElapsed = 0f;

        while (timeElapsed / lerpDuration < 1f && isMovementActive)
        {
            timeElapsed += speedWeight * Time.deltaTime;

            transform.position = Vector2.Lerp(startPos, endPos, timeElapsed / lerpDuration);
            yield return null;
        }
    }

    // 감속 운동
    public IEnumerator DeceleratedMotion(Vector2 endPos, float lerpDuration = 1f, float speedWeight = 1f)
    {
        float timeElapsed = 0f;

        while (timeElapsed / lerpDuration < 1f && isMovementActive)
        {
            timeElapsed += speedWeight * Time.deltaTime;

            transform.position = Vector2.Lerp(transform.position, endPos, timeElapsed / lerpDuration);
            yield return null;
        }
    }

    // 거리에 상관없이 같은 속도로 이동한다.
    private float CalculateRatioOfDistance(Vector2 moveTo, float moveSpeed)
    {
        float distanceBetweenRoads = Vector2.Distance(transform.position, moveTo);

        return moveSpeed / distanceBetweenRoads;
    }

    public void SetMoving(bool condition)
    {
        isMovementActive = condition;
    }
}
