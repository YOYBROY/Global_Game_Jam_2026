using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyVisionCone : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float VisionRange;
    [SerializeField] private float VisionAngle;
    [SerializeField] private LayerMask VisionObstructingLayer;
    [SerializeField] private int VisionConeResolution = 120;

    [SerializeField] private Color calmColor;
    [SerializeField] private Color alertColor;

    [Header("References")]
    public Material VisionConeMaterial;
    

    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;

    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }

    
    void Update()
    {
        DetectPlayer();
        DrawVisionCone();
    }

    public void DetectPlayer()
    {
        VisionConeMaterial.color = calmColor;
        float Currentangle = -VisionAngle / 2;
        float angleIncrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;
        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange))
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    VisionConeMaterial.color = alertColor;
                    GameEvents.current.PlayerLocated(hit.collider.gameObject);
                    break;
                }
            }
            Currentangle += angleIncrement;
        }
    }

    void DrawVisionCone()//this method creates the vision cone mesh
    {
	int[] triangles = new int[(VisionConeResolution - 1) * 3];
    	Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIncrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }


            Currentangle += angleIncrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }
}