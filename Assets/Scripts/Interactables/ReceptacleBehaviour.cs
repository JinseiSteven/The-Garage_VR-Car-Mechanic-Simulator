using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

// the possible pourable liquids
public enum Liquid {
    Olie,
    // ruitenwisservloeistof
    RWV,
    //koelvloeistof
    KV
}

public class ReceptacleBehaviour : MonoBehaviour
{
    [Range(0, 1)]
    [DoNotSerialize] public float fillRatio = 0f;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float goalRatio = 0.8f;
    [Range(0.0f, 0.2f)]
    [SerializeField] private float ratioRange = 0.1f;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float startFillRatio;
    [SerializeField] private float fillSpeed = 0.1f;
    [SerializeField] private Liquid correctLiquid;

    [SerializeField] private GameObject liquidObject;

    public bool isOpen = false;
    public bool IsOpen {get { return isOpen;} set { isOpen = value; } }

    public UnityEvent OnReceptacleFilled;
    public UnityEvent<bool> OnGoalUpdate;
    public UnityEvent OnWrongLiquid;

    private Material liquidMaterial;
    private bool isFilling = false;
    private bool isMessedUp = false;
    private bool inRange = false;

    private void Start()
    {
        fillRatio = startFillRatio;

        if (liquidObject != null)
        {
            liquidMaterial = liquidObject.GetComponent<MeshRenderer>().material;
        }

        if (liquidMaterial != null)
        {
            liquidMaterial.SetFloat("_FillRatio", fillRatio);
        }

    }

    public void StartFill(Liquid liquid)
    {
        if (!isOpen) { return; }

        if (liquid != correctLiquid)
        {
            isMessedUp = true;
            isFilling = false;
            OnWrongLiquid.Invoke();
        }

        if (!isFilling && fillRatio < 1f)
        {
            isFilling = true;
            // we can even start making the filling sound here
        }
    }

    public void StopFill()
    {
        if (isFilling)
        {
            isFilling = false;
            // stop the filling sound
        }
    }

    private void Update()
    {
        if (isMessedUp)
        {
            return;
        }

        // we only fill up the receptacle if a liquid is pouring and the receptacle isnt full
        if (isFilling && fillRatio < 1f)
        {

            fillRatio += fillSpeed * Time.deltaTime;
            fillRatio = Mathf.Clamp(fillRatio, 0f, 1f);

            if (liquidMaterial != null)
            {
                liquidMaterial.SetFloat("_FillRatio", fillRatio);
            }

            // if the liquid is in the reccomended range, we invoke selected events
            if (fillRatio > (goalRatio - ratioRange) && fillRatio < (goalRatio + ratioRange))
            {
                inRange = true;
                OnGoalUpdate.Invoke(true);
            }
            // if we then leave this range, we invoke it again
            else if (inRange)
            {
                OnGoalUpdate.Invoke(false);
            }

            // once the receptacle is filled, we invoke the selected events
            if (fillRatio == 1f)
            {
                OnReceptacleFilled.Invoke();
            }
        }
    }
}
