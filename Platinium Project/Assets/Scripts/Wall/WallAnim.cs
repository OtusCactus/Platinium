using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnim : MonoBehaviour
{
    public GameManager _gameManagerScript;


    public void SetFallingAnimFalse()
    {
        _gameManagerScript.SetPreviousFaceAnimatorFallingFalse();
    }

    public void SetRisingAnimFalse()
    {
        _gameManagerScript.SetPreviousFaceAnimatorRisingFalse();
    }
}
