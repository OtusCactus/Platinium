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
    public WallList _wallNameList;
    public List<ArenaConfig> _questions;

    [System.Serializable]
    public struct WallConfig
    {
        public int walls;
        public bool isBounc;
        public bool isIndestructibl;
        public bool isConnecte;
        public bool isOpenned;

        public WallConfig(int id)
        {
            walls = 1;
            isOpenned = false;
            isBounc = false;
            isIndestructibl = false;
            isConnecte = false;
        }
    }

    [System.Serializable]
    public struct ArenaConfig
    {
        //Variable declaration
        public int ID;
        public string label;
        public List<WallConfig> wallsNamesList;
        public bool isOpenned;
        public bool wallIsOpenned;

        public ArenaConfig(int id, string lab)
        {
            ID = id;
            label = lab;
            wallsNamesList = new List<WallConfig>();
            isOpenned = false;
            wallIsOpenned = false;
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(RandomizerArena))]
public class QuestionStructureEditor : Editor
{
    public RandomizerArena _myTarget;

    private bool _isOpenned;
    private bool _isAnswerOpenned;
    private bool _isEffectOpenned;

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

        //base.OnInspectorGUI();

        _myTarget._wallNameList = EditorGUILayout.ObjectField("Effect List", _myTarget._wallNameList, typeof(WallList), true) as WallList;

        if (_myTarget._wallNameList != null)
        {
            //serializedObject.Update();
            //SerializedProperty sp = serializedObject.FindProperty("_questions.Array");
            //EditorGUILayout.PropertyField(sp, new GUIContent("Questions"), true);
            //serializedObject.ApplyModifiedProperties();

            if (EditorGUILayout.Foldout(_isOpenned, "Arènes", true))
            {
                _isOpenned = true;

                EditorGUI.indentLevel++;
                int nbElement = _myTarget._questions.Count;
                nbElement = EditorGUILayout.IntField("Size", nbElement);

                if (_myTarget._questions.Count != nbElement)
                {
                    while (_myTarget._questions.Count != nbElement)
                    {
                        if (_myTarget._questions.Count < nbElement)
                            _myTarget._questions.Add(new RandomizerArena.ArenaConfig(0, "New Arena"));
                        else
                            _myTarget._questions.RemoveAt(_myTarget._questions.Count - 1);
                    }
                }

                for (int i = 0; i < _myTarget._questions.Count; i++)
                {
                    EditorGUI.indentLevel++;

                    RandomizerArena.ArenaConfig arenaConfig = new RandomizerArena.ArenaConfig();
                    arenaConfig = _myTarget._questions[i];

                    if (EditorGUILayout.Foldout(arenaConfig.isOpenned, arenaConfig.label, true))
                    {
                        arenaConfig.isOpenned = true;
                        arenaConfig.ID = EditorGUILayout.IntField("ID", arenaConfig.ID);
                        arenaConfig.label = EditorGUILayout.TextField("Label", arenaConfig.label);

                        EditorGUI.indentLevel++;

                        if (EditorGUILayout.Foldout(arenaConfig.wallIsOpenned, "Walls", true))
                        {
                            arenaConfig.wallIsOpenned = true;

                            EditorGUI.indentLevel++;

                            int nbElementAnswer = arenaConfig.wallsNamesList.Count;
                            nbElementAnswer = EditorGUILayout.IntField("Size", nbElementAnswer);

                            if (arenaConfig.wallsNamesList.Count != nbElementAnswer)
                            {
                                while (arenaConfig.wallsNamesList.Count != nbElementAnswer)
                                {
                                    if (arenaConfig.wallsNamesList.Count < nbElementAnswer)
                                        arenaConfig.wallsNamesList.Add(new RandomizerArena.WallConfig(0));
                                    else
                                        arenaConfig.wallsNamesList.RemoveAt(arenaConfig.wallsNamesList.Count - 1);
                                }
                            }

                            for (int j = 0; j < arenaConfig.wallsNamesList.Count; j++)
                            {
                                EditorGUI.indentLevel++;

                                RandomizerArena.WallConfig wallConfig = new RandomizerArena.WallConfig();
                                wallConfig = arenaConfig.wallsNamesList[j];
                                List<string> labelEffects = new List<string>();

                                foreach (WallList.WallName wallNameList in _myTarget._wallNameList._wallNames)
                                {
                                    labelEffects.Add(wallNameList.type.ToString());
                                }

                                if (EditorGUILayout.Foldout(wallConfig.isOpenned, labelEffects[wallConfig.walls].ToString(), true))
                                {
                                    wallConfig.isOpenned = true;
                                    //List<string> labelEffects = new List<string>();

                                    //foreach (WallList.WallName wallNameList in _myTarget._wallNameList._wallNames)
                                    //{
                                    //    labelEffects.Add(wallNameList.type.ToString());
                                    //}

                                    int nbElementEffect = wallConfig.walls;
                                    wallConfig.walls = EditorGUILayout.Popup("Wall : ", wallConfig.walls, labelEffects.ToArray());
                                    
                                    wallConfig.isBounc = EditorGUILayout.Toggle("isBouncy", wallConfig.isBounc);
                                    wallConfig.isIndestructibl = EditorGUILayout.Toggle("isIndestructible", wallConfig.isIndestructibl);
                                    wallConfig.isConnecte = EditorGUILayout.Toggle("isConnected", wallConfig.isConnecte);
                                }
                                else
                                {
                                    wallConfig.isOpenned = false;
                                }

                                arenaConfig.wallsNamesList[j] = wallConfig;

                                EditorGUI.indentLevel--;
                            }

                            EditorGUI.indentLevel--;
                        }
                        else
                        {
                            arenaConfig.wallIsOpenned = false;
                        }

                        EditorGUI.indentLevel--;
                    }
                    else
                    {
                        arenaConfig.isOpenned = false;
                    }

                    _myTarget._questions[i] = arenaConfig;
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                _isOpenned = false;
            }

        }
        EditorUtility.SetDirty(_myTarget);
    }

}
#endif
