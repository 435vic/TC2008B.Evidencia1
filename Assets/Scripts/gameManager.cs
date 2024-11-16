using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject[] spawns;
    public robot[] robots;

    public boxPile spawnRef;

    public int boxNumber = 20;

    int[] positions = new int[]{0, 1, 2, 3, 4};
    // Start is called before the first frame update
    void Start()
    {
        ShufflePositions();
        spawnRobots();
        spawnBoxes();
    }

    void ShufflePositions(){
        int temp_value;
        int random_index;
        int size = positions.Length;
        for (int i = 0; i < size; i++){
            temp_value = positions[i];
            random_index = Random.Range(0, size);
            positions[i] = positions[random_index];
            positions[random_index] = temp_value;
        }
    }

    void spawnRobots(){
        for (int i = 0; i < robots.Length; i++){
            robots[i].transform.position = spawns[positions[i]].transform.position;
            robots[i].transform.rotation = spawns[positions[i]].transform.rotation;
        }
    }

    void spawnBoxes(){
        float xPos = 0.5f;
        float zPos = 1.5f;
        int numOfRow = 0;
        int counter = 0;
        Quaternion defRot = new Quaternion();
        while(counter < boxNumber && numOfRow < 9){
            zPos += Random.Range(1, 4);
            if(zPos <= 6.5){
                counter++;
                Instantiate(
                    spawnRef,
                    new Vector3(xPos, 0.5f, zPos),
                    defRot
                );
            } else {
                zPos = 1.5f;
                numOfRow++;
                xPos++;
            }
            
        }
            
        
    }
}
