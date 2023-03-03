using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HandheldObject : InputMap.IMapActions
{
    void OnAttachedCarrier(carrierSystem carrier);
    void OnEquip();
    void OnUnequip();
}
