using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Placer : MonoBehaviour
{
    public GameObject tower;
    public TowerInfo_Base info;

    private Camera mainCamera;

    [SerializeField]
    private Material highlightMat;

    [SerializeField]
    private Material badMat;

    [SerializeField]
    private LayerMask layerToCheck;

    private bool overlapping = false;

    void Start()
    {
        MeshFilter[] sourceMeshFilters = tower.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter sourceMeshFilter in sourceMeshFilters)
        {
            GameObject newModel = new GameObject("part");
            newModel.transform.parent = transform;

            newModel.AddComponent<MeshFilter>();
            newModel.AddComponent<MeshRenderer>();

            MeshFilter newMeshFilter = newModel.GetComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = newModel.GetComponent<MeshRenderer>();

            newMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            newMeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;

            newMeshFilter.transform.localScale = sourceMeshFilter.transform.localScale;
        }

        UpdateMats(true);
        overlapping = true;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.y -= 25.0f; //Offset
        mouseScreenPos.z = Camera.main.transform.position.y;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseWorldPos.y = 0f;
        transform.position = mouseWorldPos;

        CheckOverlap();

        if (Input.GetButtonDown("Fire1"))
        {
            if (tower && GameManager.Instance.MoneySystem.MoneyAmount >= (int)info.BuildCost && !overlapping)
            {
                if (IsOverUI())
                {
                    Destroy(gameObject);
                    return;
                }
                GameManager.Instance.MoneySystem.SubtractCurrency((int)info.BuildCost);
                PlaceTower();
            }
            else if (!IsOverUI())
            {
                GameManager.Instance.MoneySystem.SubtractCurrency(999999999);//This will show the player that they do not have enough money
            }
        }
    }

    private bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void PlaceTower()
    {
        var newTower = Instantiate(tower);
        newTower.transform.position = transform.position;
        Destroy(gameObject);
    }

    private void CheckOverlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, layerToCheck);
        if (colliders.Length > 0)
        {
            overlapping = true;
        }
        else
        {
            overlapping = false;
        }

        UpdateMats(overlapping);
    }

    private void UpdateMats(bool overlap)
    {
        Material targetMat = null;
        if (overlap)
        {
            targetMat = badMat;
        }
        else
        {
            targetMat = highlightMat;
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] newMaterials = meshRenderer.materials;
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = targetMat;
            }
            meshRenderer.materials = newMaterials;
        }
    }
}
