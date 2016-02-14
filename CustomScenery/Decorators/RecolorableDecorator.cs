using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Custom_Scenery.Decorators
{
    class RecolorableDecorator : IDecorator
    {
        
        public void Decorate(GameObject go, ParkitectObject PO)
        {
            string ShaderName;
            if (go.GetComponent<BuildableObject>() != null && PO.recolorable)
            {
                CustomColors cc = go.AddComponent<CustomColors>();

                List<Color> colors = new List<Color>();
                if (PO.color1 != new Color(0.95f, 0, 0))
                    colors.Add(PO.color1);
                if (PO.color2 != new Color(0.32f, 1, 0))
                    colors.Add(PO.color2);
                if (PO.color3 != new Color(0.95f, 0, 0))
                    colors.Add(PO.color3);
                if (PO.color4 != new Color(1, 0, 1))
                    colors.Add(PO.color4);


                cc.setColors(colors.ToArray());
                ShaderName = "CustomColors" + PO.XMLNode["shader"].InnerText;

            }
            else
            {
                ShaderName = PO.XMLNode["shader"].InnerText;
            }
            foreach (Material material in AssetManager.Instance.objectMaterials)
            {
                if (material.name == ShaderName)
                {
                    SetMaterial(go, material);

                    // edge case for fences
                    Fence fence = go.GetComponent<Fence>();

                    if (fence != null)
                    {
                        if (fence.flatGO != null)
                        {
                            SetMaterial(fence.flatGO, material);
                        }

                        if (fence.postGO != null)
                        {
                            SetMaterial(fence.postGO, material);
                        }
                    }

                    break;
                }
            }
        }

        private void SetMaterial(GameObject go, Material material)
        {
            // Go through all child objects and recolor		
            Renderer[] renderCollection;
            renderCollection = go.GetComponentsInChildren<Renderer>();

            foreach (Renderer render in renderCollection)
            {
                render.sharedMaterial = material;
            }
        }

        
    }
}
