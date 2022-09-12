using System;
using UnityEngine;

public abstract class PooledMonoBehaviour : MonoBehaviour
{
    private Action onDeactivate;

    public void Activate(PlayerEnum newOwner, Vector3 newVelocity, Action onDeactivateCallback)
    {
        Activate(newOwner, newVelocity);
        if (onDeactivateCallback != null) onDeactivate = onDeactivateCallback;
    }

    protected abstract void Activate(PlayerEnum newOwner, Vector3 newVelocity);

    protected void Deactivate()
    {
        onDeactivate.Invoke();
    }
}