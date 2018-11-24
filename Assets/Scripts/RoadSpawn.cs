using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadSpawn : MonoBehaviour {

    public GameObject anchorPoint;
    public GameObject ducetto;
    public Button roadButton;
    List<GameObject> anchorList;
    LineRenderer lineRenderer;
    PathCreator pathCreator;
    RoadCreator3D roadCreator;

    public void cleanList()
    {
        lineRenderer.positionCount = 0;
        anchorList.Clear();
    }
    private void Start()
    {
        anchorList = new List<GameObject>();
        lineRenderer = GetComponent<LineRenderer>();
        pathCreator = GetComponent<PathCreator>();
        roadCreator = GetComponent<RoadCreator3D>();
    }

    public void SpawnAnchor()
    {
        
        var cur = Instantiate(anchorPoint,transform);
        anchorList.Add(cur);

        if (anchorList.Count == 1)
            StartCoroutine(FirstSpawn());
        else if (anchorList.Count > 1)
        {
            Debug.Log("Mo");
            pathCreator.path.AddSegment(cur.transform.position);
        }
    }

    IEnumerator FirstSpawn()
    {
        if (transform.GetChild(1).GetComponent<DragMovement>().IsDragging)
        {
            yield return new WaitForFixedUpdate();
        }
        else
        {
            pathCreator.CreatePath(anchorList[0].transform.position);
            //SpawnAnchor();
            yield return null;
        }
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            SpawnAnchor();
        }

        if (anchorList.Count > 1)
        {
            for (int j = 0; j < anchorList.Count; j++)
            {
                //Debug.Log(" j è uguale a " + j * 3);
                //Debug.Log("la lista di punti è lunga" + (pathCreator.path.points.Count-1));
                Vector3 adjustedPos = new Vector3(anchorList[j].transform.position.x, 0.3f, anchorList[j].transform.position.z);
                pathCreator.path.MovePoint(j * 3, adjustedPos);
                roadCreator.UpdateRoad();

            }
        }

    }
}
