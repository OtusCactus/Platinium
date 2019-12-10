using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    public Canvas backgroundCanvas;
    public Image cloud;
    public RectTransform[] spawnPoints;
    //public float xValue;
    //public float minY;
    //public float maxY;
    public int cloudCount;
    public float spawnWait;
    public float startWait;
    public float WaveWait;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < cloudCount; i++)
            {
                Image instatiated;
                //Vector3 spawnPosition = new Vector3(xValue, Random.Range(minY, maxY), 0);
                Quaternion spawnRotation = Quaternion.identity;
                instatiated = Instantiate(cloud);
                //instatiated.rectTransform.anchoredPosition = spawnPosition;
                //instatiated.rectTransform.anchoredPosition = new Vector2 (xValue, Random.Range(minY, maxY));
                instatiated.rectTransform.anchoredPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].anchoredPosition;
                instatiated.transform.SetParent(backgroundCanvas.transform);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(WaveWait);
        }
    }
}
