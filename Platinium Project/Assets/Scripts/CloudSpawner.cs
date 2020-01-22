using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    public Canvas backgroundCanvas;
    public Image[] cloud;
    [Header("Cadre de Spawn")]
    public Image pivot;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    [Header("Paramètres de Spawn")]
    public int cloudCount;
    public float spawnWait;
    public float startWait;
    public float WaveWait;
    public float spaceBetweenClouds;

    private float _minPosY = 0;
    private float _maxPosY = 0;

    private void Awake()
    {
        _minPosY = pivot.rectTransform.anchoredPosition.y + minY;
        _maxPosY = pivot.rectTransform.anchoredPosition.y + maxY;
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            Image[] clouds = new Image[cloudCount];
            //créer une liste de toutes les positions possibles entre le minY et maxY
            List<float> cloudsPosY = new List<float>();
            for (float arg = _minPosY; arg < _maxPosY; arg++)
            {
                cloudsPosY.Add(arg);
            }
            for (int i = 0; i < cloudCount; i++)
            {
                Image instantiated = Instantiate(cloud[Random.Range(0, cloud.Length)]);
                clouds[i] = instantiated;
                int index = Random.Range(0, cloudsPosY.Count);
                float posY = cloudsPosY[index];
                bool iHaveRemoved = false;
                int howmuchremoved = 0;
                //boucle qui permet d'enlever de la listre de positions possibles celle que vient de prendre le nuages,
                //ainsi que toutes les autres jusqu'à une distance de 40
                for (int x = 0; x < cloudsPosY.Count; x++)
                {
                    if(iHaveRemoved)
                    {
                        x--;
                        iHaveRemoved = false;
                    }
                    if (Mathf.Abs((posY - cloudsPosY[x])) <= spaceBetweenClouds)
                    {
                        cloudsPosY.RemoveAt(x);
                        iHaveRemoved = true;
                        howmuchremoved++;
                    }
                }
                instantiated.transform.SetParent(backgroundCanvas.transform, false);
                instantiated.rectTransform.anchoredPosition = new Vector2(Random.Range(pivot.rectTransform.anchoredPosition.x + minX, pivot.rectTransform.anchoredPosition.x + maxX), posY);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(WaveWait);
        }
    }
}
