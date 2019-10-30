using UnityEngine;
using UnityEngine.UI;

public class TargetSummary : MonoBehaviour
{
    [SerializeField] Text summaryText;

    public void UpdateSummary(RotationObserver observer)
    {
        for (int i = 0; i < observer.TargetRotations.Count; i++)
        {
            RotationTarget rt = observer.TargetRotations[i];
            summaryText.text += i.ToString() + ": " + rt.ToString() + " was hit " + observer.GetTargetHits(i) + " time[s]\n"; 
        }
    }
}
