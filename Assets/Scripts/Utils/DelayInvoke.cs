using UnityEngine;
using System.Collections;
using System;

public class DelayInvoke
{

    public static IEnumerator DelayInvokeDo(Action action, float delaySeconds)
    {
        if (action == null)
            yield break;
        yield return new WaitForSeconds(delaySeconds);
        action?.Invoke();
    }
}