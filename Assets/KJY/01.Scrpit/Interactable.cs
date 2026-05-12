using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("아이템 획득")]
    public Item itemToGive;       // 플레이어에게 줄 아이템 (ScriptableObject)

    [Header("아이템 사용")]
    public Item requiredItem;     // 상호작용에 필요한 아이템 (ScriptableObject)

    [Header("실행될 사건")]
    public UnityEvent onAction;   // 조건 충족 시 에디터에서 설정한 함수들을 실행

    public void OnInteract(PlayerInteraction player)
    {
        // 1. 필요한 아이템이 있는 경우 (예: 거울에 드라이버 사용)
        if (requiredItem != null)
        {
            // 현재 손에 든 아이템(equippedItem)과 필요한 아이템(requiredItem)이 같은지 비교
            if (player.inventory.equippedItem == requiredItem)
            {
                Debug.Log(requiredItem.itemName + " 사용 성공!");
                onAction.Invoke(); // 에디터에서 연결한 이벤트들(문 열기 등) 실행
            }
            else
            {
                Debug.Log(requiredItem.itemName + "이(가) 필요합니다.");
            }
            return; // 아이템 사용 로직이 우선이므로 종료
        }

        // 2. 아이템을 줍는 경우 (예: 세숫대야에서 드라이버 획득)
        if (itemToGive != null)
        {
            player.inventory.AddItem(itemToGive);
            Debug.Log(itemToGive.itemName + " 획득!");

            // 아이템을 획득하면 필드에서 사라지게 함 (또는 onAction으로 처리 가능)
            gameObject.SetActive(false);
        }
        else
        {
            // 아이템과 상관없는 단순 클릭 상호작용 (예: 그냥 쪽지 읽기 등)
            onAction.Invoke();
        }
    }
}
