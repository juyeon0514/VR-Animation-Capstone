using UnityEngine;

public class FlashlightToShader : MonoBehaviour
{
    public Material tmpMaterial; 

    void Update()
    {
        if (tmpMaterial != null)
        {
            tmpMaterial.SetVector("_FlashlightPos", transform.position);
        }
    }
}
