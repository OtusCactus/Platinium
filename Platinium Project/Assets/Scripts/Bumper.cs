using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    private WallManager _wallManagerScript;
    private Animator _anim;
    public float friction;

    private float _timer;
    private float _timerMax = 0.2f;

    private NewSoundManager _newSoundManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _anim = GetComponent<Animator>();
        _timer = _timerMax;
        _newSoundManagerScript = NewSoundManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_anim.GetBool("isHit"))
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                _timer = _timerMax;
                _anim.SetBool("isHit", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        _anim.SetBool("isHit", true);

        _wallManagerScript.Bounce(player.GetLastFrameVelocity(), collision.GetContact(0).normal, playerRigidbody, player.speed, friction);
        if(_newSoundManagerScript != null)
        _newSoundManagerScript.PlaySound(0, "BumperHit");
    }

}
