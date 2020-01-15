using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Randomizer", menuName = "Randomizer")]
[System.Serializable]
public class RandomizerArena : ScriptableObject
{
    public List<RandomizerConfig> _arena;

    [System.Serializable]
    public struct WallConfig
    {
        //public int ID;
        //public string label;
        //public string caption;
        //public int idTarget;
        //public string dialogue;
        //public List<int> effects;

        public string wallName;
        public bool isOpenned;
        public bool isBouncy;
        public bool isSticky;
        public bool isIndestructible;
        public bool isConnected;
        public bool isMoving;

        public WallConfig(string name)
        {
            wallName = name;
            isBouncy = false;
            isSticky = false;
            isIndestructible = false;
            isConnected = false;
            isMoving = false;
            isOpenned = false;
        }
    }

    [System.Serializable]
    public struct RandomizerConfig
    {
        //Variable declaration
        public int arenaNumber;
        public string arenaLabel;
        public bool isOpenned;
        public bool proprietiesIsOpenned;
        public List<WallConfig> wallList;

        public RandomizerConfig(int number, string label)
        {
            arenaNumber = number;
            arenaLabel = label;
            isOpenned = false;
            proprietiesIsOpenned = false;
            wallList = new List<WallConfig>();
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RandomizerArena))]
public class RandomizerStructureEditor : Editor
{
    public RandomizerArena _myTarget;

    private bool _isOpenned;
    private bool _isProprietiesOpenned;

    private void OnEnable()
    {
        _myTarget = target as RandomizerArena;
    }

    public override void OnInspectorGUI()
    {

        if (_myTarget == null)
        {
            EditorGUILayout.HelpBox("Script no found !", MessageType.Error);
            return;
        }

        base.OnInspectorGUI();

        if (EditorGUILayout.Foldout(_isOpenned, "Arènes", true))
        {
            _isOpenned = true;

            EditorGUI.indentLevel++;
            int nbElement = _myTarget._arena.Count;
            nbElement = EditorGUILayout.IntField("Size", nbElement);

            if (_myTarget._arena.Count != nbElement)
            {
                while (_myTarget._arena.Count != nbElement)
                {
                    if (_myTarget._arena.Count < nbElement)
                        _myTarget._arena.Add(new RandomizerArena.RandomizerConfig(0, "New Arena"));
                    else
                        _myTarget._arena.RemoveAt(_myTarget._arena.Count - 1);
                }
            }

            for (int i = 0; i < _myTarget._arena.Count; i++)
            {
                EditorGUI.indentLevel++;

                RandomizerArena.RandomizerConfig qc = new RandomizerArena.RandomizerConfig();
                qc = _myTarget._arena[i];

                if (EditorGUILayout.Foldout(qc.isOpenned, qc.arenaLabel, true))
                {
                    qc.isOpenned = true;
                    qc.arenaNumber = EditorGUILayout.IntField("Number", qc.arenaNumber);
                    qc.arenaLabel = EditorGUILayout.TextField("Name", qc.arenaLabel);

                    EditorGUI.indentLevel++;

                    if (EditorGUILayout.Foldout(qc.proprietiesIsOpenned, "Proprieties", true))
                    {
                        qc.proprietiesIsOpenned = true;

                        EditorGUI.indentLevel++;

                        int nbElementAnswer = qc.wallList.Count;
                        nbElementAnswer = EditorGUILayout.IntField("Size", nbElementAnswer);

                        if (qc.wallList.Count != nbElementAnswer)
                        {
                            while (qc.wallList.Count != nbElementAnswer)
                            {
                                if (qc.wallList.Count < nbElementAnswer)
                                    qc.wallList.Add(new RandomizerArena.WallConfig("New Wall"));
                                else
                                    qc.wallList.RemoveAt(qc.wallList.Count - 1);
                            }
                        }

                        for (int j = 0; j < qc.wallList.Count; j++)
                        {
                            EditorGUI.indentLevel++;

                            RandomizerArena.WallConfig qcAnswer = new RandomizerArena.WallConfig();
                            qcAnswer = qc.wallList[j];

                            if (EditorGUILayout.Foldout(qcAnswer.isOpenned, qcAnswer.wallName, true))
                            {
                                qcAnswer.isOpenned = true;
                                qcAnswer.wallName = EditorGUILayout.TextField("Label", qc.wallList[j].wallName);
                                
                            }
                            else
                            {
                                qcAnswer.isOpenned = false;
                            }

                            qc.wallList[j] = qcAnswer;

                            EditorGUI.indentLevel--;
                        }

                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        qc.proprietiesIsOpenned = false;
                    }

                    EditorGUI.indentLevel--;
                }
                else
                {
                    qc.isOpenned = false;
                }

                _myTarget._arena[i] = qc;
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        else
        {
            _isOpenned = false;
        }


        EditorUtility.SetDirty(_myTarget);
    }

}
#endif
