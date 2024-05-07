using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;
    private Camera mainCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);
        Debug.DrawRay(transform.position, offset, Color.red);
        foreach (var hit in hitObjects)
        {
            ProcessAllRenderers(hit.transform, (renderer) =>
            {
                Material[] materials = renderer.materials;
                for (int m = 0; m < materials.Length; m++)
                {
                    materials[m].SetVector("_CutoutPos", new Vector4(cutoutPos.x, cutoutPos.y, 0, 0));
                    materials[m].SetFloat("_CutoutSize", 0.1f);
                    materials[m].SetFloat("_FalloffSize", 0.05f);
                }
            });
        }
    }
    // Helper method to process all renderers in the hierarchy
    private void ProcessAllRenderers(Transform root, System.Action<Renderer> process)
    {
        Renderer renderer = root.GetComponent<Renderer>();
        if (renderer != null)
        {
            process(renderer);
        }
        foreach (Transform child in root)
        {
            ProcessAllRenderers(child, process);
        }
    }
}
