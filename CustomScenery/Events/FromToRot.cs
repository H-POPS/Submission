
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class FromToRot : RideAnimationEvent
{
    [SerializeField]
    public RotateBetweenHP rotator;
    [SerializeField]
    public ParkitectObject obj;
    [SerializeField]
    float lastTime;

    public override void Check(CustomFlatRide RA)
    {
        foreach (RotateBetweenHP R in RA.motors.OfType<RotateBetweenHP>().ToList())
            if (R.Identifier == identifierMotor)
            {
                rotator = R;
            }
        base.Check(RA);
    }
    public override string EventName
    {
        get
        {
            return "From-To Rot";
        }
    }
  

    public override void Enter()
    {
        lastTime = Time.realtimeSinceStartup;

        rotator.startFromTo();
        base.Enter();
    }
    public override void Run()
    {

        if (rotator)
        {
            
            
            rotator.tick(Time.deltaTime);
            lastTime = Time.realtimeSinceStartup;
            if (rotator.isStopped())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
