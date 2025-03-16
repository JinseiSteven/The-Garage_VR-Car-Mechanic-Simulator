using Unity.VisualScripting;
using UnityEngine;

public class OliepeilBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject olieReceptacle;
    private ReceptacleBehaviour receptacleBehaviour;
    [SerializeField] private GameObject olieAnchor;
    private Transform olieTransform;

    [Range(0f, 1f)]
    [SerializeField] private float minHeight = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float maxHeight = 1;

    private bool _isCleaned = false;

    private void Start()
    {
        olieTransform = olieAnchor.transform;
        receptacleBehaviour = olieReceptacle.GetComponent<ReceptacleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClothRag"))
        {
            _isCleaned = true;
            olieAnchor.SetActive(false);
        }
    }

    public void UpdateOliepeil()
    {
        if (_isCleaned)
        {
            SetHeight(receptacleBehaviour.fillRatio);
            olieAnchor.SetActive(true);
        }
    }

    private void SetHeight(float fillRatio)
    {
        float newHeight = Mathf.Lerp(minHeight, maxHeight, fillRatio);

        olieTransform.localScale = new Vector3(olieTransform.localScale.x, newHeight, olieTransform.localScale.z);
    }
}
