using UnityEngine;
using System.Collections;

public class DelayOperation : MonoBehaviour
{
    public delegate void Callback();

    public void StartDelayOperation(Callback callback, float delay)
    {
        StartCoroutine(Delay(callback, delay));
    }

    IEnumerator Delay(Callback callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback.Invoke();
    }
}
