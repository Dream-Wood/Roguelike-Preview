using System;
using UnityEngine;
using Random = System.Random;

public class Pedestal : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;
    private Artifacts _artifact;
    private GameObject _obj;

    //TODO: Artifact Struct?
    
    private void OnEnable()
    {
        Array val = Enum.GetValues(typeof(Artifacts));
        Random rnd = new Random();
        int rand = rnd.Next(0, val.Length);
        _artifact = (Artifacts) val.GetValue(rand);
        _obj = Instantiate(objs[rand], transform.position + Vector3.up * 2.5f, Quaternion.identity);
        _obj.transform.parent = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().AddArtifact(_artifact);
            FindObjectOfType<GUIInformation>().ShowTextEvent(_artifact.ToString());
            Destroy(_obj);
            Destroy(this);
        }
    }
}
