using System;
using UnityEngine;

public class GameBounds : MonoBehaviour
{
    [SerializeField] private bool constrainX;

    [SerializeField, Range(0f, 1f)] private float paddingFactorX;
    
    [SerializeField] private bool constrainY;

    [SerializeField, Range(0f, 1f)] private float paddingFactorY;
    
    [SerializeField] private bool constrainZ;

    [SerializeField, Range(0f, 1f)] private float paddingFactorZ;

    private Vector3 position;

    private Vector3 scale;

    private void Start()
    {
        var t = transform;
        position = t.position;
        scale = t.lossyScale;
    }

    public bool KeepInBounds(ref Vector3 otherPosition, Vector3 otherScale)
    {
        var result = false;

        if (constrainX)
        {
            var minX = position.x - 0.5f * (1f - paddingFactorX) * scale.x + 0.5f * otherScale.x;
            var maxX = position.x + 0.5f * (1f - paddingFactorX) * scale.x - 0.5f * otherScale.x;
            var clampedX = Mathf.Clamp(otherPosition.x, minX, maxX);
            if (Math.Abs(clampedX - otherPosition.x) > float.Epsilon)
            {
                otherPosition.x = clampedX;
                result = true;
            }
        }

        if (constrainY)
        {
            var minY = position.y - 0.5f * (1f - paddingFactorY) * scale.y + 0.5f * otherScale.y;
            var maxY = position.y + 0.5f * (1f - paddingFactorY) * scale.y - 0.5f * otherScale.y;
            var clampedY = Mathf.Clamp(otherPosition.y, minY, maxY);
            if (Math.Abs(clampedY - otherPosition.y) > float.Epsilon)
            {
                otherPosition.y = clampedY;
                result = true;
            }
        }

        if (constrainZ)
        {
            var minZ = position.z - 0.5f * (1f - paddingFactorZ) * scale.z + 0.5f * otherScale.z;
            var maxZ = position.z + 0.5f * (1f - paddingFactorZ) * scale.z - 0.5f * otherScale.z;
            var clampedZ = Mathf.Clamp(otherPosition.z, minZ, maxZ);
            if (Math.Abs(clampedZ - otherPosition.z) > float.Epsilon)
            {
                otherPosition.z = clampedZ;
                result = true;
            }
        }

        return result;
    }
}
