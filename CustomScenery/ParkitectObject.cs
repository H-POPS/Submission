using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Xml;

public class ParkitectObject
{

    internal string ObjName;
    public GameObject gameObject;
    public enum ObjType
    {
        none,
        _,
        deco,
        trashbin,
        seating,
        seatingAuto,
        lamp,
        fence,
        FlatRide
    }
    public float XSize = 1;
    public float ZSize = 1;
    public string inGameName;
    public int price;
    public bool grid;
    public int heightDelta;
    public bool recolorable;
    public Color color1 = new Color(0.95f, 0, 0);
    public Color color2 = new Color(0.32f, 1, 0);
    public Color color3 = new Color(0.11f, .05f, 1);
    public Color color4 = new Color(1, 0, 1);

    public bool snapCenter = true;
    public bool snap;
    public ObjType type = ObjType.none;

    public List<Waypoint> waypoints = new List<Waypoint>();
    public XmlNode XMLNode;
}
