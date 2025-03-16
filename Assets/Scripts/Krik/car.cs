using UnityEngine;

public class MakeVisible : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); 
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                renderer.enabled = true; 
            }
        }
    }
}