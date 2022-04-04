using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TimeExtensions
{
    public static bool useUnscaledTime = false;
    
    public static bool unscaledTime => Time.timeScale <= 0 && useUnscaledTime;

    public static float deltaTime => !unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
    
    public static float fixedDeltaTime => !unscaledTime ? Time.fixedDeltaTime : Time.fixedUnscaledDeltaTime;
    
    public static float time => !unscaledTime ? Time.time : Time.unscaledTime;

    public static float GetNormalizedTime(this Animator animator, int layer, int round = 2)
    {
        return (float)System.Math.Round(
            ((animator.IsInTransition(layer) 
            ? animator.GetNextAnimatorStateInfo(layer).normalizedTime 
            : animator.GetCurrentAnimatorStateInfo(layer).normalizedTime)) % 1, 
            round);
    }
}
