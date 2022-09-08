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

    public Vector3 KeepInBounds(Vector3 otherPosition, Vector3 otherScale)
    {
        var result = otherPosition;

        if (constrainX)
        {
            var minX = position.x - 0.5f * (1f - paddingFactorX) * scale.x + 0.5f * otherScale.x;
            var maxX = position.x + 0.5f * (1f - paddingFactorX) * scale.x - 0.5f * otherScale.x;
            result.x = Mathf.Clamp(result.x, minX, maxX);
        }

        if (constrainY)
        {
            var minY = position.y - 0.5f * (1f - paddingFactorY) * scale.y + 0.5f * otherScale.y;
            var maxY = position.y + 0.5f * (1f - paddingFactorY) * scale.y - 0.5f * otherScale.y;
            result.y = Mathf.Clamp(result.y, minY, maxY);
        }

        if (constrainZ)
        {
            var minZ = position.z - 0.5f * (1f - paddingFactorZ) * scale.z + 0.5f * otherScale.z;
            var maxZ = position.z + 0.5f * (1f - paddingFactorZ) * scale.z - 0.5f * otherScale.z;
            result.z = Mathf.Clamp(result.z, minZ, maxZ);
        }

        return result;
    }
}
