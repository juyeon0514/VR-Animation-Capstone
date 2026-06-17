using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Transform slotParent;
    public GameObject itemSlotPrefab;

    [Header("UI Reference")]
    public GameObject inventoryPanel;    // I 키로 열고 닫을 패널
    public Image hudItemIcon;            // 화면 구석에 상시 표시될 아이콘
    public TextMeshProUGUI hudItemName;  // 화면 구석에 상시 표시될 아이템 이름

    public Item equippedItem;
    public GameObject currentHandObject;
    public Transform handAnchor;

    public GameObject passwordUI;

    private bool isInvOpen = false;

    void Start()
    {
        // 처음에는 인벤토리를 닫아둡니다.
        inventoryPanel.SetActive(false);
        UpdateHUD(); // 초기 HUD 업데이트
    }

    void Update()
    {
        // 1. I키를 누르면 인벤토리 토글
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInvOpen = !isInvOpen;
        inventoryPanel.SetActive(isInvOpen);

        if (isInvOpen)
        {
            // 인벤토리 열림: 마우스 커서 활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 인벤토리 닫힘: 마우스 커서 다시 고정
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        UpdateUI();
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            if (equippedItem == item) UnequipItem();
            items.Remove(item);
            UpdateUI();
        }
    }

    public void UnequipItem()
    {
        if (currentHandObject != null) Destroy(currentHandObject);
        equippedItem = null;
        ApplyItemSpecialEffect("");
        UpdateHUD(); // HUD 초기화
    }

    // [중요] 더블클릭 스크립트에서 이 함수를 호출하게 됩니다.
    public void EquipItem(Item item)
    {
        if (currentHandObject != null) Destroy(currentHandObject);

        SoundManager.Instance.PlaySFX(SFXType.ItemEquip);
        equippedItem = item;

        if (item.prefab != null)
        {
            currentHandObject = Instantiate(item.prefab, handAnchor);
            currentHandObject.transform.localPosition = Vector3.zero;
        }

        ApplyItemSpecialEffect(item.itemName);
        UpdateHUD(); // 장착 아이템 정보를 HUD에 표시
    }

    // 장착된 아이템 정보를 화면 구석 HUD에 업데이트
    void UpdateHUD()
    {
        if (equippedItem != null)
        {
            hudItemIcon.sprite = equippedItem.icon;
            hudItemName.text = equippedItem.itemName;
            hudItemIcon.enabled = true;
        }
        else
        {
            hudItemName.text = "None";
            hudItemIcon.enabled = false; // 장착 해제 시 아이콘 숨김
        }
    }

    void UpdateUI()
    {
        foreach (Transform child in slotParent) Destroy(child.gameObject);

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, slotParent);

            // 슬롯 스크립트에 정보 전달 (더블클릭 감지용)
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            if (slotScript != null)
            {
                slotScript.item = item;
                slotScript.iconImage.sprite = item.icon;
                slotScript.inventory = this;
            }
        }
    }

    void ApplyItemSpecialEffect(string itemName)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Secret"));
        if (itemName == "손전등")
        {
            Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Secret"));
            Light lightComp = currentHandObject.GetComponentInChildren<Light>();
            if (lightComp != null) lightComp.enabled = true;
        }
    }

    public void OpenPasswordUI()
    {
        if (passwordUI != null)
        {
            passwordUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("Inventory에 Password UI가 등록되지 않았습니다!");
        }
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
}
