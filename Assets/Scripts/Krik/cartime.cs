using UnityEngine;

public class cartime : MonoBehaviour
{
    public GameObject car;
    void Start()
    {
        int i = 0;
        while(i < 200)
        {
            i++;
        }
        car.SetActive(true);


        
    }

}
