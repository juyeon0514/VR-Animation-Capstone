using UnityEngine;

public class FlashlightToShader : MonoBehaviour
{
    public Material tmpMaterial; // 여기에 비밀번호 텍스트에 적용한 메터리얼을 넣습니다.

    void Update()
    {
        // 매 프레임마다 손전등의 현재 월드 좌표를 메터리얼의 FlashlightPos 변수로 보냅니다.
        if (tmpMaterial != null)
        {
            tmpMaterial.SetVector("_FlashlightPos", transform.position);
        }
    }
}
