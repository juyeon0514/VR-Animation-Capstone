using UnityEngine;

public class MirrorScrewManager : MonoBehaviour
{
    [SerializeField] private MirrorFall mirrorFall;
    [SerializeField] private int requiredScrewCount = 4;

    private int removedScrewCount;
    private bool mirrorDropped;

    public void AddRemovedScrew()
    {
        if (mirrorDropped)
        {
            return;
        }

        removedScrewCount++;

        Debug.Log($"³ª»ç Á¦°ÅµÊ: {removedScrewCount} / {requiredScrewCount}");

        if (removedScrewCount >= requiredScrewCount)
        {
            mirrorDropped = true;

            if (mirrorFall != null)
            {
                Debug.Log("Fall Down");
                mirrorFall.Fall();
            }
        }
    }

}
