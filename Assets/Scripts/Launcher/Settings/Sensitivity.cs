using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivity : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] InputField sensitivity;
   float sens=1;

    void Start()
    {
        if(PlayerPrefs.HasKey("sensitivity"))
        {
            sens=PlayerPrefs.GetFloat("sensitivity");
            sensitivity.text=sens.ToString();
            
        }
        else{
            sensitivity.text=sens.ToString();
            onsensitivityValueChanged();
        }
    }
    public void onsensitivityValueChanged()
    {
        
        PlayerPrefs.SetFloat("sensitivity",float.Parse(sensitivity.text));
    }
}
