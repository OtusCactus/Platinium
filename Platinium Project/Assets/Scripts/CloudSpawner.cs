using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    public Canvas backgroundCanvas;
    public Image[] cloud;
    [Header("Cadre de Spawn")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    [Header("Paramètres de Spawn")]
    public int cloudCount;
    public float spawnWait;
    public float startWait;
    public float WaveWait;
    public float timeToDestroy = 2;

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
            for (float arg = minY; arg < maxY; arg++)
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
                    if (Mathf.Abs((posY - cloudsPosY[x])) <= 40)
                    {
                        cloudsPosY.RemoveAt(x);
                        iHaveRemoved = true;
                        howmuchremoved++;
                    }
                }
                instantiated.transform.SetParent(backgroundCanvas.transform, false);
                instantiated.rectTransform.anchoredPosition = new Vector2(Random.Range(minX, maxX), posY);
                Destroy(instantiated, timeToDestroy);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(WaveWait);
        }
    }
}
