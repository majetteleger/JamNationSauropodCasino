using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class MainManager : MonoBehaviour
{
    public enum RocketColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        COUNT
    }

    public struct KeyRocketBinding
    {
        public Button Key;
        public Rocket Rocket;

        public KeyRocketBinding(Button key, Rocket rocket)
        {
            Key = key;
            Rocket = rocket;
        }
    }

    public static MainManager Instance;
    
    public RocketContainer RocketContainer;
    public int NumRockets;
    public int StartRockets;
    public float RocketSpawnTime;
    public Button[] PossibleKeyBinds;

    public KeyRocketBinding[] KeyRocketBindings { get; set; }

    private float _RocketSpawnTimer;

    private bool _buttonADown;
    private bool _buttonBDown;
    private bool _buttonXDown;
    private bool _buttonYDown;
    private bool _buttonL1Down;
    private bool _buttonR1Down;
    private bool _buttonL2Down;
    private bool _buttonR2Down;
    private bool _buttonUpDown;
    private bool _buttonDownDown;
    private bool _buttonLeftDown;
    private bool _buttonRightDown;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _RocketSpawnTimer = RocketSpawnTime;

        var keyRocketBindingsList = new List<KeyRocketBinding>();
        for(int i = 0; i < PossibleKeyBinds.Length; i++)
        {
            var newBinding = new KeyRocketBinding(PossibleKeyBinds[i], null);
            keyRocketBindingsList.Add(newBinding);
        }

        KeyRocketBindings = keyRocketBindingsList.ToArray();

        for(int i = 0; i < StartRockets; i++)
        {
            TrySpawnRocket(true);
        }
    }

    private void Update()
    {
        TrySpawnRocket();
        
        for(int i = 0; i < KeyRocketBindings.Length; i++)
        {
            if(KeyRocketBindings[i].Rocket == null)
            {
                continue;
            }

            var key = KeyRocketBindings[i].Key;
            var rocket = KeyRocketBindings[i].Rocket;
            
            if (ButtonManager.GetButtonDown(key, Player.Any))
            {
                if (!rocket.IsSelected)
                {
                    switch (key)
                    {
                        case Button.A:
                            _buttonADown = true;
                            break;
                        case Button.B:
                            _buttonBDown = true;
                            break;
                        case Button.X:
                            _buttonXDown = true;
                            break;
                        case Button.Y:
                            _buttonYDown = true;
                            break;
                        case Button.L1:
                            _buttonL1Down = true;
                            break;
                        case Button.R1:
                            _buttonR1Down = true;
                            break;
                        case Button.R2:
                            _buttonR2Down = true;
                            break;
                        case Button.L2:
                            _buttonL2Down = true;
                            break;
                        case Button.Up:
                            _buttonUpDown = true;
                            break;
                        case Button.Down:
                            _buttonDownDown = true;
                            break;
                        case Button.Left:
                            _buttonLeftDown = true;
                            break;
                        case Button.Right:
                            _buttonRightDown = true;
                            break;
                    }

                    rocket.Select();
                }
            }

            if(ButtonManager.GetButtonUp(key, Player.Any))
            {
                switch (key)
                {
                    case Button.A:
                        if(_buttonADown)
                        {
                            rocket.Unselect();
                            _buttonADown = false;
                        }
                        break;
                    case Button.B:
                        if (_buttonBDown)
                        {
                            rocket.Unselect();
                            _buttonBDown = false;
                        }
                        break;
                    case Button.X:
                        if (_buttonXDown)
                        {
                            rocket.Unselect();
                            _buttonXDown = false;
                        }
                        break;
                    case Button.Y:
                        if (_buttonYDown)
                        {
                            rocket.Unselect();
                            _buttonYDown = false;
                        }
                        break;
                    case Button.L1:
                        if (_buttonL1Down)
                        {
                            rocket.Unselect();
                            _buttonL1Down = false;
                        }
                        break;
                    case Button.R1:
                        if (_buttonR1Down)
                        {
                            rocket.Unselect();
                            _buttonR1Down = false;
                        }
                        break;
                    case Button.R2:
                        if (_buttonR2Down)
                        {
                            rocket.Unselect();
                            _buttonR2Down = false;
                        }
                        break;
                    case Button.L2:
                        if (_buttonL2Down)
                        {
                            rocket.Unselect();
                            _buttonL2Down = false;
                        }
                        break;
                    case Button.Up:
                        if (_buttonUpDown)
                        {
                            rocket.Unselect();
                            _buttonUpDown = false;
                        }
                        break;
                    case Button.Down:
                        if (_buttonDownDown)
                        {
                            rocket.Unselect();
                            _buttonDownDown = false;
                        }
                        break;
                    case Button.Left:
                        if (_buttonLeftDown)
                        {
                            rocket.Unselect();
                            _buttonLeftDown = false;
                        }
                        break;
                    case Button.Right:
                        if (_buttonRightDown)
                        {
                            rocket.Unselect();
                            _buttonRightDown = false;
                        }
                        break;
                }
            }
        }
    }
    
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver"); 
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

    private void TrySpawnRocket(bool fuckTimer = false)
    {
        _RocketSpawnTimer -= Time.deltaTime;

        if (_RocketSpawnTimer <= 0 || fuckTimer)
        {
            var rocket = RocketContainer.TrySpawnRocket();

            if (rocket != null)
            {
                var key = BindRocketToRandomKey(rocket);
                string displayedKey;

                switch (key)
                {
                    case Button.A:
                        displayedKey = "A";
                        break;
                    case Button.B:
                        displayedKey = "B";
                        break;
                    case Button.X:
                        displayedKey = "X";
                        break;
                    case Button.Y:
                        displayedKey = "Y";
                        break;
                    case Button.L1:
                        displayedKey = "L1";
                        break;
                    case Button.R1:
                        displayedKey = "R1";
                        break;
                    case Button.R2:
                        displayedKey = "R2";
                        break;
                    case Button.L2:
                        displayedKey = "L2";
                        break;
                    case Button.Up:
                        displayedKey = "U";
                        break;
                    case Button.Down:
                        displayedKey = "D";
                        break;
                    case Button.Left:
                        displayedKey = "L";
                        break;
                    case Button.Right:
                        displayedKey = "R";
                        break;
                    default:
                        displayedKey = " ";
                        break;
                }

                rocket.RocketInfo.DisplayKey(displayedKey);
            }

            _RocketSpawnTimer = RocketSpawnTime;
        }
    }

    private Button BindRocketToRandomKey(Rocket rocket)
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
