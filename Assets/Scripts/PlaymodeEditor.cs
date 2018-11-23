using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaymodeEditor : MonoBehaviour {

    public GameObject anchor;
    public float spacing = .1f;
    public float resolution = 1;
    PathCreator creator;

    private void Start()
    {
        creator = FindObjectOfType<PathCreator>();
    }

    public void DrawAnchors()
    {
        for (int i = 0; i < creator.path.NumPoints; i++)
        {
            Instantiate(anchor, creator.path.points[i], Quaternion.identity, this.gameObject.transform);
        }
    }

}
