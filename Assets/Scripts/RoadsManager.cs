using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadsManager : MonoBehaviour {

    public GameObject road;
    Dictionary<GameObject, bool> roads = new Dictionary<GameObject, bool>();
    GameObject toRemove;

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

    public void CompleteNetwork()
    {
        int i = 0;
        int h = 0;
        foreach(GameObject g in roads.Keys)
        {
            var snapList = g.GetComponent<RoadSpawn>().snapPointList;
            for (int n = 0; n < snapList.Count; n++)
            {

                var pos = snapList[n].transform.position;
                var curNode = new NodeStreet(pos);

                // Checking for crosses
                var cols = Physics.OverlapSphere(pos, 2, LayerMask.GetMask("sferetta"));
                foreach(Collider c in cols)
                {
                    c.gameObject.transform.position += Vector3.up * 3;
                }









                //// street to go straight
                //curNode.AddStreet(new ArcStreet(pos, snapList[n + 1].transform.position));


                //// if colliding then streets to left and right
                //if (snapList[n].GetComponent<IsCollidingScript>().isColliding)
                //{
                //    snapList[n].transform.position += Vector3.up * 3;

                //    foreach (GameObject snapPoint in snapList[n].GetComponent<IsCollidingScript>().otherSphere.GetComponentInParent<RoadSpawn>().snapPointList)
                //    {
                //        var totalLenght = snapList[n].GetComponent<IsCollidingScript>().otherSphere.GetComponentInParent<RoadSpawn>().snapPointList.Count;
                //        if (snapPoint.GetComponent<IsCollidingScript>().isColliding)
                //            return;

                //        else
                //            h++;
                //    }


                //}




            }
               
        }
    }
}
