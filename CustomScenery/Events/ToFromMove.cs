
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class ToFromMove : RideAnimationEvent
{
    [SerializeField]
    public MoverHP rotator;
    [SerializeField]
    public ParkitectObject obj;
    [SerializeField]
    float lastTime;

    public override void Check(CustomFlatRide RA)
    {
        foreach (MoverHP R in RA.motors.OfType<MoverHP>().ToList())
            if (R.Identifier == identifierMotor)
                rotator = R;

        base.Check(RA);
    }
    public override string EventName
    {
        get
        {
            return "To-From Move";
        }
    }
    
    public override void Enter()
    {
        lastTime = Time.realtimeSinceStartup;

        rotator.startToFrom();
        base.Enter();
    }
    public override void Run()
    {

        if (rotator)
        {
            
            
            rotator.tick(Time.deltaTime);
            lastTime = Time.realtimeSinceStartup;
            if (rotator.reachedTarget())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
