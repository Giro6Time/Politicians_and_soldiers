using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DialogUnitGroup", menuName = "DialogData/UnitGroup")]
public class DialogUnitGroupSO : ScriptableObject
{
    public DialogUnitSO[] dialogUnitSOs = new DialogUnitSO[0];
}
