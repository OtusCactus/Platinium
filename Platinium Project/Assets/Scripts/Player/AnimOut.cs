using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOut : MonoBehaviour
{
    private AttackTest _attackTestScript;

    // Start is called before the first frame update
    void Start()
    {
        _attackTestScript = transform.parent.GetComponent<AttackTest>();
    }

    void AnimationEventOut()
    {
        _attackTestScript.GetAnimationEnd();
    }
}
