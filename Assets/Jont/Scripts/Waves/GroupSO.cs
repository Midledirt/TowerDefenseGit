using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[CreateAssetMenu (menuName = "ScriptableObject/Waves/GrpupSO")]
public class GroupSO : ScriptableObject
{
    [SerializeField] private List<GameObject> creepList;
    [SerializeField] private float timeBetweenCreeps;
    //I do not yet know how to do this, but I want to be able to assign each creep a path.  
    [SerializeField] private PathCreator path1;
    [SerializeField] private PathCreator path2;
    [SerializeField] private PathCreator path3;
}
