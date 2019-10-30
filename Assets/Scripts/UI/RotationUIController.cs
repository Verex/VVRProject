using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public struct HitRecord
{
    public RotationTarget Target { get; private set; }
    public Vector3 Rotation { get; private set; }
    public int TotalHits { get; private set; }

    public HitRecord(RotationTarget rt, Vector3 rotation, int totalHits)
    {
        Target = rt;
        Rotation = rotation;
        TotalHits = totalHits;
    }
}

public class RotationUIController : MonoBehaviour
{
    Queue<HitRecord> hitRecords;
    [SerializeField] Text lastRotationText;
    [SerializeField] GameObject targetHitTextPrefab;
    [SerializeField] GameObject targetSummaryPrefab;

    void Start()
    {
        hitRecords = new Queue<HitRecord>();
    }

    void LateUpdate()
    {
        while (hitRecords.Count > 0)
        {
            HitRecord record = hitRecords.Dequeue();

            GameObject targetHitTextOjbect = Instantiate(targetHitTextPrefab);
            targetHitTextOjbect.transform.SetParent(this.transform, false);

            TargetHitText hitText = targetHitTextOjbect.GetComponent<TargetHitText>();
            hitText.DisplayTargetHit(record.Target, record.Rotation, record.TotalHits, Globals.Instance.freezeDuration);

            RectTransform rt = targetHitTextOjbect.GetComponent<RectTransform>();
            rt.anchoredPosition = rt.anchoredPosition + new Vector2(0, hitRecords.Count * -20);
        }
    }

    public void OnRotationVisualized(Rotator rotator, Vector3 rotation)
    {
        lastRotationText.text = "Last Rotation: " + rotation.ToString();
    }

    public void OnTargetHit(RotationObserver observer, int targetIndex, Vector3 rotation)
    {
        hitRecords.Enqueue(new HitRecord(
             observer.TargetRotations[targetIndex],
             rotation,
             observer.GetTargetHits(targetIndex)
        ));
    }

    public void OnDisplaySummary(RotationObserver observer)
    {
        GameObject summaryObject = Instantiate(targetSummaryPrefab);
        summaryObject.transform.SetParent(this.transform, false);

        summaryObject.GetComponent<TargetSummary>().UpdateSummary(observer);
    }
}
