
using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class SpinRotater : RideAnimationEvent 
{
    [SerializeField]
    public RotatorHP rotator;
    [SerializeField]
    public bool spin = false;
    [SerializeField]
    public float spins = 1;
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
            return "SpinRotator";
        }
    }
    

    public override void Enter()
    {
        rotator.resetRotations();
        base.Enter();
    }
    public override void Run()
    {
        if (rotator != null)
        {


            rotator.tick(Time.deltaTime);
            if (spin)
            {
                if (rotator.getRotationsCount() >= spins)
                {
                    done = true;
                }
            }
            else
            { done = true;}
            
            base.Run();
        }

    }
}
