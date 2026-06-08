using UnityEngine;

public class RevealObject : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private Light spotLight;

    [Header("Renderer")]
    [SerializeField] private Renderer targetRenderer;

    private Material m_Mat;

    private void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (targetRenderer == null)
        {
            Debug.LogError($"{gameObject.name}: Renderer를 찾지 못했습니다.");
            enabled = false;
            return;
        }

        m_Mat = targetRenderer.material;
    }

    private void Update()
    {
        if (m_Mat == null)
        {
            return;
        }

        if (spotLight == null)
        {
            GameObject lightObj = GameObject.FindWithTag("Flashlight");

            if (lightObj != null)
            {
                spotLight = lightObj.GetComponent<Light>();
            }
        }

        if (spotLight != null && spotLight.gameObject.activeInHierarchy)
        {
            m_Mat.SetVector("_MyLightPosition", spotLight.transform.position);
            m_Mat.SetVector("_MyLightDirection", -spotLight.transform.forward);
        }
        else
        {
            m_Mat.SetVector("_MyLightPosition", new Vector4(9999, 9999, 9999, 1));
        }
    }
}
