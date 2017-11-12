using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpectacularityTracker : MonoBehaviour
    {
        public static SpectacularityTracker Instance;

        [SerializeField]
        private float _crowdExcitment = 0.2f;
        public float CrowdExcitment
        {
            get
            {
                return _crowdExcitment;
            }
            set
            {
                _crowdExcitment = Mathf.Clamp01(value);
                SpectacularityPanel.Instance.UpdateSpectacularityGauge(_crowdExcitment, 1f);
            }
        }

        public float ExcitmentLossPerSec = 0.01f;
        public float ComboTimeBuffer = 3f;
        public float GrandFinaleBonus = 0.4f;
        public float TimedBlastBonus = 0.2f;

        public float UpdateCrowdHandsTime;
        private float _updateCrowdHandsTimer;
        public float UpdateCrowdFlipTime;
        private float _updateCrowdFlipTimer;
        public float CrowdExcitementBuffer;

        public Folk[] Crowd { get; set; }

        private float _lastLossTick;
        private Dictionary<Rocket, float> _recentDetonations = new Dictionary<Rocket, float>();
        private List<Rocket> _usedInCombo = new List<Rocket>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpectacularityPanel.Instance.UpdateSpectacularityGauge(CrowdExcitment, 1f);
            Crowd = FindObjectsOfType<Folk>();
            UpdateCrowdHands(CrowdExcitment);
            UpdateCrowdFlip(CrowdExcitment);

            _updateCrowdHandsTimer = UpdateCrowdHandsTime;
            _updateCrowdFlipTimer = UpdateCrowdFlipTime;
        }

        private List<Rocket> _toRemove = new List<Rocket>();

        private void UpdateCrowdHands(float excitement)
        {
            for(int i = 0; i < Crowd.Length; i++)
            {
                float rndEx = Random.Range(0.0f, 1.0f);

                if (rndEx <= excitement - CrowdExcitementBuffer)
                {
                    Crowd[i].HandsUp();
                }
                else
                {
                    Crowd[i].HandsDown();
                }
            }
        }

        private void UpdateCrowdFlip(float excitement)
        {
            for (int i = 0; i < Crowd.Length; i++)
            {
                float rndFl = Random.Range(0.0f, 1.0f);

                if (rndFl <= excitement - CrowdExcitementBuffer)
                {
                    Crowd[i].transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
                }
                else
                {
                    Crowd[i].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                }
            }
        }

        private void Update()
        {
            _updateCrowdHandsTimer -= Time.deltaTime;
            _updateCrowdFlipTimer -= Time.deltaTime;

            if(_updateCrowdHandsTimer <= 0)
            {
                _updateCrowdHandsTimer = UpdateCrowdHandsTime;
                UpdateCrowdHands(CrowdExcitment);
            }

            if (_updateCrowdFlipTimer <= 0)
            {
                _updateCrowdFlipTimer = UpdateCrowdFlipTime;
                UpdateCrowdFlip(CrowdExcitment);
            }
            
            if (Time.time - 1 > _lastLossTick)
            {
                _lastLossTick = Time.time;
                CrowdExcitment -= ExcitmentLossPerSec;
            }

            CheckCombos();

            foreach (KeyValuePair<Rocket, float> entry in _recentDetonations)
            {
                if (Time.time - ComboTimeBuffer > entry.Value)
                    _toRemove.Add(entry.Key);
            }
            foreach (Rocket rocket in _toRemove)
            {
                _recentDetonations.Remove(rocket);
                _usedInCombo.Remove(rocket);
            }
            _toRemove.Clear();

            if(CrowdExcitment <= 0)
            {
                MainManager.Instance.GameOver();
            }
        }

        private void CheckCombos()
        {
            //Grand Finale Combo
            if (_recentDetonations.Count >= 12)
            {
                CrowdExcitment += 0.4f;
                Debug.Log("Grand Finale");
            }

            //Timed Blast Combo
            int timedBlastCounter = 0;
            foreach (KeyValuePair<Rocket, float> entry in _recentDetonations)
            {
                if (entry.Value <= Time.time + 0.1f && !_usedInCombo.Contains(entry.Key))
                {
                    timedBlastCounter++;
                    _usedInCombo.Add(entry.Key);
                }
            }
            if (timedBlastCounter >= 3)
            {
                CrowdExcitment += 0.2f;
                Debug.Log("Timed Blast");
            }

            //Color Blast Combo
            /*bool colorMatch = true;
            int colorCounter = 0;
            MainManager.RocketColor tempColor = _recentDetonations.ElementAt(0).Key.RocketColor;
            foreach (KeyValuePair<Rocket, float> entry in _recentDetonations)
            {
                colorCounter++;

                if (entry.Key.RocketColor != tempColor)
                {
                    colorMatch = false;
                    break;
                }
            }
            if(colorMatch)
            {
                CrowdExcitment += 0.05f * colorCounter;
                Debug.Log("Color Blast");
            }*/
        }

        public void RegisterDetonation(Rocket rocket)
        {
            CrowdExcitment += 0.05f;
            _recentDetonations.Add(rocket, Time.time);
        }
    }
}
