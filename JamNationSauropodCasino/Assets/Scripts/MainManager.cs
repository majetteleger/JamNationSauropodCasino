﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public struct KeyRocketBinding
    {
        public KeyCode Key;
        public Rocket Rocket;

        public KeyRocketBinding(KeyCode key, Rocket rocket)
        {
            Key = key;
            Rocket = rocket;
        }
    }

    public static MainManager Instance;
    
    public RocketContainer RocketContainer;
    public float RocketSpawnTime;
    public KeyCode[] PossibleKeyBinds;

    public KeyRocketBinding[] KeyRocketBindings { get; set; }

    private float _RocketSpawnTime;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _RocketSpawnTime = RocketSpawnTime;

        var keyRocketBindingsList = new List<KeyRocketBinding>();
        for(int i = 0; i < PossibleKeyBinds.Length; i++)
        {
            var newBinding = new KeyRocketBinding(PossibleKeyBinds[i], null);
            keyRocketBindingsList.Add(newBinding);
        }

        KeyRocketBindings = keyRocketBindingsList.ToArray();
    }

    private void Update()
    {
        _RocketSpawnTime -= Time.deltaTime;

        if (_RocketSpawnTime <= 0)
        {
            var rocket = RocketContainer.TrySpawnRocket();

            if(rocket != null)
            {
                var key = BindRocketToRandomKey(rocket);
                var displayedKey = key.ToString();

                rocket.DisplayKey(displayedKey.Length > 1 ? displayedKey[displayedKey.Length - 1].ToString() : displayedKey);
            }
            
            _RocketSpawnTime = RocketSpawnTime;
        }
        
        for(int i = 0; i < KeyRocketBindings.Length; i++)
        {
            if(KeyRocketBindings[i].Rocket == null)
            {
                continue;
            }

            var key = KeyRocketBindings[i].Key;
            var rocket = KeyRocketBindings[i].Rocket;
            
            if (Input.GetKeyDown(key))
            {
                rocket.Select();
            }

            if(Input.GetKeyUp(key))
            {
                rocket.Unselect();
            }
        }
    }
    
    public void UnbindKey(Rocket rocket)
    {
        for(int i = 0; i < KeyRocketBindings.Length; i++)
        {
            if(KeyRocketBindings[i].Rocket == rocket)
            {
                KeyRocketBindings[i].Rocket = null;
                return;
            }
        }
    }

    private KeyCode BindRocketToRandomKey(Rocket rocket)
    {
        var availableIndicesList = new List<int>();
        for (int i = 0; i < KeyRocketBindings.Length; i++)
        {
            if(KeyRocketBindings[i].Rocket == null)
            {
                availableIndicesList.Add(i);
            }
        }

        var availableIndices = availableIndicesList.ToArray();
        var randomIndex = availableIndices[Random.Range(0, availableIndices.Length)];

        KeyRocketBindings[randomIndex].Rocket = rocket;

        return KeyRocketBindings[randomIndex].Key;
    }
}