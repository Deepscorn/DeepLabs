// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.Util.Extensions;
using UnityEngine;

public class RotateLeftRightBrute : MonoBehaviour
{

    float lastX = 0;
    const float minVel = 0.0001f;
    
    void Start()
    {
        lastX = transform.position.x;
    }
    
    void Update()
    {
        float vel = transform.position.x - lastX;
        if (vel < -minVel)
            transform.SetAngleY(180);
        else if (vel > minVel)
            transform.SetAngleY(0);
        lastX = transform.position.x;
    }
}
