using System.Collections;
using UnityEngine;

public enum Stage2DoorType
{
    Left,
    Right
}

public class Stage2Door : MonoBehaviour
{
    [Header("문 회전")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private Vector3 openEuler = new Vector3(0f, 90f, 0f);
    [SerializeField] private float openDuration = 0.5f;

    private Quaternion originalRotation;
    private Quaternion openedRotation;

    private bool isOpen;
    private bool isOpening;
    private bool isLocked;

    private Coroutine openCoroutine;

    private void Awake()
    {
        if (doorPivot == null)
        {
            doorPivot = transform;
        }

        originalRotation = doorPivot.localRotation;
        openedRotation = originalRotation * Quaternion.Euler(openEuler);
    }

    public void OpenDoor()
    {
        if (isLocked)
        {
            Debug.Log("이 문은 더 이상 열 수 없습니다.");
            return;
        }

        if (isOpen || isOpening)
        {
            return;
        }

        openCoroutine = StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        isOpening = true;

        Quaternion startRot = doorPivot.localRotation;
        Quaternion endRot = openedRotation;

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
        openCoroutine = null;
    }

    public void HideDoor()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        doorPivot.localRotation = originalRotation;

        isOpen = false;
        isOpening = false;
        isLocked = true;
    }

    public void ResetDoor()
    {
        gameObject.SetActive(true);

        if (doorPivot == null)
        {
            doorPivot = transform;
        }

        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        doorPivot.localRotation = originalRotation;

        isOpen = false;
        isOpening = false;
        isLocked = false;
    }
}
