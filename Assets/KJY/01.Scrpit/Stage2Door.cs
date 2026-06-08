using System.Collections;
using UnityEngine;

public enum Stage2DoorType
{
    Left,
    Right
}

public class Stage2Door : MonoBehaviour
{
    [Header("¹® È¸Àü")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private Vector3 openEuler = new Vector3(0f, 90f, 0f);
    [SerializeField] private float openDuration = 0.5f;

    private Quaternion originalRotation;
    private bool isOpen;
    private bool isOpening;

    private void Awake()
    {
        if (doorPivot == null)
        {
            doorPivot = transform;
        }

        originalRotation = doorPivot.localRotation;
    }

    public void OpenDoor()
    {
        if (isOpen || isOpening)
        {
            return;
        }

        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        isOpening = true;

        Quaternion startRot = doorPivot.localRotation;
        Quaternion endRot = Quaternion.Euler(openEuler);

        float timer = 0f;

        while (timer < openDuration)
        {
            timer += Time.deltaTime;
            float t = timer / openDuration;

            doorPivot.localRotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        doorPivot.localRotation = endRot;
        isOpen = true;
        isOpening = false;
    }

    public void HideDoor()
    {
        gameObject.SetActive(false);
    }

    public void ResetDoor()
    {
        gameObject.SetActive(true);

        if (doorPivot == null)
        {
            doorPivot = transform;
        }

        doorPivot.localRotation = originalRotation;
        isOpen = false;
        isOpening = false;
    }
}
