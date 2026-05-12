using TMPro;
using UnityEngine;

public class DialLock : MonoBehaviour
{
    [Header("Password Settings")]
    public string correctPassword = "1234";
    public TextMeshProUGUI[] digitTexts;
    private int[] currentDigits;

    [Header("Reward & Inventory")]
    public Item rewardKey;
    public Inventory inventory;

    [Header("Box Animation Settings")]
    public Transform boxLid; 
    public float openAngle = -90f; 
    public float openSpeed = 3f;

    // 내부 상태 변수
    private bool isSuccess = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        currentDigits = new int[digitTexts.Length];
        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentDigits[i] = 0;
            UpdateUI(i);
        }

        if (boxLid != null)
        {
            closedRotation = boxLid.localRotation;
            openRotation = Quaternion.Euler(openAngle, 0, 0) * closedRotation;
        }
    }

    void Update()
    {
        if (isSuccess && boxLid != null)
        {
            boxLid.localRotation = Quaternion.Lerp(boxLid.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    public void ClickUp(int index)
    {
        currentDigits[index]++;
        if (currentDigits[index] > 9) currentDigits[index] = 0;
        UpdateUI(index);
    }

    public void ClickDown(int index)
    {
        currentDigits[index]--;
        if (currentDigits[index] < 0) currentDigits[index] = 9;
        UpdateUI(index);
    }

    void UpdateUI(int index)
    {
        digitTexts[index].text = currentDigits[index].ToString();
    }

    public void OnClickEnterButton()
    {
        string currentInput = "";
        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentInput += currentDigits[i].ToString();
        }

        if (currentInput == correctPassword)
        {
            Success();
        }
        else
        {
            Debug.Log("비밀번호가 틀렸습니다!");
        }
    }

    void Success()
    {
        Debug.Log("비밀번호 일치! 상자가 열립니다.");

        isSuccess = true;

        // 2. 인벤토리에 아이템 지급
        if (rewardKey != null && inventory != null)
        {
            inventory.AddItem(rewardKey);
        }

        Invoke("CloseUI", 2f);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
