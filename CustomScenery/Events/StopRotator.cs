

using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class StopRotator : RideAnimationEvent
{
    [SerializeField]
    public RotatorHP rotator;
    [SerializeField]
    float lastTime;
    [SerializeField]
    public ParkitectObject obj;
    public override void Check(CustomFlatRide RA)
    {
        foreach (RotatorHP R in RA.motors.OfType<RotatorHP>().ToList())
            if (R.Identifier == identifierMotor)
                rotator = R;
        base.Check(RA);
    }
    public override string EventName
    {
        get
        {
            return "StopRotator";
        }
    }
   

    public override void Enter()
    {

        rotator.stop();
        base.Enter();
    }
    public override void Run()
    {

        if (rotator)
        {
            
            
            rotator.tick(Time.deltaTime);
            if (rotator.isStopped())
            {
                done = true;
            }
            base.Run();
        }
        
    }
}
