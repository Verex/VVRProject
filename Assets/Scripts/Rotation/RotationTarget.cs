using System.Text;
using UnityEngine;

[System.Serializable]
public struct RotationTarget
{
    [SerializeField] public bool targetX, targetY, targetZ;
    [SerializeField]
    public float xMin, xMax,
        yMin, yMax,
        zMin, zMax;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("(");

        if (targetX)
        {
            sb.Append(xMin.ToString() + " - " + xMax.ToString());
        }
        else
        {
            sb.Append("?");
        }

        sb.Append(", ");

        if (targetY)
        {
            sb.Append(yMin.ToString() + " - " + yMax.ToString());
        }
        else
        {
            sb.Append("?");
        }

        sb.Append(", ");

        if (targetZ)
        {
            sb.Append(zMin.ToString() + " - " + zMax.ToString());
        }
        else
        {
            sb.Append("?");
        }

        sb.Append(")");

        return sb.ToString();
    }
}