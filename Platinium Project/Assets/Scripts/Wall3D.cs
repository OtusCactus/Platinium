using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall3D : MonoBehaviour
{
    //Matilde s'est occupée de ce script
    //Grégoire a travaillé sur Wall, qui permet de gérer la transition de la caméra selon quel mur est détruit
    //
    //ce script permet de gérer la vie du mur et son apparence

    [Header("Propriétés")]
    public float wallLifeMax;
    public float wallLimitVelocity;
    private float wallLife;
    //
    private bool _lastHit = false;

    //Materials
    [Header("Apparence")]
    public Material[] wallAppearance;

    private float _playerVelocityRatio;

    [Header("Camera Shake")]
    public Camera camera;
    public float magnitudeShake;
    public float speedShake;
    public float shakeDuration;

    public int numberWallStateMax;
    private int numberWallState;
    private Vector3 _cameraStartPosition;
    private float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        //set le material du mur par défaut
        GetComponent<MeshRenderer>().material = wallAppearance[0];
        wallLife = wallLifeMax;
    }

    private void Start()
    {
        _cameraStartPosition = camera.transform.position;
        numberWallState = numberWallStateMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (wallLife <= 0)
        {
            _lastHit = true;
            if(numberWallState > numberWallStateMax - 2) ShakeScreen();
            GetComponent<MeshRenderer>().material = wallAppearance[2];
            
        }
        if(wallLife < wallLifeMax && !_lastHit)
        {
            if(numberWallState > numberWallStateMax - 1) ShakeScreen();
            GetComponent<MeshRenderer>().material = wallAppearance[1];
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        _playerVelocityRatio = collision.GetComponent<PlayerEntity>().GetVelocityRatio();
        //
        if (_playerVelocityRatio >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocityRatio < wallLimitVelocity)
        {
            wallLife -= _playerVelocityRatio;
        }
        if (_lastHit)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void ShakeScreen()
    {
        camera.transform.position = new Vector3((Mathf.Cos(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.x, (Mathf.Sin(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.y, _cameraStartPosition.z);
        timer += Time.deltaTime;
        if (timer >= shakeDuration)
        {
            camera.transform.position = _cameraStartPosition;
            numberWallState -= 1;
            timer = 0;
        }
    }
}
