using UnityEngine;
using System.Collections;

public class RectTransformConstraint : MonoBehaviour
{

    public bool constrainOnRectTrsChange = false;
    public bool constrainOnStart = false;
    public bool constrainOnUpdate = false;

    public bool constrainWidth = false;
    public bool constrainHeight = false;

    public RectTransform constrainMaster;
    public RectTransform constrainSlave;

    public float widthOffset = 0f;
    public float heightOffset = 0f;

    void Constrain()
    {
        if (constrainMaster == null || constrainSlave == null)
        {
            return;
        }

        Vector2 sizeDelta = constrainSlave.sizeDelta;
        bool applySizeDelta = false;
        if (constrainWidth)
        {
            applySizeDelta = true;
            sizeDelta.x = constrainMaster.sizeDelta.x + widthOffset;
        }
        if (constrainHeight)
        {
            applySizeDelta = true;
            sizeDelta.y = constrainMaster.sizeDelta.y + heightOffset;
        }
        if (applySizeDelta)
        {
            constrainSlave.sizeDelta = sizeDelta;
        }
    }

    void OnRectTransformDimensionsChange()
    {
        if (constrainOnRectTrsChange)
        {
            Constrain();
        }
    }

    void Start()
    {
        if (constrainOnStart)
        {
            Constrain();
        }
    }

    void Update()
    {
        if (constrainOnUpdate)
        {
            Constrain();
        }
    }

}