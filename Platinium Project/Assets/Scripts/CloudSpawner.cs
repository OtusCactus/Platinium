using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    public Canvas backgroundCanvas;
    public Image[] cloud;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
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

                Image instatiated = Instantiate(cloud[Random.Range(0, cloud.Length)]);
                instatiated.transform.SetParent(backgroundCanvas.transform, false);
                instatiated.rectTransform.anchoredPosition = new Vector2 (Random.Range(minX, maxX), Random.Range(minY, maxY));
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(WaveWait);
        }
    }
}
