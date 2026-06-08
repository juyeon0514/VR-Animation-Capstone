using TMPro;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    public static PasswordManager Instance { get; private set; }

    [Header("Password Settings")]
    [SerializeField] private int passwordLength = 4;

    [Header("Wall Password Texts")]
    [SerializeField] private TMP_Text[] wallPasswordTexts;

    public string CurrentPassword { get; private set; }

    private void Awake()
    {
        Instance = this;
        GeneratePassword();
        ApplyPasswordToWallTexts();
    }

    public void GeneratePassword()
    {
        CurrentPassword = "";

        for (int i = 0; i < passwordLength; i++)
        {
            int randomDigit = Random.Range(0, 10);
            CurrentPassword += randomDigit.ToString();
        }

        Debug.Log("이번 판 비밀번호: " + CurrentPassword);
    }

    public void ApplyPasswordToWallTexts()
    {
        if (string.IsNullOrEmpty(CurrentPassword))
        {
            Debug.LogWarning("비밀번호가 아직 생성되지 않았습니다.");
            return;
        }

        if (wallPasswordTexts == null || wallPasswordTexts.Length == 0)
        {
            Debug.LogWarning("벽 비밀번호 Text가 연결되지 않았습니다.");
            return;
        }

        for (int i = 0; i < wallPasswordTexts.Length; i++)
        {
            if (wallPasswordTexts[i] == null)
            {
                continue;
            }

            if (i < CurrentPassword.Length)
            {
                wallPasswordTexts[i].text = CurrentPassword[i].ToString();
            }
            else
            {
                wallPasswordTexts[i].text = "";
            }
        }
    }

    public void RegeneratePassword()
    {
        GeneratePassword();
        ApplyPasswordToWallTexts();
    }
}
