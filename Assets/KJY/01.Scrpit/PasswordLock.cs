using UnityEngine;

public class PasswordLock : MonoBehaviour
{
    public string correctPassword = "1234";
    public GameObject rewardItem; // 문고리 등

    public void CheckPassword(string input)
    {
        if (input == correctPassword)
        {
            Debug.Log("정답!");
            rewardItem.SetActive(true); // 보상 등장
            this.gameObject.SetActive(false); // 상자 뚜껑 열림 처리 등
        }
    }
}
