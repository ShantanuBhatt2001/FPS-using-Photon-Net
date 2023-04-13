using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private PlayerManager target;
    [SerializeField] Text healthText;
    [SerializeField] Text bulletText;
    
    

   
    

        public void SetCanvas( int health,int maxHealth,int currentBullets,int maxBullets)
    {
        healthText.text="Health: "+health.ToString()+"/"+maxHealth.ToString();
        bulletText.text="Bullets: "+currentBullets.ToString()+"/"+maxBullets.ToString();
    }

}
