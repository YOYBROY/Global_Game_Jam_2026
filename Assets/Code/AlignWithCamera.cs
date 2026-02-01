using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    private Transform mainCameraTransform;
    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }
    void LateUpdate()
    {
        Vector3 cameraPos = mainCameraTransform.position;
        cameraPos.y = transform.position.y;
        transform.LookAt(cameraPos);
    }
}
