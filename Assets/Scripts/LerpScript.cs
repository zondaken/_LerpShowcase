using System;
using UnityEditor.UIElements;
using UnityEngine;

public class LerpScript : MonoBehaviour
{
    public enum LerpType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        AnimationCurve
    }

    [SerializeField] public LerpType type;
    [SerializeField] public float duration = 2f;
    [SerializeField] public float distance = 5f;
    [SerializeField] public bool lerpUnclamped = true;
    [SerializeField] private AnimationCurve animationCurve;

    private Vector3 _initialPosition;
    private Vector3 _finalPosition;

    private Func<Vector3, Vector3, float, Vector3> VecLerp => lerpUnclamped ? Vector3.LerpUnclamped : Vector3.Lerp;
    private Func<float, float, float, float> FloatLerp => lerpUnclamped ? Mathf.LerpUnclamped : Mathf.Lerp;
    
    private void Awake()
    {
        _initialPosition = transform.position;
        _finalPosition = _initialPosition + Vector3.right * distance;
    }

    private void Update()
    {
        var t = (Time.time % duration) / duration;

        t = type switch
        {
            LerpType.Linear => Linear(t),
            LerpType.EaseIn => EaseIn(t),
            LerpType.EaseOut => EaseOut(t),
            LerpType.EaseInOut => EaseInOut(t),
            LerpType.AnimationCurve => animationCurve.Evaluate(t),
            _ => throw new ArgumentOutOfRangeException()
        };

        
        transform.position = VecLerp(_initialPosition, _finalPosition, t);
    }

    private float Linear(float t) => t;
    private float Flip(float t) => 1 - t;
    private float EaseIn(float t) => t * t;
    private float EaseOut(float t) => Flip(EaseIn(Flip(t)));
    private float EaseInOut(float t) => FloatLerp(EaseIn(t), EaseOut(t), t);
}
