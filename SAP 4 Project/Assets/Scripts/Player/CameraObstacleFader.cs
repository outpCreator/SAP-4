using UnityEngine;
using System.Collections.Generic;

public class CameraObstacleFader : MonoBehaviour
{
    Transform cam;
    public Transform pivot;
    public LayerMask fadeLayer;

    List<Renderer> renderes = new List<Renderer>();

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 direction = cam.position - pivot.position;
        float distance = direction.magnitude;
        Ray ray = new Ray(pivot.position, direction.normalized);

        foreach (var r in renderes)
        {
            SetTransparent(r, false);
        }
        renderes.Clear();

        var hits = Physics.RaycastAll(ray, distance, fadeLayer);
        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                SetTransparent(rend, true);
                renderes.Add(rend);
            }
        }
    }

    void SetTransparent(Renderer rend, bool fade)
    {
        foreach (var mat in rend.materials)
        {
            if (fade)
            {
                mat.SetFloat("_Mode", 2);
                Color c = mat.color;
                c.a = 0.3f;
                mat.color = c;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.renderQueue = 3000;
            }
            else
            {
                Color c = mat.color;
                c.a = 1f;
                mat.color = c;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.renderQueue = -1;
            }
        }
    }
}
