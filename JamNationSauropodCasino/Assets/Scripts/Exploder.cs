using System;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [Serializable]
    public class Explodable
    {
        public GameObject Go;
        public List<TimeTransform> TimeTransforms = new List<TimeTransform>();
    }

    public List<Explodable> Explodables = new List<Explodable>();
    private Explodable _explodable;

    public int GetExplodablesCount()
    {
        return Explodables.Count;
    }

    public void Select(int index)
    {
        //if (index < 0 || index >= Explodables.Count) return; //let's not mute errors
        _explodable = Explodables[index];
        //if (_explodable == null) return; //let's not mute errors
        _explodable.Go.SetActive(true);
    }

    public void Launch()
    {
        //if (_explodable == null) return; //let's not mute errors
        foreach (TimeTransform tt in _explodable.TimeTransforms)
        {
            tt.Launch();
        }
    }

    public void Detonate()
    {
        //if (_explodable == null) return; //let's not mute errors
        foreach (TimeTransform tt in _explodable.TimeTransforms)
        {
            tt.Detonate();
        }
    }
}

