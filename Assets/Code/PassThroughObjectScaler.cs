using DG.Tweening;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [Header ("Variables")]
    [SerializeField] float maxSize = 3f;
    [SerializeField] float doScaleDuration = 2f;
    [SerializeField] Ease doScaleEasing;
    [SerializeField] bool orientTargetObject;

    [Header ("References")]
    [SerializeField] private LayerMask wallMask;

    private Transform targetObject;
    private Camera mainCamera;
    private int objectVisible = 0;

    void Awake()
    {
        mainCamera = Camera.main;
        targetObject = gameObject.transform;
    }

    private void Update()
    {
        if (orientTargetObject)
        {
            targetObject.rotation = Quaternion.LookRotation((mainCamera.transform.position - targetObject.transform.position).normalized);
        }

        int temp = RayHitObject();
        if(objectVisible != temp)
        {
            targetObject.transform.DOScale(maxSize * temp, doScaleDuration).SetEase(doScaleEasing);
            objectVisible = temp;
        }
    }

    int RayHitObject()
    {
        Vector3 direction = mainCamera.transform.position - targetObject.transform.position;
        if(Physics.Raycast(targetObject.transform.position, (direction).normalized, out RaycastHit hit, direction.magnitude, wallMask))
        {
            Debug.DrawRay(targetObject.transform.position, (direction).normalized, Color.green);
            return 1;
        }
        Debug.DrawRay(targetObject.transform.position, (direction).normalized, Color.red);
        return 0;
    }
}