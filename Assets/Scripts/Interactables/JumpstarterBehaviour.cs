using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// the connected cable types
public enum Cable
{
    Black,
    Red
}

public class JumpstarterBehaviour : MonoBehaviour
{
    // we simply assume the jumpstarter starts with both cables connected
    private HashSet<Cable> connectedCables = new HashSet<Cable> { Cable.Black, Cable.Red };

    [SerializeField] private GameObject blackSnapper;
    [SerializeField] private GameObject redSnapper;
    [SerializeField] private GameObject blackSnapperAnchor;
    [SerializeField] private GameObject redSnapperAnchor;

    

    public void ReturnCables()
    {
        redSnapper.transform.position = redSnapperAnchor.transform.position;
        blackSnapper.transform.position = blackSnapperAnchor.transform.position;
    }

    public void AddCable(int cableId)
    {
        Cable cable = (Cable)cableId;
        connectedCables.Add(cable);
    }

    public void RemoveCable(int cableId)
    {
        Cable cable = (Cable)cableId;
        connectedCables.Remove(cable);
    } 

}
