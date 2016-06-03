// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using Assets.Sources.Util.Extensions;

public class RotateLeftRightRigid : MonoBehaviour
{

    Rigidbody2D rigidBody;
    const float minVel = 0.0001f;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
   
    void Update()
    {
        if (rigidBody.velocity.x > minVel)
            transform.SetAngleY(0);
        else if (rigidBody.velocity.x < -minVel)
            transform.SetAngleY(180);
    }
}
