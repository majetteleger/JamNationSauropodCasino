using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketContainer : MonoBehaviour
{
    public enum ContentOrientation
    {
        LeftToRight,
        RightToLeft
    }

    public GameObject RocketPrefab;
    public ContentOrientation Orientation;
    public int Capacity;
    
    public Rocket TrySpawnRocket()
    {
        if(transform.childCount <= Capacity)
        {
            var rocket = Instantiate(RocketPrefab, transform).GetComponent<Rocket>();
            ArrangeContent();

            return rocket;
        }
        
        return null;
    }

    public void RocketLaunched()
    {
        ArrangeContent();
    }

    private void ArrangeContent()
    {
        var content = new List<Transform>();
        for (int i = 1; i < transform.childCount; i++)
        {
            content.Add(transform.GetChild(i));
        }

        if(content.Count == 0)
        {
            return;
        }
        
        var containerSize = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.extents.x * 2;
        var contentSize = content[0].GetComponent<MeshRenderer>().bounds.extents.x * 2;

        var spacing = (containerSize - (Capacity * contentSize)) / (Capacity - 1);

        for (var i = 0; i < content.Count; i++)
        {
            var xPosition = 0f;

            if(Orientation == ContentOrientation.LeftToRight)
            {
                xPosition = (spacing * i + contentSize * i + contentSize / 2) - containerSize / 2;
            }
            else
            {
                xPosition = -(spacing * i + contentSize * i + contentSize / 2) + containerSize / 2;
            }

            content[i].position = new Vector3(xPosition, content[i].position.y, content[i].position.z);
        }
    }
}
