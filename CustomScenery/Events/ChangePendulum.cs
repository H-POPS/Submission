﻿using System;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[Serializable]
public class ChangePendulum : RideAnimationEvent 
{
    [SerializeField]
    public PendulumRotatorHP rotator;
    float lastTime;
    public ParkitectObject obj;
    public float Friction = 20f;
    public bool Pendulum;
    private float startPendulumPosition;
    private float windUpAngleTarget = 100f;
    public override void Check(CustomFlatRide RA)
    {
        foreach (PendulumRotatorHP R in RA.motors.OfType<PendulumRotatorHP>().ToList())
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
            return "ChangePendulum";
        }
    }

    public override void Enter()
    {
        if (rotator)
        {
            rotator.setActAsPendulum(Pendulum);
        rotator.angularFriction = Friction;
        done = true;

        }
    }
    public override void Run()
    {
        
            

    }
}
