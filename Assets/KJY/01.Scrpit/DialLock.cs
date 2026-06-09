using TMPro;
using UnityEngine;

public class DialLock : MonoBehaviour
{
    [Header("Password Settings")]
    [SerializeField] private TMP_Text passwordHintText;
    [SerializeField] private TMP_Text[] digitTexts;

    private string correctPassword;
    private int[] currentDigits;

    [Header("Reward & Inventory")]
    public Item rewardKey;
    public Inventory inventory;

    [Header("Clear Target")]
    [SerializeField] private GameObject objectToOpenOrDisable;

    private bool isSuccess = false;

    private void OnEnable()
    {
        LoadPasswordFromManager();
        InitDigits();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LoadPasswordFromManager()
    {
        if (PasswordManager.Instance == null)
        {
            Debug.LogError("PasswordManager가 씬에 없습니다.");
            return;
        }

        correctPassword = PasswordManager.Instance.CurrentPassword;

        if (passwordHintText != null)
        {
            passwordHintText.text = correctPassword;
        }

        Debug.Log("자물쇠가 받은 비밀번호: " + correctPassword);
    }

    private void InitDigits()
    {
        currentDigits = new int[digitTexts.Length];

        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentDigits[i] = 0;
            UpdateUI(i);
        }
    }

    public void ClickUp(int index)
    {
        if (isSuccess)
        {
            return;
        }

        currentDigits[index]++;

        if (currentDigits[index] > 9)
        {
            currentDigits[index] = 0;
        }

        UpdateUI(index);
    }

    public void ClickDown(int index)
    {
        if (isSuccess)
        {
            return;
        }

        currentDigits[index]--;

        if (currentDigits[index] < 0)
        {
            currentDigits[index] = 9;
        }

        UpdateUI(index);
    }

    private void UpdateUI(int index)
    {
        if (digitTexts[index] != null)
        {
            digitTexts[index].text = currentDigits[index].ToString();
        }
    }

    public void OnClickEnterButton()
    {
        if (isSuccess)
        {
            return;
        }

        string currentInput = "";

        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentInput += currentDigits[i].ToString();
        }

        Debug.Log("입력한 비밀번호: " + currentInput);
        Debug.Log("정답 비밀번호: " + correctPassword);

        if (currentInput == correctPassword)
        {
            Success();
        }
        else
        {
            Debug.Log("비밀번호가 틀렸습니다!");

            if (InteractionUI.Instance != null)
            {
                InteractionUI.Instance.ShowMessage("비밀번호가 틀렸습니다.");
            }
        }
    }

    private void Success()
    {
        Debug.Log("비밀번호 일치!");

        isSuccess = true;

        if (rewardKey != null && inventory != null)
        {
            inventory.AddItem(rewardKey);
        }

        if (objectToOpenOrDisable != null)
        {
            objectToOpenOrDisable.SetActive(false);
        }

        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.ShowMessage("아이템을 획득했습니다.");
        }

        Invoke(nameof(CloseUI), 1.5f);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
