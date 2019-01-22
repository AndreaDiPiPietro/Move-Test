using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadsManager : MonoBehaviour
{

    public GameObject road;
    public GameObject sferettablu;


    Dictionary<GameObject, bool> roads = new Dictionary<GameObject, bool>();
    GameObject toRemove;


    int lenghtCurStreet;
    Vector3[] curArray;
    List<Vector3> curList = new List<Vector3>();
    List<List<Vector3>> listOfStreets = new List<List<Vector3>>();
    List<Vector3> curStreet = new List<Vector3>();

    List<NodeStreet> nodes = new List<NodeStreet>();

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

    public void CreateNavMeshPoints()
    {
        listOfStreets = new List<List<Vector3>>();
        curStreet = new List<Vector3>();
        foreach (GameObject road in roads.Keys)
        {
            var wayPoints = road.GetComponent<RoadSpawn>().snapPointList;
            for (int snap = 0; snap < wayPoints.Count; snap++)
            {
                if (wayPoints[snap].GetComponent<IsCollidingScript>().isColliding)
                {

                    FinishCurStreet();

                }
                else
                {
                    if (snap + 1 < wayPoints.Count)
                    {
                        Vector3 forward = wayPoints[snap + 1].transform.position - wayPoints[snap].transform.position;
                        Vector3 left = new Vector3(-forward.z,0, forward.x);
                        curStreet.Add(wayPoints[snap].transform.position - left.normalized/2);
                    }
                    else
                    {
                        Vector3 forward = wayPoints[snap].transform.position - wayPoints[snap-1].transform.position;
                        Vector3 left = new Vector3(-forward.z,0, forward.x);
                        curStreet.Add(wayPoints[snap].transform.position - left.normalized/2);
                    }
                }
            }
            FinishCurStreet();
        }
    }


    public void FinishCurStreet()
    {
        var wayPoints = road.GetComponent<RoadSpawn>().snapPointList;
        lenghtCurStreet = curStreet.Count;
        for (int i = lenghtCurStreet - 1; i >= 0; i--)
        {
            curStreet.Add(curStreet[i]);
        }
        for (int i = lenghtCurStreet; i < curStreet.Count; i++)
        {
            if (i + 1 < curStreet.Count)
            {
                Vector3 forward = curStreet[i + 1] - curStreet[i];
                Vector3 left = new Vector3(-forward.z, 0, forward.x);
                curStreet[i] = curStreet[i] - left.normalized;
            }
            else
            {
                Vector3 forward = curStreet[i - 1] - curStreet[i - 2];
                Vector3 left = new Vector3(-forward.z, 0, forward.x);
                Debug.DrawLine(curStreet[i], curStreet[i] - left.normalized, Color.red, Mathf.Infinity);
                curStreet[i] = curStreet[i] - left.normalized;
            }
        }

        foreach (Vector3 v in curStreet)
        {
            Instantiate(sferettablu, v, Quaternion.identity);
        }
        curArray = new Vector3[curStreet.Count];
        curStreet.CopyTo(curArray);
        curList = new List<Vector3>();
        foreach (Vector3 v in curArray)
        {
            curList.Add(v);
        }
        listOfStreets.Add(curList);
        Debug.Log(listOfStreets[0].Count);
        curStreet.Clear();
        Debug.Log(listOfStreets[0].Count);
    }





    public void CompleteNetwork()
    {
        CreateNavMeshPoints();
    }
}      
        
    

