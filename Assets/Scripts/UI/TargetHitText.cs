using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetHitText : MonoBehaviour
{
    [SerializeField] Text targetText;

    IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject.Destroy(this.gameObject);

        yield break;
    }

    public void DisplayTargetHit(RotationTarget rt, Vector3 rotation, int totalHits, float displayTime)
    {  
        targetText.text += rotation.ToString() + " hit target " + rt.ToString() + " (Total Hits: " + totalHits.ToString() + ")";

        StartCoroutine(DelayedDestroy(displayTime));
    }
}
