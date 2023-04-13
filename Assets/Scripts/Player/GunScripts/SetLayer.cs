using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Awake()
    {
        SetLayer[] components = GameObject.FindObjectsOfType<SetLayer>();
        foreach(SetLayer comp in components)
            {
                if(playerMovement.PV.IsMine)
                {
                    foreach (Transform child in comp.transform)
                    {
                        child.gameObject.layer=LayerMask.NameToLayer("Weapon");
                    }
                }
                
            }
    }

   
}
