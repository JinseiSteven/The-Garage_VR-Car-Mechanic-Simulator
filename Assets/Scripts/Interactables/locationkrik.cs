using UnityEngine;

public class locationkrik : MonoBehaviour
{
    public GameObject krik;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        krik.transform.Translate(new Vector3(0.5f, 0f, 0.5f));
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
