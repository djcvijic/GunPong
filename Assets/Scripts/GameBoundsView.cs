using UnityEngine;

public class GameBoundsView : MonoBehaviour
{
    [SerializeField] private bool constrainX;

    [SerializeField, Range(0f, 1f)] private float paddingFactorX;
    
    [SerializeField] private bool constrainY;

    [SerializeField, Range(0f, 1f)] private float paddingFactorY;
    
    [SerializeField] private bool constrainZ;

    [SerializeField, Range(0f, 1f)] private float paddingFactorZ;

    public bool KeepInBoundsPadded(Vector3 position, Vector3 scale, out Vector3 newPosition)
    {
        var result = IsLeavingBounds(transform, position, scale,
            paddingFactorX, paddingFactorY, paddingFactorZ, out var clampedPosition, out _);
        newPosition = position;
        if (constrainX) newPosition.x = clampedPosition.x;
        if (constrainY) newPosition.y = clampedPosition.y;
        if (constrainZ) newPosition.z = clampedPosition.z;
        return result != GameBoundsEdge.None;
    }

    public GameBoundsEdge Reflect(Vector3 position, Vector3 scale, out Vector3 newPosition)
    {
        var result = IsLeavingBounds(transform, position, scale,
            paddingFactorX, paddingFactorY, paddingFactorZ, out _, out var reflectedPosition);
        newPosition = position;
        if (constrainX) newPosition.x = reflectedPosition.x;
        if (constrainY) newPosition.y = reflectedPosition.y;
        if (constrainZ) newPosition.z = reflectedPosition.z;
        return result;
    }

    private static GameBoundsEdge IsLeavingBounds(Transform bounds, Vector3 position, Vector3 scale,
        float padX, float padY, float padZ, out Vector3 clampedPosition, out Vector3 reflectedPosition)
    {
        var boundsPosition = bounds.position;
        var boundsScale = bounds.localScale;
        var result = GameBoundsEdge.None;
        clampedPosition = position;
        reflectedPosition = position;

        var minX = boundsPosition.x - 0.5f * (1f - padX) * boundsScale.x + 0.5f * scale.x;
        if (position.x < minX)
        {
            clampedPosition.x = minX;
            reflectedPosition.x = 2f * minX - position.x;
            result = GameBoundsEdge.Left;
        }
        else
        {
            var maxX = boundsPosition.x + 0.5f * (1f - padX) * boundsScale.x - 0.5f * scale.x;
            if (position.x > maxX)
            {
                clampedPosition.x = maxX;
                reflectedPosition.x = 2f * maxX - position.x;
                result = GameBoundsEdge.Right;
            }
        }
        
        var minY = boundsPosition.y - 0.5f * (1f - padY) * boundsScale.y + 0.5f * scale.y;
        if (position.y < minY)
        {
            clampedPosition.y = minY;
            reflectedPosition.y = 2f * minY - position.y;
            result = GameBoundsEdge.Bottom;
        }
        else
        {
            var maxY = boundsPosition.y + 0.5f * (1f - padY) * boundsScale.y - 0.5f * scale.y;
            if (position.y > maxY)
            {
                clampedPosition.y = maxY;
                reflectedPosition.y = 2f * maxY - position.y;
                result = GameBoundsEdge.Top;
            }
        }
        
        var minZ = boundsPosition.z - 0.5f * (1f - padZ) * boundsScale.z + 0.5f * scale.z;
        if (position.z < minZ)
        {
            clampedPosition.z = minZ;
            reflectedPosition.z = 2f * minZ - position.z;
            result = GameBoundsEdge.Rear;
        }
        else
        {
            var maxZ = boundsPosition.z + 0.5f * (1f - padZ) * boundsScale.z - 0.5f * scale.z;
            if (position.z > maxZ)
            {
                clampedPosition.z = maxZ;
                reflectedPosition.z = 2f * maxZ - position.z;
                result = GameBoundsEdge.Front;
            }
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
