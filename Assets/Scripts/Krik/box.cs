using UnityEngine;

public class ScaleInYDirection : MonoBehaviour
{
    public GameObject targetObject; 
    public float targetScaleY = 2.0f; 
    public float scaleSpeed = 1.0f; 

    private float currentScaleY;

    void Start()
    {
        if (targetObject != null)
        {
            currentScaleY = targetObject.transform.localScale.y;
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            currentScaleY = Mathf.Lerp(currentScaleY, targetScaleY, scaleSpeed * Time.deltaTime);
            targetObject.transform.localScale = new Vector3(targetObject.transform.localScale.x,targetObject.transform.localScale.z, currentScaleY);
        }
    }
}
