using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Inventory inventory; // 인벤토리 스크립트 연결
    public Transform handPos;   // 손 위치 Transform

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interactable obj = hit.transform.GetComponent<Interactable>();
                if (obj != null)
                {
                    obj.OnInteract(this);
                }
            }
        }
    }
}
