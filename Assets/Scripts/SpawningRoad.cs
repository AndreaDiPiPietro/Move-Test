using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningRoad : MonoBehaviour {

    public GameObject road;
    Dictionary<GameObject, bool> roads = new Dictionary<GameObject, bool>();
    GameObject toRemove;

    public void SpawnRoad()
    {
        roads.Add(Instantiate(road), true);     
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject g in roads.Keys)
                if (roads[g] && g)
                    g.GetComponent<RoadSpawn>().OnDisableEditing();
                else
                    toRemove = g;
            if (toRemove)
                roads.Remove(toRemove);
        }
    }
}
