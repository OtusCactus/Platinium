using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Name List", menuName = "WallList")]
[System.Serializable]
public class WallList : ScriptableObject
{
    public List<WallName> _wallNames;

    [System.Serializable]
    public struct WallName
    {
        public enum NOM
        {
            WallNorthEast,
            WallNorthWest,
            WallSouthEast,
            WallSouth,
            WallSouthWest
        }
        public NOM type;
    }
}
