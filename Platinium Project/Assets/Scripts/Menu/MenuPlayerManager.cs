using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MenuPlayerManager : MonoBehaviour
{
    //Grégoire a fait ce script

    public static MenuPlayerManager Instance = null;


    public Player _player;

    private List<Player> otherPlayers;
    private List<Player> allPlayers;

    public InMenuPlayer playerEntity;
    private GetMenuInformation getMenuInfoScript;

    [Header("Options")]
    public GameObject optionsPanel;
    public Slider musicSlider;
    private bool _isOnOptions = false;

    [Header("Play")]
    public GameObject startPanel;
    //public GameObject playerSelection;
    public Slider numberPlayers;
    public string sceneName;
    private bool _isStartGameShowing;
    public GameObject selecPanel;
    public Slider playerNumberSlider;
    private bool _theMenuHasBegun = false;

    [Header("Players 2, 3 & 4")]
    public GameObject[] OtherPlayerNumber;
    public GameObject[] OtherPlayerName;
    public GameObject[] OtherPlayerFace;
    public GameObject[] OtherJoin;
    private bool _secondPress = false;
    private bool _thirdPress = false;
    private bool _fourthPress = false;
    private bool _secondPlayerIsHere = false;
    private bool _thirdPlayerIsHere = false;
    private bool _fourthPlayerIsHere = false;
    private int secondPlayerIndex = -1;
    private int thirdPlayerIndex = -1;
    private int fourthPlayerIndex = -1;

    [Header ("Char Select Buttons")]
    public Image[] readyButton;
    public Image[] readyButtonConfirm;
    public Sprite readySprite;
    public Sprite originalSprite;
    public Sprite outSprite;
    private bool _isCharSelecShowing = false;

    private float timerPOne = 0;
    private float timerPTwo = 0;
    private float timerPThree = 0;
    private float timerPFour = 0;

    private bool _isPOneReady = false;
    private bool _isPTwoReady = false;
    private bool _isPThreeReady = false;
    private bool _isPFourReady = false;

    private bool[] _isEveryoneReady = new bool[4];
    private int _howManyReady = 0;

    [Header ("Mouvement")]
    public Image[] mouvementImageP1;
    public Image[] mouvementImageP2;
    public Image[] mouvementImageP3;
    public Image[] mouvementImageP4;

    public Sprite[] spriteSelecOrNoP1;
    public Sprite[] spriteSelecOrNoP2;
    public Sprite[] spriteSelecOrNoP3;
    public Sprite[] spriteSelecOrNoP4;

    private float inputXPlayer2;
    private float inputXPlayer3;
    private float inputXPlayer4;

    [Header("Vibrations")]
    public Selectable vibrations;
    public Image[] vibrationOnOff;
    public Sprite[] spritesSelectedOrNot;
    private bool _isOnVibration = false;

    [Header("Tutorial")]
    public GameObject tutorialPanel;
    private bool _isOnTutorial = false;

    [Header("Credits")]
    public GameObject creditsPanel;
    private bool _isOnCredits = false;

    private float timerMenu = 0;

    private bool _hasOptionOrPlayOpened;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        getMenuInfoScript = GetMenuInformation.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(true);
        selecPanel.SetActive(false);
        optionsPanel.SetActive(false);
        _player = ReInput.players.GetPlayer("Player1");
        Time.timeScale = 1;

        playerNumberSlider.onValueChanged.AddListener(NumberOfPlayersSlider);
        playerNumberSlider.value = getMenuInfoScript.numbersOfPlayers; ;

        allPlayers = new List<Player>();

        for (int i = 0; i < 4; i++)
        {
            allPlayers.Add(ReInput.players.GetPlayer("Player" + (i + 1)));
        }

        otherPlayers = new List<Player>();
        otherPlayers.Add(ReInput.players.GetPlayer("Player" + 1));
    

        readyButton[1].sprite = outSprite;
        readyButton[2].sprite = outSprite;
        readyButton[3].sprite = outSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isOnOptions || _isCharSelecShowing || _isStartGameShowing)
        {
            playerEntity.IsInOptionOrCharacterMenu(true);
            _player.StopVibration();
        }
        else
        {
            playerEntity.IsInOptionOrCharacterMenu(false);

        }

        if (!_theMenuHasBegun && _player.GetButtonUp("Push1"))
        {
            startPanel.SetActive(false);
            _theMenuHasBegun = true;
        }
        if (_theMenuHasBegun)
        {
            if (!_isOnOptions && !_isCharSelecShowing && !_isStartGameShowing && !_isOnCredits && !_hasOptionOrPlayOpened && !_isOnTutorial && _player.GetButtonDown("BackMenu"))
            {
                timerMenu = 0;
                _theMenuHasBegun = false;
                startPanel.SetActive(true);
            }
            timerMenu += Time.deltaTime;
            //Gère à quel joueur attribué quel action
            float inputXPlayer1 = -_player.GetAxis("HorizontalJoy1");
            float inputYPlayer1 = _player.GetAxis("VerticalJoy1");
            Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);
            if (dirPlayer1.magnitude < 0.3f)
            {
                dirPlayer1 = Vector2.zero;
            }
            if (!_isStartGameShowing && !_isOnOptions && !_isCharSelecShowing)
                playerEntity.SetInputX(dirPlayer1);

            //gère apparence boutons vibrations
            if (getMenuInfoScript.GetVibrationsValue())
            {
                vibrationOnOff[0].sprite = spritesSelectedOrNot[1];
                vibrationOnOff[1].sprite = spritesSelectedOrNot[0];
            }
            else
            {
                vibrationOnOff[0].sprite = spritesSelectedOrNot[0];
                vibrationOnOff[1].sprite = spritesSelectedOrNot[1];
            }

            //permet de gérer la surbrillance des boutons de sélections du mode de mouvement
            //selon le booléen du joueur
            #region Affichage Mode Déplacement
            if (getMenuInfoScript.getPlayerMouvementMode()[0])
            {
                mouvementImageP1[0].sprite = spriteSelecOrNoP1[1];
                mouvementImageP1[1].sprite = spriteSelecOrNoP1[2];
            }
            else
            {
                mouvementImageP1[0].sprite = spriteSelecOrNoP1[0];
                mouvementImageP1[1].sprite = spriteSelecOrNoP1[3];
            }
            if (getMenuInfoScript.getPlayerMouvementMode()[1])
            {
                mouvementImageP2[0].sprite = spriteSelecOrNoP2[1];
                mouvementImageP2[1].sprite = spriteSelecOrNoP2[2];
            }
            else
            {
                mouvementImageP2[0].sprite = spriteSelecOrNoP2[0];
                mouvementImageP2[1].sprite = spriteSelecOrNoP2[3];
            }
            if (getMenuInfoScript.numbersOfPlayers >= 3)
            {
                if (getMenuInfoScript.getPlayerMouvementMode()[2])
                {
                    mouvementImageP3[0].sprite = spriteSelecOrNoP3[1];
                    mouvementImageP3[1].sprite = spriteSelecOrNoP3[2];
                }
                else
                {
                    mouvementImageP3[0].sprite = spriteSelecOrNoP3[0];
                    mouvementImageP3[1].sprite = spriteSelecOrNoP3[3];
                }
            }
            if (getMenuInfoScript.numbersOfPlayers == 4)
            {
                if (getMenuInfoScript.getPlayerMouvementMode()[3])
                {
                    mouvementImageP4[0].sprite = spriteSelecOrNoP4[1];
                    mouvementImageP4[1].sprite = spriteSelecOrNoP4[2];
                }
                else
                {
                    mouvementImageP4[0].sprite = spriteSelecOrNoP4[0];
                    mouvementImageP4[1].sprite = spriteSelecOrNoP4[3];
                }
            }
            #endregion

            //Gère à quel joueur attribué quel action

            if (playerEntity.currentFace == 0 && _player.GetButtonUp("Push1") && !_isStartGameShowing && timerMenu >= 0.2 && !_isOnTutorial)
            {
                ShowSelectionChar();
            }
            else if (playerEntity.currentFace == 1 && _player.GetButton("Push1"))
            {
                ExitGame();
            }
            else if (playerEntity.currentFace == 2 && _player.GetButton("Push1"))
            {
                Options();
                _hasOptionOrPlayOpened = true;
            }
            else if (playerEntity.currentFace == 3 && _player.GetButton("Push1"))
            {
                Credits();
                _hasOptionOrPlayOpened = true;
            }

            if (_isCharSelecShowing && _player.GetButtonUp("BackMenu"))
            {
                playerEntity.enabled = true;
                selecPanel.SetActive(false);
                _isCharSelecShowing = false;
                _hasOptionOrPlayOpened = false;
                _secondPlayerIsHere = false;
                _thirdPlayerIsHere = false;
                _fourthPlayerIsHere = false;
            }
            if (_isOnTutorial && _player.GetButtonUp("BackMenu"))
            {
                tutorialPanel.SetActive(false);
                _isOnTutorial = false;
            }

            if (_isOnCredits && _player.GetButtonUp("BackMenu"))
            {
                creditsPanel.SetActive(false);
                _isOnCredits = false;
                _hasOptionOrPlayOpened = false;
            }

            if (_isCharSelecShowing)
            {
                if (allPlayers[1].GetButtonUp("Push2") && !_secondPress)
                {
                    otherPlayers.Add(ReInput.players.GetPlayer("Player" + 2));
                    _secondPress = true;
                    for (int i = 0; i < otherPlayers.Count; i++)
                    {
                        if (otherPlayers[i].name == allPlayers[1].name)
                        {
                            secondPlayerIndex = i;
                        }
                    }
                    switch (secondPlayerIndex)
                    {
                        case 1:
                            _secondPlayerIsHere = true;
                            break;
                        case 2:
                            _thirdPlayerIsHere = true;
                            break;
                        case 3:
                            _fourthPlayerIsHere = true;
                            break;
                    }
                }
                if (allPlayers[2].GetButtonUp("Push3") && !_thirdPress)
                {
                    otherPlayers.Add(ReInput.players.GetPlayer("Player" + 3));
                    _thirdPress = true;
                    for (int i = 0; i < otherPlayers.Count; i++)
                    {
                        if (otherPlayers[i].name == allPlayers[2].name)
                        {
                            thirdPlayerIndex = i;
                        }
                    }
                    switch (thirdPlayerIndex)
                    {
                        case 1:
                            _secondPlayerIsHere = true;
                            break;
                        case 2:
                            _thirdPlayerIsHere = true;
                            break;
                        case 3:
                            _fourthPlayerIsHere = true;
                            break;
                    }
                }
                if (allPlayers[3].GetButtonUp("Push4") && !_fourthPress)
                {
                    otherPlayers.Add(ReInput.players.GetPlayer("Player" + 4));
                    _fourthPress = true;
                    for (int i = 0; i < otherPlayers.Count; i++)
                    {
                        if (otherPlayers[i].name == allPlayers[3].name)
                        {
                            fourthPlayerIndex = i;
                        }
                    }
                    switch (fourthPlayerIndex)
                    {
                        case 1:
                            _secondPlayerIsHere = true;
                            break;
                        case 2:
                            _thirdPlayerIsHere = true;
                            break;
                        case 3:
                            _fourthPlayerIsHere = true;
                            break;
                    }
                }

                if (otherPlayers.Count >= 2)
                {
                    if (otherPlayers[1].GetButtonUp("BackMenu" + (otherPlayers[1].name.Substring(otherPlayers[1].name.Length - 1))) && _secondPlayerIsHere)
                    {
                        switch (int.Parse(otherPlayers[1].name.Substring(otherPlayers[1].name.Length - 1)))
                        {
                            case 2:
                                _secondPress = false;
                                break;
                            case 3:
                                _thirdPress = false;
                                break;
                            case 4:
                                _fourthPress = false;
                                break;
                        }
                        otherPlayers.Remove(ReInput.players.GetPlayer("Player" + (otherPlayers[1].name.Substring(otherPlayers[1].name.Length - 1))));
                        _secondPlayerIsHere = false;
                        readyButton[1].sprite = outSprite;
                        OtherJoin[0].SetActive(true);
                        OtherPlayerName[0].SetActive(false);
                        OtherPlayerNumber[0].SetActive(false);
                        OtherPlayerFace[0].SetActive(false);
                        mouvementImageP2[0].gameObject.SetActive(false);
                        mouvementImageP2[1].gameObject.SetActive(false);
                    }


                    if(otherPlayers.Count >= 3)
                    {
                        if (otherPlayers[2].GetButtonUp("BackMenu" + (otherPlayers[2].name.Substring(otherPlayers[2].name.Length - 1))) && _thirdPlayerIsHere)
                        {
                            switch (int.Parse(otherPlayers[2].name.Substring(otherPlayers[2].name.Length - 1)))
                            {
                                case 2:
                                    _secondPress = false;
                                    break;
                                case 3:
                                    _thirdPress = false;
                                    break;
                                case 4:
                                    _fourthPress = false;
                                    break;
                            }
                            otherPlayers.Remove(ReInput.players.GetPlayer("Player" + (otherPlayers[2].name.Substring(otherPlayers[2].name.Length - 1))));
                            _thirdPlayerIsHere = false;
                            readyButton[2].sprite = outSprite;
                            OtherJoin[1].SetActive(true);
                            OtherPlayerName[1].SetActive(false);
                            OtherPlayerNumber[1].SetActive(false);
                            OtherPlayerFace[1].SetActive(false);
                            mouvementImageP3[0].gameObject.SetActive(false);
                            mouvementImageP3[1].gameObject.SetActive(false);
                        }
                    }


                    if (otherPlayers.Count >= 4)
                    {
                        if (otherPlayers[3].GetButtonUp("BackMenu" + (otherPlayers[3].name.Substring(otherPlayers[3].name.Length - 1))) && _fourthPlayerIsHere)
                        {
                            switch (int.Parse(otherPlayers[3].name.Substring(otherPlayers[3].name.Length - 1)))
                        {
                            case 2:
                                _secondPress = false;
                                break;
                            case 3:
                                _thirdPress = false;
                                break;
                            case 4:
                                _fourthPress = false;
                                break;
                        }
                            otherPlayers.Remove(ReInput.players.GetPlayer("Player" + (otherPlayers[3].name.Substring(otherPlayers[3].name.Length - 1))));
                            _fourthPlayerIsHere = false;
                            readyButton[3].sprite = outSprite;
                            OtherJoin[2].SetActive(true);
                            OtherPlayerName[2].SetActive(false);
                            OtherPlayerNumber[2].SetActive(false);
                            OtherPlayerFace[2].SetActive(false);
                            mouvementImageP4[0].gameObject.SetActive(false);
                            mouvementImageP4[1].gameObject.SetActive(false);
                        }
                    }
                }
                
                if(otherPlayers.Count >= 2)
                {
                    inputXPlayer2 = -otherPlayers[1].GetAxis("HorizontalJoy2");
                    OtherJoin[0].SetActive(false);
                    OtherPlayerName[0].SetActive(true);
                    OtherPlayerNumber[0].SetActive(true);
                    OtherPlayerFace[0].SetActive(true);
                    mouvementImageP2[0].gameObject.SetActive(true);
                    mouvementImageP2[1].gameObject.SetActive(true);
                    readyButton[2].sprite = outSprite;
                    if (otherPlayers.Count >= 3)
                    {
                        //Gère à quel joueur attribué quel action
                        inputXPlayer3 = -otherPlayers[2].GetAxis("HorizontalJoy3");
                        OtherJoin[1].SetActive(false);
                        OtherPlayerName[1].SetActive(true);
                        OtherPlayerNumber[1].SetActive(true);
                        OtherPlayerFace[1].SetActive(true);
                        mouvementImageP3[0].gameObject.SetActive(true);
                        mouvementImageP3[1].gameObject.SetActive(true);
                        readyButton[3].sprite = outSprite;

                        if (otherPlayers.Count >= 4)
                        {
                            //Gère à quel joueur attribué quel action
                            inputXPlayer4 = -otherPlayers[3].GetAxis("HorizontalJoy4");
                            OtherJoin[2].SetActive(false);
                            OtherPlayerName[2].SetActive(true);
                            OtherPlayerNumber[2].SetActive(true);
                            OtherPlayerFace[2].SetActive(true);
                            mouvementImageP4[0].gameObject.SetActive(true);
                            mouvementImageP4[1].gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    readyButton[2].sprite = outSprite;
                }

                if (inputXPlayer1 > 0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(0, true);
                }
                else if (inputXPlayer1 < -0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(0, false);

                }

                if (inputXPlayer2 > 0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(1, true);
                }
                else if (inputXPlayer2 < -0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(1, false);
                }

                if (inputXPlayer3 > 0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(2, true);
                }
                else if (inputXPlayer3 < -0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(2, false);
                }

                if (inputXPlayer4 > 0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(3, true);
                }
                else if (inputXPlayer4 < -0.2f)
                {
                    getMenuInfoScript.setPlayerMouvementMode(3, false);
                }
            }
            if (_isOnOptions && _player.GetButtonDown("BackMenu"))
            {
                playerEntity.enabled = true;
                optionsPanel.SetActive(false);
                _isOnOptions = false;
                _hasOptionOrPlayOpened = false;

            }
            if (_isOnOptions && (EventSystem.current.currentSelectedGameObject.tag == "Vibrations"))
            {
                if (-inputXPlayer1 > 0.2f)
                {
                    getMenuInfoScript.SetVibrationsValue(false);
                }
                else if (-inputXPlayer1 < -0.2f)
                {
                    getMenuInfoScript.SetVibrationsValue(true);
                    Vibration(_player, 0, 1.0f, 0.1f);
                }
            }


            #region Check if everyone ready
            if (!_isPOneReady)
                readyButtonConfirm[0].fillAmount = timerPOne;
            if (!_isPTwoReady)
                readyButtonConfirm[1].fillAmount = timerPTwo;
            if (!_isPThreeReady)
                readyButtonConfirm[2].fillAmount = timerPThree;
            if (!_isPFourReady)
                readyButtonConfirm[3].fillAmount = timerPFour;
            #endregion


            #region How to tell if ready
            //if (_isCharSelecShowing && _player.GetButtonDown("BackMenu"))
            //{
            //    selecPanel.SetActive(false);
            //    //ShowPlayerNumberSelection();
            //    _isPOneReady = false;
            //    _isPTwoReady = false;
            //    _isPThreeReady = false;
            //    _isPFourReady = false;
            //    _isCharSelecShowing = false;

            //}
            //quand les joueurs garde le bouton a appuyé, le timer augmente et, passé un certain temps, le joueur est considérer comme prêt
            //dès que tous les joueurs sont prêts, le jeu se lance
            if (_isCharSelecShowing && _player.GetButton("Push1"))
            {
                timerPOne += Time.deltaTime;
            }
            else if (_isCharSelecShowing && _player.GetButtonUp("Push1"))
            {
                timerPOne = 0;
            }
            if (timerPOne >= 1)
            {
                _isPOneReady = true;
            }
            if (_isPOneReady)
            {
                readyButton[0].sprite = readySprite;
            }
            else
            {
                readyButton[0].sprite = originalSprite;
            }

            if (otherPlayers.Count >= 2)
            {
                if (_isCharSelecShowing && otherPlayers[1].GetButton("Push" + (otherPlayers[1].name.Substring(otherPlayers[1].name.Length - 1))))
                {
                    timerPTwo += Time.deltaTime;
                }
                else if (_isCharSelecShowing && otherPlayers[1].GetButtonUp("Push" + "Push" + (otherPlayers[1].name.Substring(otherPlayers[1].name.Length - 1))))
                {
                    timerPTwo = 0;
                }
                if (timerPTwo >= 1)
                {
                    _isPTwoReady = true;
                }
                if (_isPTwoReady)
                {
                    readyButton[1].sprite = readySprite;
                }
                else
                {
                    readyButton[1].sprite = originalSprite;
                }
            }
            

            if (otherPlayers.Count > 2)
            {
                if (_isCharSelecShowing && otherPlayers[2].GetButton("Push" + (otherPlayers[2].name.Substring(otherPlayers[2].name.Length - 1))))
                {
                    timerPThree += Time.deltaTime;
                }
                else if (_isCharSelecShowing && otherPlayers[2].GetButtonUp("Push" + (otherPlayers[2].name.Substring(otherPlayers[2].name.Length - 1))))
                {
                    timerPThree = 0;
                }
                if (timerPThree >= 1)
                {
                    _isPThreeReady = true;
                }
                if (_isPThreeReady)
                {
                    readyButton[2].sprite = readySprite;
                }
                else
                {
                    readyButton[2].sprite = originalSprite;
                }
            }
            

            if (otherPlayers.Count > 3)
            {
                if (_isCharSelecShowing && otherPlayers[3].GetButton("Push" + (otherPlayers[3].name.Substring(otherPlayers[3].name.Length - 1))))
                {
                    timerPFour += Time.deltaTime;
                }
                else if (_isCharSelecShowing && otherPlayers[3].GetButtonUp("Push" + (otherPlayers[3].name.Substring(otherPlayers[3].name.Length - 1))))
                {
                    timerPFour = 0;
                }
                if (timerPFour >= 1)
                {
                    _isPFourReady = true;
                }
                if (_isPFourReady)
                {
                    readyButton[3].sprite = readySprite;
                }
                else
                {
                    readyButton[3].sprite = originalSprite;
                }
            }

            
            #endregion

            #region Start game when all ready
            if (otherPlayers.Count == 2)
            {
                if (_isPOneReady && _isPTwoReady)
                {
                    StartGame();
                }
            }
            else if (otherPlayers.Count == 3)
            {
                if (_isPOneReady && _isPTwoReady && _isPThreeReady)
                {
                    StartGame();
                }
            }
            else
            {
                if (_isPOneReady && _isPTwoReady && _isPThreeReady && _isPFourReady)
                {
                    StartGame();
                }
            }
            #endregion
        }

    }
    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {
        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowSelectionChar()
    {
        //otherPlayers = new List<Player>();
        //for (int i = 0; i < (int)playerNumberSlider.value -1; i++)
        //{
        //    otherPlayers.Add(ReInput.players.GetPlayer("Player" + (i + 2)));
        //}
        //playerSelection.SetActive(false);
        selecPanel.SetActive(true);
        _isCharSelecShowing = true;
        _isStartGameShowing = false;
    }

    //void ShowPlayerNumberSelection()
    //{
    //    playerSelection.SetActive(true);
    //    _isStartGameShowing = true;
        
    //    numberPlayers.Select();
    //}

    void ExitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
        _isOnOptions = true;
        musicSlider.Select();
    }

    public void Tutorial()
    {
        tutorialPanel.SetActive(true);
        _isOnTutorial = true;
        //_isStartGameShowing = false;
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        _isOnCredits = true;
    }

    public void VibrationToggle(bool isOn)
    {
        getMenuInfoScript.vibrationBool = isOn;
    }

    public void NumberOfPlayersSlider(float number)
    {
        getMenuInfoScript.numbersOfPlayers = (int)number;
    }

    public bool hasOptionOrPlayBeenOpened()
    {
        return _hasOptionOrPlayOpened;
    }

    public void StopVibration(Player _player)
    {
        _player.StopVibration();
    }
}
