using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

/*
    Passes calling rotation observer, the target rotation index, and the rotation.
*/
[System.Serializable] public class TargetHitEvent : UnityEvent<RotationObserver, int, Vector3> { }
[System.Serializable] public class DisplaySummaryEvent : UnityEvent<RotationObserver> { }

public class RotationObserver : MonoBehaviour
{
    Dictionary<int, int> targetRotationHits;

    public List<RotationTarget> TargetRotations;
    public TargetHitEvent OnTargetHit;
    public DisplaySummaryEvent OnDisplaySummary;

    void Start()
    {
        targetRotationHits = new Dictionary<int, int>();
    }

    public int GetTargetHits(int index)
    {
        if (targetRotationHits.ContainsKey(index))
            return targetRotationHits[index];

        return 0;
    }

    public void OnRotationVisualized(Rotator rotator, Vector3 rotation)
    {
        // Check if rotation within range.
        for (int i = 0; i < TargetRotations.Count; i++)
        {
            RotationTarget rt = TargetRotations[i];

            if (rt.targetX && !(rotation.x >= rt.xMin && rotation.x <= rt.xMax))
            {
                continue;
            }

            if (rt.targetY && !(rotation.y >= rt.yMin && rotation.y <= rt.yMax))
            {
                continue;
            }

            if (rt.targetZ && !(rotation.z >= rt.zMin && rotation.z <= rt.zMax))
            {
                continue;
            }

            // Record target hits.
            if (targetRotationHits.ContainsKey(i))
            {
                targetRotationHits[i] += 1;
            }
            else
            {
                targetRotationHits.Add(i, 1);
            }

            rotator.FreezeFrame();

            OnTargetHit.Invoke(this, i, rotation);
        }
    }

    public void OnVisualizationComplete()
    {
        // Display the summary.
        OnDisplaySummary.Invoke(this);
    }
}
