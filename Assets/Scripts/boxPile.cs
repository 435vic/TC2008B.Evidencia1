using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPile : MonoBehaviour
{
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        amount = 1;
    }

    public void addBox(){
        amount++;
    }
    public int getBoxes(){
        return amount;
    }
}
