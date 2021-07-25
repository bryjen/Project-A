using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int energy { get; private set; }
    
    public void SetEnergy(int newEnergy)
    {
        energy = newEnergy;
        //todo change the value of the text
    }
}
