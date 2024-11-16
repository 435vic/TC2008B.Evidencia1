using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxPile : MonoBehaviour
{
    public GameObject[] boxes;
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        amount = 1;
    }

    public void addBox(){
        amount++;
        boxes[amount-2].SetActive(true);
    }
    public int getBoxes(){
        return amount;
    }
}
