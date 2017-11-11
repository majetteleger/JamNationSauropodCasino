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
    public float UpdateDelay;

    public bool CanSpawnRocket
    {
        get
        {
            return !_isWaitingForUpdate && transform.childCount <= Capacity;
        }
    }

    private bool _isWaitingForUpdate;

    public Rocket SpawnRocket()
    {
        var rocket = Instantiate(RocketPrefab, transform).GetComponent<Rocket>();
        TryUpdateContent();
        
        return rocket;
    }

    public void RocketLaunched()
    {
        TryUpdateContent(UpdateDelay);
    }

    private void TryUpdateContent(float delay = 0)
    {
        if (!_isWaitingForUpdate)
        {
            Invoke("UpdateContent", delay);
            _isWaitingForUpdate = true;
        }
    }

    private void UpdateContent()
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

            if (Orientation == ContentOrientation.LeftToRight)
            {
                xPosition = (spacing * i + contentSize * i + contentSize / 2) - containerSize / 2;
            }
            else
            {
                xPosition = -(spacing * i + contentSize * i + contentSize / 2) + containerSize / 2;
            }

            content[i].position = new Vector3(xPosition, content[i].position.y, content[i].position.z);
        }

        _isWaitingForUpdate = false;
    }
}
