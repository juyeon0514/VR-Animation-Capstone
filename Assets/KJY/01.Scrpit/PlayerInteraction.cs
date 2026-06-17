using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Inventory inventory;
    public Transform handPos;

    private Interactable currentInteractable;

    [Header("Ray 확인용")]
    [SerializeField] private bool showRayGizmo = true;
    [SerializeField] private Color rayColor = Color.green;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitSphereSize = 0.08f;

    private RaycastHit lastHit;
    private bool hasHit;

    void Update()
    {
        CheckInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteract(this);
            }
        }
    }

    private void CheckInteractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {

            Interactable obj = hit.transform.GetComponentInParent<Interactable>();

            if (obj != null)
            {
               // Debug.Log("Interactable Found: " + obj.name);
            }
            else
            {
               // Debug.Log("Interactable 없음");
            }

            if (obj != null)
            {
                if (currentInteractable != obj)
                {
                    ClearCurrentInteractable();

                    currentInteractable = obj;
                    currentInteractable.SetHighlight(true);
                }
                return;
            }
        }

        ClearCurrentInteractable();
    }

    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.SetHighlight(false);
            currentInteractable = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showRayGizmo)
        {
            return;
        }

        if (Camera.main == null)
        {
            return;
        }

        Vector3 start = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;
        Vector3 end = start + direction * interactionDistance;

        Gizmos.color = rayColor;
        Gizmos.DrawLine(start, end);

        Gizmos.DrawWireSphere(end, 0.05f);

        if (Application.isPlaying && hasHit)
        {
            Gizmos.color = hitColor;
            Gizmos.DrawSphere(lastHit.point, hitSphereSize);
            Gizmos.DrawLine(start, lastHit.point);
        }
    }
}
