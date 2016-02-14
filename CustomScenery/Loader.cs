using System;
using System.Collections.Generic;
using System.IO;
using Custom_Scenery.Decorators;
using UnityEngine;
using System.Xml;
using System.Linq;
public class Loader : MonoBehaviour
{
    private List<BuildableObject> _sceneryObjects = new List<BuildableObject>();

    public string Path;

    public string Identifier;

    List<ParkitectObject> ParkitectObjects = new List<ParkitectObject>();
    public string modName;
    public string modDiscription;

    public void LoadScenery()
    {
        LoadFromXML();
        try
        {

            GameObject hider = new GameObject();

            char dsc = System.IO.Path.DirectorySeparatorChar;

            using (WWW www = new WWW("file://" + Path + dsc + "assetbundle" + dsc + "mod"))
            {
                if (www.error != null)
                    throw new Exception("Loading had an error:" + www.error);

                AssetBundle bundle = www.assetBundle;

                foreach (ParkitectObject PO in ParkitectObjects)
                {
                    try
                    {

                        Debug.Log("================== [Loading: " + PO.ObjName + "-- InGame Named: " + PO.inGameName + " -HP- ] ==================");
                        GameObject asset = Instantiate(bundle.LoadAsset(PO.inGameName)) as GameObject;
                        
                        switch (PO.type)
                        {
                            case ParkitectObject.ObjType.deco:
                            case ParkitectObject.ObjType.trashbin:
                            case ParkitectObject.ObjType.seating:
                            case ParkitectObject.ObjType.seatingAuto:
                            case ParkitectObject.ObjType.lamp:
                            case ParkitectObject.ObjType.fence:
                                Debug.Log("Scenery is not supported with this template");
                                break;
                            case ParkitectObject.ObjType.FlatRide:
                                (new FlatRideDecorator()).Decorate(asset, PO);
                                break;
                            default:
                                print("Failed");
                                break;
                        }
                        
                        Deco deco = asset.GetComponent<Deco>();
                        if (deco)
                        {

                            deco.categoryTag = PO.XMLNode["category"].InnerText;
                            deco.buildOnGrid = PO.grid;
                        }

                        if (PO.XMLNode["BoudingBoxes"] != null)
                        {

                            XmlNodeList BoudingBoxNodes = PO.XMLNode.SelectNodes("BoudingBoxes/BoudingBox");
                            foreach (XmlNode Box in BoudingBoxNodes)
                            {
                                BoundingBox BB = asset.AddComponent<BoundingBox>();
                                BB.isStatic = false;
                                BB.bounds.min = getVector3(Box["min"].InnerText);
                                BB.bounds.max = getVector3(Box["max"].InnerText);
                                BB.layers = BoundingVolume.Layers.Buildvolume;
                                BB.isStatic = true;
                            }
                        }

                        BuildableObject BO = asset.GetComponent<BuildableObject>();
                        BO.price = PO.price;
                        BO.setDisplayName(PO.inGameName);

                        new RecolorableDecorator().Decorate(asset, PO);
                        DontDestroyOnLoad(asset);

                        BuildableObject buildableObject = asset.GetComponent<BuildableObject>();
                        buildableObject.dontSerialize = true;
                        buildableObject.isPreview = true;
                        AssetManager.Instance.registerObject(buildableObject);
                        _sceneryObjects.Add(asset.GetComponent<BuildableObject>());


                        Debug.Log("Loaded: " + buildableObject.gameObject.name);
                        // hide it from view
                        asset.transform.parent = hider.transform;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);

                        LogException(e);
                    }
                }

                bundle.Unload(false);
            }

            hider.SetActive(false);
        }
        catch (Exception e)
        {
            LogException(e);
        }
    }

    private void LogException(Exception e)
    {
        StreamWriter sw = File.AppendText(Path + @"/mod.log");

        sw.WriteLine(e);

        sw.Flush();

        sw.Close();
    }

    public void UnloadScenery()
    {
        foreach (BuildableObject deco in _sceneryObjects)
        {
            AssetManager.Instance.unregisterObject(deco);
        }
    }


    public void LoadFromXML()
    {

        XmlDocument doc = new XmlDocument();
        string[] files = System.IO.Directory.GetFiles(Path, "*.xml");
        doc.Load(files[0]);
        XmlElement xelRoot = doc.DocumentElement;
        XmlNodeList ModNodes = xelRoot.SelectNodes("/Mod");

        foreach (XmlNode Mod in ModNodes)
        {

            modName = Mod["ModName"].InnerText;
            modDiscription = Mod["ModDiscription"].InnerText;
        }
        XmlNodeList ObjectNodes = xelRoot.SelectNodes("/Mod/Objects/Object");

        foreach (XmlNode ParkOBJ in ObjectNodes)
        {
            ParkitectObject PO = new ParkitectObject();
            PO.ObjName = ParkOBJ["OBJName"].InnerText;
            PO.type = (ParkitectObject.ObjType)Enum.Parse(typeof(ParkitectObject.ObjType), ParkOBJ["Type"].InnerText, true);
            PO.XSize = float.Parse(ParkOBJ["X"].InnerText);
            PO.ZSize = float.Parse(ParkOBJ["Z"].InnerText);
            PO.inGameName = ParkOBJ["inGameName"].InnerText;
            PO.price = Int32.Parse(ParkOBJ["price"].InnerText);
            PO.grid = Convert.ToBoolean(ParkOBJ["grid"].InnerText);
            PO.snapCenter = Convert.ToBoolean(ParkOBJ["snapCenter"].InnerText);
            PO.heightDelta = Int32.Parse(ParkOBJ["heightDelta"].InnerText);
            PO.recolorable = Convert.ToBoolean(ParkOBJ["recolorable"].InnerText);
            if (PO.recolorable)
            {
                PO.color1 = HexToColor(ParkOBJ["Color1"].InnerText);
                PO.color2 = HexToColor(ParkOBJ["Color2"].InnerText);
                PO.color3 = HexToColor(ParkOBJ["Color3"].InnerText);
                PO.color4 = HexToColor(ParkOBJ["Color4"].InnerText);
            }
            

            XmlNodeList WaypointsNodes = ParkOBJ.SelectNodes("Waypoints/Waypoint");
            foreach (XmlNode xndNode in WaypointsNodes)
            {

                Waypoint w = new Waypoint();
                w.isOuter = Convert.ToBoolean(xndNode["isOuter"].InnerText);
                w.isRabbitHoleGoal = Convert.ToBoolean(xndNode["isRabbitHoleGoal"].InnerText);
                if (xndNode["connectedTo"].InnerText != "")
                {
                    w.connectedTo = xndNode["connectedTo"].InnerText.Split(',').ToList().ConvertAll(s => Int32.Parse(s));
                }
                w.localPosition = getVector3(xndNode["localPosition"].InnerText);
                PO.waypoints.Add(w);

            }
            PO.XMLNode = ParkOBJ;
            ParkitectObjects.Add(PO);
        }
        UnityEngine.Debug.Log("Loaded from XML in : " + files[0]);
    }
    public static Vector3 getVector3(string rString)
    {
        string[] temp = rString.Substring(1, rString.Length - 2).Split(',');
        float x = float.Parse(temp[0]);
        float y = float.Parse(temp[1]);
        float z = float.Parse(temp[2]);
        Vector3 rValue = new Vector3(x, y, z);
        return rValue;
    }
    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}

