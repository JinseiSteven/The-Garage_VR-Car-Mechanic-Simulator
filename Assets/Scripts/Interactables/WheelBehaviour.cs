using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[Serializable]
struct BoltInfo
{
    public GameObject boltSocket;
    public GameObject fauxBolt;
    public Transform boltStartPosition;
    public Transform boltFinalPosition;
    public Transform keyAttachTransform;
    public GameObject keySocket;
    public float screwRatio;
}

public class WheelBehaviour : MonoBehaviour
{
    [SerializeField] private float screwSpeed = 0.25f;

    [SerializeField] private GameObject keyObject;
    [SerializeField] private GameObject fauxKey;
    [SerializeField] private List<BoltInfo> boltInfos;

    public UnityEvent OnWheelDone;

    private Dictionary<int, BoltInfo> boltInfoDict = new Dictionary<int, BoltInfo>();

    public bool[] doneTable = { false, false, false, false, false };
    public bool wheel_finished = false;

    private int activeId;
    private float fauxKeyStartX;

    private void Start()
    {
        int id = 0;
        foreach (BoltInfo boltInfo in boltInfos)
        {
            boltInfoDict[id] = boltInfo;
            id++;
        }

        fauxKeyStartX = fauxKey.transform.localPosition.x;
    }

    private bool CheckDone()
    {
        foreach (bool status in doneTable)
        {
            if (status == false)
            {
                return false;
            }
        }

        OnWheelDone.Invoke();
        return true;
    }

    public void OnBoltEnter(int id)
    {
        // bolt socket + bolt object dissapears
        // fauxbolt appears
        // keysocket appears

        if (boltInfoDict.ContainsKey(id))
        {
            BoltInfo boltInfo = boltInfoDict[id];

            // set the bolt inactive
            IXRSelectInteractable screw = boltInfo.boltSocket.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
            screw.transform.gameObject.SetActive(false);

            boltInfo.boltSocket.SetActive(false);

            boltInfo.fauxBolt.SetActive(true);
            boltInfo.keySocket.SetActive(true);
        }
    }

    public void OnKeyEnter(int id)
    {
        if (boltInfoDict.ContainsKey(id))
        {
            activeId = id;
            keyObject.SetActive(false);
            boltInfoDict[id].keySocket.SetActive(false);
            SetKeyPosition(boltInfoDict[id]);
        }
    }

    private void OnScrewFinished(int id)
    {
        // key socket active
        // key object active
        // fauxkey inactive

        boltInfoDict[id].keySocket.SetActive(true);
        keyObject.SetActive(true);
        fauxKey.SetActive(false);
        boltInfoDict.Remove(id);
        doneTable[id] = true;

        wheel_finished = CheckDone();
    }

    private void SetKeyPosition(BoltInfo boltInfo)
    {
        Vector3 boltPos = boltInfo.fauxBolt.transform.parent.localPosition;
        fauxKey.transform.localPosition = new Vector3(fauxKeyStartX, boltPos.y, boltPos.z);
        fauxKey.SetActive(true);
    }

    public void TurnScrew(float rotationDelta)
    {
        if (boltInfoDict.ContainsKey(activeId))
        {
            BoltInfo boltInfo = boltInfoDict[activeId];

            Transform fbt = boltInfo.fauxBolt.transform;

            fbt.Rotate(transform.right, -rotationDelta, Space.World);

            float movedistance = screwSpeed * Time.deltaTime;

            if (rotationDelta == 0)
            {
                return;
            }

            Vector3 newPos = fbt.position;

            // move towards or away from the wheel screwhole
            if (rotationDelta > 0)
            {
                newPos = Vector3.MoveTowards(fbt.position, boltInfo.boltFinalPosition.position, movedistance);

            }
            else if (rotationDelta < 0)
            {
                newPos = Vector3.MoveTowards(fbt.position, boltInfo.boltStartPosition.position, movedistance);
            }

            float xOffset = Vector3.Distance(newPos, fbt.position);

            // also move the key ofcourse
            fbt.position = newPos;

            // also move the key ofcourse
            fauxKey.transform.position = Vector3.MoveTowards(fauxKey.transform.position, boltInfo.keyAttachTransform.position, movedistance);

            if (Vector3.Distance(boltInfo.boltFinalPosition.position, fbt.position) < 0.001)
            {
                OnScrewFinished(activeId);
            }
        }
    }
}
