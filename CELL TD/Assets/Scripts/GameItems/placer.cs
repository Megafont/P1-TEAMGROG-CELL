using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Placer : MonoBehaviour
{
    public GameObject tower;
    public TowerInfo_Base info;
    public bool disableAnimations = true;


    [SerializeField]
    private Material highlightMat;

    [SerializeField]
    private Material badMat;

    [SerializeField]
    private LayerMask layerToCheck;

    private Camera mainCamera;
    private bool overlapping = false;

    void Start()
    {
        /* NOTE:
         *     We call SetActive(false) on the prefab, so that when we instantiate an instance, it will be disabled.
         *     We just have to be careful to call SetActive(true) after Instantiate(), or else the next time the player
         *     places this type of tower, it will not work, as it will be disabled.
         * */
        info.Prefab.SetActive(false);
        GameObject newModel = Instantiate(info.Prefab, Vector3.zero, Quaternion.identity, transform);
        info.Prefab.SetActive(true); // Re-enable the active state of the prefab. This is NOT the instance we just spawned in.


        // Remove the tower component from the spawned prefab before we enable it.
        Destroy(newModel.GetComponent<Tower_Base>());

        // Enable the prefab so it can receive events and stick to the mouse.
        newModel.SetActive(true);

        // Set the size of the range bubble to the towers base range.
        // NOTE: FindRecursive() is an extension method I defined in GameObjectUtils.cs. Unlike the normal Transform.find(), this one
        //       is recursive so it can find an object with the specified name, even if it is a child of a child of the parent object.
        GameObject rangeObj = newModel.transform.FindResursive("Range");
        rangeObj.transform.localScale = new Vector3(info.BaseRange, info.BaseRange, info.BaseRange);

        // Disable all colliders in the tower
        // IMPORTANT: This step is REQUIRED, otherwise you can't place the tower because the tower ghost will detect itself when it does Physics.SphereCast(),
        // which causes it to think you can't place the tower there.
        Collider[] colliders = newModel.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Disable animations if this option is on.
        if (disableAnimations)
        {
            Animator[] animators = newModel.GetComponentsInChildren<Animator>();
            foreach (Animator animator in animators)
            {
                animator.enabled = false;
            }
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

                GameManager.Instance.MoneySystem.SubtractCurrency((int) info.BuildCost);
                PlaceTower();
            }
            else if (!IsOverUI())
            {
                GameManager.Instance.MoneySystem.SubtractCurrency(999999999); //This will show the player that they do not have enough money
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
