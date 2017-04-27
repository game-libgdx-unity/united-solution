using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace UnitedSolution
{
    public class LazyBehaviour : MonoBehaviour
    {
        protected Coroutine RunAfter(float time, Action action = null)
        {
            return StartCoroutine(RunAfterCoroutine(time, action));
        }
        private IEnumerator RunAfterCoroutine(float time, Action delayCall = null)
        {
            if(delayCall == null)
            {
                yield break;
            }
            if (time != 0)
            {
                yield return new WaitForSeconds(time);
            }
            delayCall();
        }
        protected void DelayCall(float time, TweenCallback delayCall)
        {
            if (delayCall == null)
            {
                return;
            }
            if (time == 0)
            {
                delayCall();
            }
            DOVirtual.DelayedCall(time, delayCall);
        }
    }
}
