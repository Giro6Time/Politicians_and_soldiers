using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToGameObject : MonoBehaviour
{
    public GameObject armyObject;

    public void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            Vector3 spawnPosition = hitInfo.point;
            Instantiate(armyObject, spawnPosition, Quaternion.identity);
        }
    }

}
