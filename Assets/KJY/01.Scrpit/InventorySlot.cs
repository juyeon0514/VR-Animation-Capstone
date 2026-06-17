using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Item item; // 여기에 ScriptableObject 파일이 할당됨
    public Inventory inventory;
    public Image iconImage; // 슬롯의 아이콘 이미지 컴포넌트


    // 슬롯에 아이템 정보를 셋팅하는 함수
    public void SetItem(Item newItem, Inventory inv)
    {
        item = newItem;
        inventory = inv;
        iconImage.sprite = item.icon; // ScriptableObject에 등록한 아이콘 적용
        iconImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[디버그] 클릭된 아이템: '{item.name}', 클릭 횟수: {eventData.clickCount}");

        if (eventData.clickCount == 2)
        {
            if (item.name == "Box")
            {
                // 2. 이름이 일치하면 이 메시지가 뜹니다.
                Debug.Log("[디버그] 상자 더블클릭 확인! UI 열기 함수를 호출합니다.");
                inventory.OpenPasswordUI();

            }
            else
            {
                Debug.Log("[디버그] 상자가 아니라서 일반 장착을 실행합니다.");
                inventory.EquipItem(item);
            }
        }
        else if (eventData.clickCount == 1)
        {
            inventory.EquipItem(item);
        }
    }
}
