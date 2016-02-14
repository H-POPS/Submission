using System;
using System.Collections.Generic;
using UnityEngine;

namespace Custom_Scenery.Decorators
{
    class FlatRideDecorator : IDecorator
    {
        public void Decorate(GameObject go, ParkitectObject PO)
        {
            if (go.GetComponent<Waypoints>())
            {
                go.GetComponent<Waypoints>().waypoints = PO.waypoints;
            }
            else
            {
                go.AddComponent<Waypoints>().waypoints = PO.waypoints;
            }

            CustomFlatRide RA = go.AddComponent<CustomFlatRide>();
            RA.xSize = (int) PO.XSize;
            RA.zSize = (int)PO.ZSize;
            RA.excitementRating = float.Parse(PO.XMLNode["Excitement"].InnerText);
            RA.intensityRating = float.Parse(PO.XMLNode["Intensity"].InnerText);
            RA.nauseaRating = float.Parse(PO.XMLNode["Nausea"].InnerText);
            RestraintRotationController controller = go.AddComponent<RestraintRotationController>();
            controller.closedAngles = Loader.getVector3(PO.XMLNode["RestraintAngle"].InnerText);


        RA.motors = FlatRideLoader.LoadMotors(PO.XMLNode, go, RA);
            RA.phases = FlatRideLoader.LoadPhases(PO.XMLNode,go,RA);
            
            foreach (Phase P in RA.phases)
            {

                
                    Debug.Log("Found phase//");
                    foreach (RideAnimationEvent RAE in P.Events)
                {
                    RAE.Check(RA);
                }
            }
            BasicFlatRideSettings(RA);


        }
        public void BasicFlatRideSettings(FlatRide FlatRideScript)
        {
            FlatRideScript.fenceGO = AssetManager.Instance.rideFenceGO;
            FlatRideScript.entranceGO = AssetManager.Instance.rideEntranceGO;
            FlatRideScript.exitGO = AssetManager.Instance.rideExitGO;
            FlatRideScript.entranceExitBuilderGO = AssetManager.Instance.flatRideEntranceExitBuilderGO;



        }
        public void AddRestraints(GameObject asset, Vector3 closeAngle)
        {
            RestraintRotationController controller = asset.AddComponent<RestraintRotationController>();
            controller.closedAngles = closeAngle;
        }
        public void AddColors(GameObject asset, Color[] C)
        {
            CustomColors cc = asset.AddComponent<CustomColors>();
            cc.setColors(C);

            foreach (Material material in AssetManager.Instance.objectMaterials)
            {
                if (material.name == "CustomColorsDiffuse")
                {

                    asset.GetComponentInChildren<Renderer>().sharedMaterial = material;

                    // Go through all child objects and recolor		
                    Renderer[] renderCollection;
                    renderCollection = asset.GetComponentsInChildren<Renderer>();

                    foreach (Renderer render in renderCollection)
                    {
                        if (render.sharedMaterial.name != "Spring")
                        {
                            render.sharedMaterial = material;
                        }

                    }

                    break;
                }
            }
        }
    }
}
