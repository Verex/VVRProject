using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class RotationVisualizedEvent : UnityEvent<Rotator, Vector3> {}

public class Rotator : MonoBehaviour
{
    bool freezeRotation = false;
    [SerializeField] Transform rotationTarget;
    [SerializeField] float rotationDuration = 5;

    public RotationVisualizedEvent OnRotationVisualized;
    public UnityEvent OnVisualizationComplete;

    /*
        Interpolates rotation of target over specified duration (seconds).
    */
    IEnumerator RotateTargetToAngle(Vector3 rotation, float duration)
    {
        // Get initial and target rotations.
        Quaternion initialRotation = rotationTarget.rotation,
            targetRotation = Quaternion.Euler(rotation);

        float remainingTime = duration;

        // Spherical interpolation over duration.
        while (remainingTime > 0)
        {
            rotationTarget.rotation = Quaternion.Slerp(initialRotation, targetRotation, 1 - (remainingTime / duration));
            remainingTime -= Time.deltaTime;

            yield return null;
        }

        yield break;
    }

    IEnumerator VisualizeRotations(List<Vector3> rotations)
    {
        foreach(Vector3 rotation in rotations)
        {
            yield return StartCoroutine(RotateTargetToAngle(rotation, rotationDuration));

            OnRotationVisualized.Invoke(this, rotation);

            if (freezeRotation)
            {
                freezeRotation = false;
                yield return new WaitForSeconds(Globals.Instance.freezeDuration);
            }
        }

        OnVisualizationComplete.Invoke();

        yield break;
    }

    public void FreezeFrame()
    {
        freezeRotation = true;
    }

    public void OnRotationsLoaded(List<Vector3> rotations)
    {
        // Start the visualization of rotations.
        StartCoroutine(VisualizeRotations(rotations));
    }
}
