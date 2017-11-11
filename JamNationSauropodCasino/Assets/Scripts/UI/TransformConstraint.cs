using UnityEngine;
using UnityEngine.Events;

public class TransformConstraint : MonoBehaviour
{
    private Camera cam;
    private Transform camTransform;

    public Transform masterTransform;
    public RectTransform slaveRectTransform;

    public Vector3 worldOffset = Vector3.zero;
    public Vector3 screenOffset = Vector3.zero;
    public float outDistance = 0f;

    public bool inverseWhenBehindCamera = true;

    public enum CameraSpace
    {
        Screen,
        Viewport
    }
    public CameraSpace cameraSpace = CameraSpace.Screen;

    private Vector3 cPosA = new Vector3();
    private Vector3 cPosB = new Vector3();
    public void Constrain()
    {
        if (cam == null)
        {
            cam = Camera.main;
            camTransform = cam.transform;
        }


        if (masterTransform == null || slaveRectTransform == null)
        {
            return;
        }

        cPosB =
            cameraSpace == CameraSpace.Screen
            ? cam.WorldToScreenPoint(masterTransform.position + worldOffset)
            : cam.WorldToViewportPoint(masterTransform.position + worldOffset);

        outDistance = cPosB.z;
        
        if(inverseWhenBehindCamera && IsBehindCamera())
        {
            cPosB.x *= -1f;
            cPosB.y *= -1f;
        }

        cPosA.x = Mathf.Round(cPosB.x);
        cPosA.y = Mathf.Round(cPosB.y);
        cPosA.z = slaveRectTransform.position.z;

        slaveRectTransform.position = cPosA + screenOffset;
    }

    void OnEnable()
    {
        Constrain();
    }

    public bool IsBehindCamera()
    {
        var camTrs = camTransform;
        var offset =
            masterTransform.position
            - camTrs.position;

        if (Vector3.Dot(offset, camTrs.forward) < 0f)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        Constrain();
	}
	
    /*void Update()
    {
        Constrain();
	}*/
    
    public UnityEvent m_OnAfterConstrain = new UnityEvent();
}
