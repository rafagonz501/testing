using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public float speed;
    public Transform obj;

    private void Update()
    {
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 tempVect = new Vector3(h, v, 0);
        speed = 10f;
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        obj.transform.position += tempVect;        
    }
}
