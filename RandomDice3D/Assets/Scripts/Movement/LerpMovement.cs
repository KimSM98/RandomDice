using System.Collections;
using UnityEngine;
using System;

public class LerpMovement : MonoBehaviour
{
    #region Movement Type
    public enum MovementType
    {
        None,
        UniformMotion,
        DeceleratedMotion
    }
    [SerializeField]
    private MovementType movementType = MovementType.None;
    #endregion

    #region Constant speed
    [SerializeField]
    [Tooltip("It is only used at the begining.")]
    private bool isMovingConstantSpeed = false;
    private delegate float UsingTimeWeightForConstatntSpeed(Vector2 moveTo, float speed);
    private UsingTimeWeightForConstatntSpeed usingConstantSpeed; 
    #endregion

    private Action<Transform, float, float> lerpMoving;

    protected bool isMovementActive = true;
    private float timeElapsed;
    private Vector2 startPos;

    private void Awake()
    {
        SetMovementType(movementType);

        if (isMovingConstantSpeed)
            UseTimeWeightForConstantSpeed();
    }

    #region Movement
    public IEnumerator MoveLerp(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        if (movementType == MovementType.None) yield break;

        timeElapsed = 0f;
        startPos = transform.position;
        float speed = usingConstantSpeed?.Invoke(endPoint.transform.position, speedWeight) ?? speedWeight;

        while (timeElapsed / lerpDuration < 1f && isMovementActive)
        {
            timeElapsed += speed * Time.deltaTime;
            lerpMoving?.Invoke(endPoint, lerpDuration, speedWeight);

            yield return null;
        }
    }

    public void UseUnifomMotion()
    {
        lerpMoving = UniformMotion;
    }

    public void UseDeceleratedMotion()
    {
        lerpMoving = DeceleratedMotion;
    }

    // 등속 운동
    public void UniformMotion(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        transform.position = Vector2.Lerp(startPos, endPoint.position, timeElapsed / lerpDuration);
    }

    // 감속 운동
    public void DeceleratedMotion(Transform endPoint, float lerpDuration = 1f, float speedWeight = 1f)
    {
        transform.position = Vector2.Lerp(transform.position, endPoint.position, timeElapsed / lerpDuration);
    } 
    #endregion

    #region Moving Constatnt Speed
    // 서로 다른 거리를 같은 속도로 이동하기 위해 사용한다.
    public void UseTimeWeightForConstantSpeed()
    {
        usingConstantSpeed = CalculateTimeWeight;
    }

    // 같은 시간 동안 
    private float CalculateTimeWeight(Vector2 moveTo, float moveSpeed)
    {
        float distanceBetweenRoads = Vector2.Distance(transform.position, moveTo);

        return moveSpeed / distanceBetweenRoads;
    }
    #endregion

    #region Setter
    public void SetMovementType(MovementType type)
    {
        switch (type)
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
    public void SetMoving(bool condition)
    {
        isMovementActive = condition;
    } 

    public bool GetMovementCondition()
    {
        return isMovementActive;
    }
    #endregion
}
