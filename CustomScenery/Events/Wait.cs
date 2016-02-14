using UnityEngine;
using System;
[Serializable]
[ExecuteInEditMode]
public class Wait : RideAnimationEvent
{
    [SerializeField]
    public float seconds;
    [SerializeField]
    float timeLimit;
    float time;
    public override string EventName
    {
        get
        {
            return "Wait";
        }
    }
   

    public override void Enter()
    {
        timeLimit =  seconds;
        time = 0;
        base.Enter();
    }
    public override void Run()
    {

        time += Time.deltaTime;
        if (time > timeLimit)
        {

            done = true;
        }
        else
        {

        }
        base.Run();
    }

}
