using System.Collections.Generic;
using UnityEngine;

namespace Custom_Scenery.Decorators
{
    interface IDecorator
    {
        void Decorate(GameObject go, ParkitectObject PO);
    }
}
