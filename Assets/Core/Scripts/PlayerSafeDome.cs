using UnityEngine;
using System.Collections;

public class PlayerSafeDome : MonoBehaviour
{
    public bool IsActive { get; private set; }

    private Coroutine routine;

    public void Activate(float duration)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(DomeRoutine(duration));
    }

    private IEnumerator DomeRoutine(float duration)
    {
        IsActive = true;
        yield return new WaitForSeconds(duration);
        IsActive = false;
        routine = null;
    }
}