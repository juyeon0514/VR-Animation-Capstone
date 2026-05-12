using UnityEngine;

public class RevealObject : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    private Material m_Mat;

    private void Start()
    {
        m_Mat = GetComponent<Renderer>().sharedMaterial;
    }

    private void Update()
    {
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
            m_Mat.SetVector("_MyLightPosition", new Vector3(9999, 9999, 9999));
        }
    }
}
