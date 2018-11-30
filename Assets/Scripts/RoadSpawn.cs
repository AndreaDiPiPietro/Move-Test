using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadSpawn : MonoBehaviour {

    public bool isEditing = true;
    public GameObject anchorPoint;
    public GameObject ducetto;
    public Button roadButton;
    List<GameObject> anchorList;
    LineRenderer lineRenderer;
    PathCreator pathCreator;
    RoadCreator3D roadCreator;
    
    public GameObject snapPoint;
    public List<GameObject> snapPointList = new List<GameObject>();
    private float snapSpacing = 1f;
    private BoxCollider bCol;


    private void Start()
    {
        anchorList = new List<GameObject>();
        lineRenderer = GetComponent<LineRenderer>();
        pathCreator = GetComponent<PathCreator>();
        roadCreator = GetComponent<RoadCreator3D>();
        bCol = GetComponent<BoxCollider>();
        SpawnAnchor();
    }

    private void Update()
    {
        if (isEditing)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                SpawnAnchor();
            }

            if (anchorList.Count > 1)
            {
                for (int j = 0; j < anchorList.Count; j++)
                {
                    Vector3 adjustedPos = new Vector3(anchorList[j].transform.position.x, 0, anchorList[j].transform.position.z);
                    pathCreator.path.MovePoint(j * 3, adjustedPos);
                    roadCreator.UpdateRoad();

                }
            }
        }

        if (bCol.enabled) 
            bCol.center = anchorList[0].transform.position;
        
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "sferetta" && anchorList.Count == 1)
        {
            var cur = col.gameObject.GetComponentInParent<RoadSpawn>().snapPointList;
            anchorList[0].transform.position = col.transform.position;
            if (Input.GetMouseButton(0) && (col.gameObject.Equals(cur[0]) || col.gameObject.Equals(cur[cur.Count - 1])))
            {
                col.gameObject.GetComponentInParent<RoadSpawn>().OnEnableEditing();

                Destroy(this.gameObject);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                float oldDistance = 100;
                float curDistance = 0;
                bool foundPoint = false;
                Vector3[] pointsIntersection = col.gameObject.GetComponentInParent<RoadCreator3D>().points;
                if (!foundPoint)
                {
                    foreach (Vector3 p in pointsIntersection)
                    {
                        curDistance = Vector3.Distance(anchorList[0].transform.position, p);
                        if (curDistance >= oldDistance)
                        {
                            //Instantiate(snapPoint, p, Quaternion.identity);
                            foundPoint = true;
                            foreach (GameObject g in col.GetComponentInParent<RoadSpawn>().snapPointList)                           
                                Destroy(g);
                            
                            return;
                        }
                        else
                            oldDistance = curDistance;
                    }
                }

                
                


            }
                
        }

    }




    public void OnDisableEditing()  
    {
        //Disabilitiamo gli anchorpoints
        foreach (GameObject g in anchorList)
            g.SetActive(false);

        //Disabilitiamo l'editing
        isEditing = false;



        Vector3[] snapMask = pathCreator.path.CalculateEvenlySpacedPoints(snapSpacing);
        foreach (Vector3 p in snapMask)
            snapPointList.Add(Instantiate(snapPoint, p + new Vector3(0,0.2f,0), Quaternion.identity, transform));

    }

    public void OnEnableEditing()
    {
        //Abilitiamo gli anchorpoints
        foreach (GameObject g in anchorList)
            g.SetActive(true);

        //eliminiamo la snap Mask
        foreach (GameObject g in snapPointList)
            Destroy(g);
        snapPointList.Clear();

        //abilitiamo l'editing
        isEditing = true;
    }




    public void cleanList()
    {
        lineRenderer.positionCount = 0;
        anchorList.Clear();
    }




    public void SpawnAnchor()
    {
        
        var cur = Instantiate(anchorPoint,transform);
        anchorList.Add(cur);

        if (anchorList.Count == 1)
            StartCoroutine(FirstSpawn());
        else if (anchorList.Count > 1)
        {
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
            var cur = new Vector3(anchorList[0].transform.position.x, 0, anchorList[0].transform.position.z);
            pathCreator.CreatePath(cur);
            //SpawnAnchor();
            yield return null;
        }
    }
    



}
