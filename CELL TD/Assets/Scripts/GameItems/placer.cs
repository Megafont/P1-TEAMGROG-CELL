using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Placer : MonoBehaviour
{
    public GameObject tower;
    public TowerInfo_Base info;
    public bool disableAnimations = true;


    [SerializeField]
    private Material highlightMat;

    [SerializeField]
    private Material badMat;

    private Camera mainCamera;
    private bool isOverlapping = false;

    private GameObject towerGhost;
    private CapsuleCollider towerGhostCollider;

    private bool _isMouseOutsideScreen;


    void Start()
    {
        /* NOTE:
         *     We call SetActive(false) on the prefab, so that when we instantiate an instance, it will be disabled.
         *     We just have to be careful to call SetActive(true) after Instantiate(), or else the next time the player
         *     places this type of tower, it will not work, as it will be disabled.
         * */
        info.Prefab.SetActive(false);
        towerGhost = Instantiate(info.Prefab, Vector3.zero, Quaternion.identity, transform);
        towerGhostCollider = towerGhost.GetComponent<CapsuleCollider>();
        info.Prefab.SetActive(true); // Re-enable the active state of the prefab. This is NOT the instance we just spawned in.


        // Remove the tower component from the spawned prefab before we enable it.
        Destroy(towerGhost.GetComponent<Tower_Base>());

        // Enable the prefab so it can receive events and stick to the mouse.
        towerGhost.SetActive(true);

        // Set the size of the range bubble to the towers base range.
        // NOTE: FindRecursive() is an extension method I defined in GameObjectUtils.cs. Unlike the normal Transform.find(), this one
        //       is recursive so it can find an object with the specified name, even if it is a child of a child of the parent object.
        GameObject rangeObj = towerGhost.transform.FindResursive("Range");
        rangeObj.transform.localScale = new Vector3(info.BaseRange, info.BaseRange, info.BaseRange);

        // Disable all colliders in the tower
        // IMPORTANT: This step is REQUIRED, otherwise you can't place the tower because the tower ghost will detect itself when it does Physics.SphereCast(),
        // which causes it to think you can't place the tower there.
        Collider[] colliders = towerGhost.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Disable animations if this option is on.
        if (disableAnimations)
        {
            Animator[] animators = towerGhost.GetComponentsInChildren<Animator>();
            foreach (Animator animator in animators)
            {
                animator.enabled = false;
            }
        }


        UpdateMats(true);
        
        isOverlapping = true;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Check that the camera is within the screen bounds. If not, it will cause errors so we will just return.
        Vector2 view = Camera.main.ScreenToViewportPoint(mousePos);
        _isMouseOutsideScreen = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        if (_isMouseOutsideScreen)
        {            
            // The multiplying by 100 here just ensures the tower ghost is completely offscreen when the mouse is.
            transform.position = new Vector3(view.x * 100f, transform.position.y, view.y * 100f);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Background")))
        {
            //Debug.LogWarning("The raycast did not hit anything! It should have hit the background.");          
            return;
        }
        else
        {
            transform.position = hit.point;
        }

        CheckOverlap();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tower && GameManager.Instance.MoneySystem.MoneyAmount >= (int)info.BuildCost && !isOverlapping)
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
        // First use an overlap sphere to check if the tower ghost is overlapping any paths.
        // I am using the ghost tower's collider radius reduced by a bit here to make the path collision a little more forgiving so you can place towers
        // in tighter spaces like between to veins (paths).
        Collider[] colliders = Physics.OverlapSphere(transform.position, towerGhostCollider.radius * 0.6f, LayerMask.GetMask("Paths"));
        bool overlappingPaths = colliders.Length > 0;
        
        // Now check if we are overlapping with any other towers.
        // I am using the ghost tower's collider radius increased by a bit here to make it require a little space around the tires so they can't be crammed too close to each other.
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, towerGhostCollider.radius * 1.2f, LayerMask.GetMask("Tower"));
        bool overlappingTowers = colliders2.Length > 0;


        isOverlapping = overlappingPaths || overlappingTowers;

        UpdateMats(isOverlapping);
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
