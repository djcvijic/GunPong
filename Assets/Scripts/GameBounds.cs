using UnityEngine;

public class GameBounds : MonoBehaviour
{
    [SerializeField] private bool constrainX;

    [SerializeField, Range(0f, 1f)] private float paddingFactorX;
    
    [SerializeField] private bool constrainY;

    [SerializeField, Range(0f, 1f)] private float paddingFactorY;
    
    [SerializeField] private bool constrainZ;

    [SerializeField, Range(0f, 1f)] private float paddingFactorZ;

    public bool KeepInBounds(ref Vector3 position, Vector3 scale)
    {
        var result = IsLeavingBounds(position, scale, out var clampedPosition);
        if (constrainX) position.x = clampedPosition.x;
        if (constrainY) position.y = clampedPosition.y;
        if (constrainZ) position.z = clampedPosition.z;
        return result;
    }

    private bool IsLeavingBounds(Vector3 position, Vector3 scale, out Vector3 clampedPosition)
    {
        var t = transform;
        var boundsPosition = t.position;
        var boundsScale = t.localScale;
        var result = false;
        clampedPosition = position;

        var minX = boundsPosition.x - 0.5f * (1f - paddingFactorX) * boundsScale.x + 0.5f * scale.x;
        var maxX = boundsPosition.x + 0.5f * (1f - paddingFactorX) * boundsScale.x - 0.5f * scale.x;
        var clampedX = Mathf.Clamp(position.x, minX, maxX);
        if (!clampedX.Approximately(position.x))
        {
            clampedPosition.x = clampedX;
            result = true;
        }
        
        var minY = boundsPosition.y - 0.5f * (1f - paddingFactorY) * boundsScale.y + 0.5f * scale.y;
        var maxY = boundsPosition.y + 0.5f * (1f - paddingFactorY) * boundsScale.y - 0.5f * scale.y;
        var clampedY = Mathf.Clamp(position.y, minY, maxY);
        if (!clampedY.Approximately(position.y))
        {
            clampedPosition.y = clampedY;
            result = true;
        }
        
        var minZ = boundsPosition.z - 0.5f * (1f - paddingFactorZ) * boundsScale.z + 0.5f * scale.z;
        var maxZ = boundsPosition.z + 0.5f * (1f - paddingFactorZ) * boundsScale.z - 0.5f * scale.z;
        var clampedZ = Mathf.Clamp(position.z, minZ, maxZ);
        if (!clampedZ.Approximately(position.z))
        {
            clampedPosition.z = clampedZ;
            result = true;
        }

        return result;
    }

    public bool IsOutOfBounds(Vector3 position, Vector3 scale)
    {
        var t = transform;
        var boundsPosition = t.position;
        var boundsScale = t.localScale;
        var result = false;

        if (constrainX)
        {
            var minX = boundsPosition.x - 0.5f * boundsScale.x - 0.5f * scale.x;
            var maxX = boundsPosition.x + 0.5f * boundsScale.x + 0.5f * scale.x;
            var clampedX = Mathf.Clamp(position.x, minX, maxX);
            if (!clampedX.Approximately(position.x))
            {
                result = true;
            }
        }

        if (constrainY)
        {
            var minY = boundsPosition.y - 0.5f * boundsScale.y - 0.5f * scale.y;
            var maxY = boundsPosition.y + 0.5f * boundsScale.y + 0.5f * scale.y;
            var clampedY = Mathf.Clamp(position.y, minY, maxY);
            if (!clampedY.Approximately(position.y))
            {
                result = true;
            }
        }

        if (constrainZ)
        {
            var minZ = boundsPosition.z - 0.5f * boundsScale.z - 0.5f * scale.z;
            var maxZ = boundsPosition.z + 0.5f * boundsScale.z + 0.5f * scale.z;
            var clampedZ = Mathf.Clamp(position.z, minZ, maxZ);
            if (!clampedZ.Approximately(position.z))
            {
                result = true;
            }
        }

        return result;
    }
}
