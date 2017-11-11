﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpectacularityTracker : MonoBehaviour
    {
        public static SpectacularityTracker Instance;

        public float CrowdExcitment = 0.2f;
        public float ExcitmentLossPerSec = 0.01f;
        public float ComboTimeBuffer = 3f;

        private float _lastLossTick;
        private Dictionary<Rocket, float> _recentDetonations = new Dictionary<Rocket, float>();

        private void Awake()
        {
            Instance = this;
        }

        private List<Rocket> _toRemove = new List<Rocket>();
        private void Update()
        {
            if (Time.time - 1 > _lastLossTick)
            {
                _lastLossTick = Time.time;
                CrowdExcitment -= ExcitmentLossPerSec;
            }

            // Use recent detonations to detect combos here

            foreach (KeyValuePair<Rocket, float> entry in _recentDetonations)
                if (Time.time - ComboTimeBuffer > entry.Value)
                    _toRemove.Add(entry.Key);

            foreach (Rocket rocket in _toRemove)
                _recentDetonations.Remove(rocket);
        }

        public void RegisterDetonation(Rocket rocket)
        {
            CrowdExcitment += 0.05f;
            _recentDetonations.Add(rocket, Time.time);
        }
    }
}
