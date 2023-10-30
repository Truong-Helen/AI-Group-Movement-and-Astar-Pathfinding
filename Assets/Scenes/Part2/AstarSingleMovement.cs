using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class AstarSingleMovement : MonoBehaviour
{
    public static AstarSingleMovement inst;
    private void Awake()
    {
        inst = this; 
    }

    Node[,] nodes;
    public int nodeCountX, nodeCountZ;
    public float xMin, xMax, zMin, zMax;
    
    
    public RaycastHit hit;
    int startLayerMask;
    int endLayerMask;
    int obstacleLayerMask;

    public GameObject waypointPrefab;
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;
    public Transform waypointParent;

    Vector3 startPointPosition;
    Vector3 endPointPosition;

    List<Node> frontier;
    HashSet<Node> explored;
    List<Node> smoothPath;

    public LineRenderer linePrefab;
    public Transform lineParent;

    // Start is called before the first frame update
    void Start()
    {
        frontier = new List<Node>();
        explored = new HashSet<Node>();

        startLayerMask = 1 << 6;
        endLayerMask = 1 << 7;
        obstacleLayerMask = 1 << 3;

        nodes = new Node[nodeCountX, nodeCountZ];
        // code from slides
        
        // generates grid of nodes
        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountZ; j++)
            {
                nodes[i,j] = new Node();
                float xPos = xMin + i * (xMax - xMin) / (nodeCountX - 1);
                float zPos = zMin + j * (zMax - zMin) / (nodeCountZ - 1);
                nodes[i,j].worldPosition = new Vector3(xPos, 0.5f, zPos);
                nodes[i,j].gridPositionX = i;
                nodes[i,j].gridPositionZ = j;
                //Instantiate(waypointPrefab, nodes[i,j].worldPosition, Quaternion.identity);
            }
        }

        // Draws border of search space
        Debug.DrawLine(nodes[0, 0].worldPosition, nodes[nodeCountX-1, 0].worldPosition, Color.white, float.MaxValue);
        Debug.DrawLine(nodes[nodeCountX-1, 0].worldPosition, nodes[nodeCountX-1, nodeCountZ - 1].worldPosition, Color.white, float.MaxValue);
        Debug.DrawLine(nodes[nodeCountX-1, nodeCountZ - 1].worldPosition, nodes[0, nodeCountZ-1].worldPosition, Color.white, float.MaxValue);
        Debug.DrawLine(nodes[0, nodeCountZ - 1].worldPosition, nodes[0, 0].worldPosition, Color.white, float.MaxValue);

        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && SelectionMgr.inst.selectedEntity != null)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, startLayerMask)) 
            {
                //DestroyStartPoint();
                
                Debug.Log("Starting Position: " + hit.point);
                //startPoint = Instantiate(startPointPrefab, hit.point, Quaternion.identity);
                startPointPosition = hit.point;
                
                //if (endPoint != null)
                //{
                //    Debug.DrawLine(startPoint.transform.position, endPoint.transform.position);
                //    Debug.Log("Path generated");
                //    GeneratePathWaypoints(FindPath());
                // }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, endLayerMask)) 
            {

                DestroyEndPoint();
                
                Debug.Log("Ending Position: " + hit.point);
                endPoint = Instantiate(endPointPrefab, hit.point, Quaternion.identity);
                endPointPosition = hit.point;

                
                if (startPoint != null)
                {
                    Debug.DrawLine(startPoint.transform.position, endPoint.transform.position);
                    Debug.Log("Path generated");
                    GeneratePathWaypoints(FindPath());
                }
            }
        }
         */
        
        if (SelectionMgr.inst.selectedEntity != null)
        {
            startPointPosition = SelectionMgr.inst.selectedEntity.position;
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Starting Position: " + SelectionMgr.inst.selectedEntity.position);
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, endLayerMask))
                {

                    Debug.Log("Ending Position: " + hit.point);
                    endPointPosition = hit.point;

                    Debug.Log("Path generated");
                    GeneratePathWaypoints(FindPath());

                    if (smoothPath.Count > 0)
                    {
                        for (int i = 0; i < smoothPath.Count; i++)
                        {
                            Debug.Log("Move to: " + smoothPath[i].worldPosition);
                            MoveToCheckpoint(smoothPath[i].worldPosition);
                        }
                        //MoveToCheckpoint(smoothPath[1].worldPosition);
                    }
                    
                }
            }
        }
    }


    public List<Node> FindPath()
    {

        /*
         * add startnode to frontier
         * while frontier is not empty
         *      list of nearby available nodes of currentnode = FindNearbyNodes()
         *      if list neaby available nodes is null
         *      
         *      loop through nearby available nodes
         *          currentnode.totalPath
         *          find f score of node
         *          keep track of lowest f score
         *      add currentnode to visited list
         *      
         *      currentnode is now the node with lowest f score
         *      add currentnode to frontier
         * 
         */


        // clear 
        frontier.Clear();
        explored.Clear();

        // find startNode and endNode based on user input
        Node startNode = FindNearestNode(startPointPosition);
        Node endNode = FindNearestNode(endPointPosition);

        // set up start node        
        startNode.gCost = 0;
        startNode.hCost = CalculateHCost(startNode, endNode);
        startNode.parentNode = null;
        CalculateFCost(startNode.parentNode, startNode, endNode);

        frontier.Add(startNode);

        
        while (frontier.Count > 0)
        {

            // if frontier is empty --> then no path was found
            if (frontier.Count == 0)
            {
                Debug.Log("Path not found");
                return null;
            }

            // current node
            Node node = frontier[0];

            // path is found when current node equals end node
            if (node == endNode)
            {
                return GeneratePath(node);
                //return path;
            }

            frontier.RemoveAt(0);
            explored.Add(node);

            // search nodes around current node
            foreach (Node nearbyNode in FindNearbyNodes(node))
            {
                //Debug.Log("checking nearby node");

                // if current node has not been explored or is not planned to be explored --> add to list
                if (!explored.Contains(nearbyNode) && !frontier.Contains(nearbyNode))
                {
                    //Debug.Log("New nearby node");

                    nearbyNode.parentNode = node;
                    nearbyNode.gCost = CalculateGCost(node, nearbyNode);
                    nearbyNode.hCost = CalculateHCost(nearbyNode, endNode);
                    CalculateFCost(node, nearbyNode, endNode);
                    PriorityInsert(nearbyNode);
                }
                // if visiting a node again --> check if it has a better gCost than before
                else if (frontier.Contains(nearbyNode))
                {
                    float potentialGValue = node.gCost + CalculateGCost(node, nearbyNode);
                    if (node.gCost + CalculateGCost(node, nearbyNode) > nearbyNode.gCost)
                    {
                        frontier.Remove(nearbyNode);

                        nearbyNode.parentNode = node;
                        nearbyNode.gCost = potentialGValue;
                        nearbyNode.hCost = CalculateHCost(nearbyNode, endNode);
                        CalculateFCost(node, nearbyNode, endNode);

                        // took out and reinserted node so it can be sorted properly into frontier w/ its new gCost
                        PriorityInsert(nearbyNode); 
                    }
                }
            }
        }

        Debug.Log("No path!");
        return null;

    }

    // works backward from the end to find a path
    public List<Node> GeneratePath(Node endNode)
    {
        Stack<Node> path = new Stack<Node>();

        Node pathNode = endNode;
        while (pathNode != null)
        {
            path.Push(pathNode);
            pathNode = pathNode.parentNode;
        }

        return path.ToList(); // ToList() will reverse the stack so the path is in the correct order
    }

    public List<Node> FindNearbyNodes(Node node)
    {
        List<Node> nearbyNodes = new List<Node>();

        // goes through nodes in all eight directions
        for (int i = -1; i <= 1; i++) 
        {
            for (int j = -1; j <= 1; j++)
            {
                // if its the current node then skip
                if (i == 0 && j == 0)
                    continue;


                int indexX = node.gridPositionX + i;
                int indexZ = node.gridPositionZ + j;
                
                // check if the indices are within bounds
                if (0 <= indexX && indexX <= nodeCountX-1 && 
                    0 <= indexZ && indexZ <= nodeCountZ-1)
                {
                    Node newNode = nodes[indexX, indexZ];
                    

                    nearbyNodes.Add(newNode);
                    if (Physics.Raycast(node.worldPosition,
                        new Vector3(i, 0, j), 
                        out RaycastHit obstacleHit,
                        Vector3.Magnitude(newNode.worldPosition - node.worldPosition), obstacleLayerMask))
                    {
                        //Debug.DrawRay(node.worldPosition, new Vector3(i, 0, j) * 10 , Color.magenta, float.MaxValue);

                        nearbyNodes.Remove(newNode);
                    }
                }
            }
        }

        //Debug.Log($"Amount of available nodes: {nearbyNodes.Count}");
        return nearbyNodes;
    }

    public void PriorityInsert(Node node) // uses frontier queue
    {
        int insertIndex = 0;

        // keeps track of index until there is an fCost larger than the node's fCost
        for (int i = 0; i < frontier.Count; i++)
        {
            if (frontier[i].fCost <= node.fCost)
            {
                insertIndex = i + 1;
            }
        }

        frontier.Insert(insertIndex, node);
    }

    public void ClearWaypoints()
    {
        foreach (Transform waypoint in waypointParent.transform)
        {
            Destroy(waypoint.gameObject);
        }
    }

    public List<Node> GeneratePathWaypoints(List<Node> path)
    {
        smoothPath = new List<Node>();
        int lastIndex = path.Count - 1;

        // filter through path and find the waypoints
        // display waypoints
        ClearWaypoints();
        ClearLines();
        if (path == null)
        {
            Debug.Log("path is null");
        }
        else
        {
            smoothPath.Add(path[0]);

            for (int i = 1; i < path.Count - 1; i++)
            {
                int smoothLastIndex = smoothPath.Count - 1;
                if (Physics.Linecast(smoothPath[smoothLastIndex].worldPosition, path[i+1].worldPosition, obstacleLayerMask))
                {
                    smoothPath.Add(path[i]);
                }
            }

            smoothPath.Add(path[lastIndex]);


            /*
            Debug.Log("Displaying path:");
            Node prevNode = null;
            GameObject waypoint;
            Vector3 additionVector = Vector3.zero;
            
            foreach (Node node in path)
            {
                if (prevNode == null)
                {
                    Debug.Log("This should print once");
                    prevNode = node;
                    waypoint = Instantiate(waypointPrefab, prevNode.worldPosition, Quaternion.identity);
                    waypoint.transform.parent = waypointParent;
                }
                else 
                {
                    if (additionVector == Vector3.zero)
                    {
                        additionVector = node.worldPosition - prevNode.worldPosition;
                        
                    }
                    else
                    {
                        
                        if (!Mathf.Approximately(prevNode.worldPosition.x + additionVector.x, node.worldPosition.x) ||
                            !Mathf.Approximately(prevNode.worldPosition.z + additionVector.z, node.worldPosition.z))
                        {
                            // create waypoint at prevNode
                            waypoint = Instantiate(waypointPrefab, prevNode.worldPosition, Quaternion.identity);
                            waypoint.transform.parent = waypointParent;
                            additionVector = node.worldPosition - prevNode.worldPosition;

                        }
                    }
                }

                prevNode = node;
            }

            waypoint = Instantiate(waypointPrefab, prevNode.worldPosition, Quaternion.identity);
            waypoint.transform.parent = waypointParent;
            */
        }
        return null;
    }
    public void MoveToCheckpoint(Vector3 newPosition)
    {
        Entity381 entity = SelectionMgr.inst.selectedEntity;
        Debug.Log(entity);
        Move m = new Move(entity, newPosition);
        UnitAI ai = entity.GetComponent<UnitAI>();
        ai.AddCommand(m);

    }
    public void ClearLines()
    {
        foreach(Transform line in lineParent)
        {
            Destroy(line.gameObject);
        }
    }
    public void CreateLine(Vector3 p1, Vector3 p2)
    {
        LineRenderer lr = Instantiate<LineRenderer>(linePrefab, lineParent);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
    }

    public Node FindNearestNode(Vector3 position) // Does not account for spawning in obstacles
    {
        //float xPos = xMin + i * (xMax - xMin) / (nodeCountX - 1);
        //float zPos = zMin + j * (zMax - zMin) / (nodeCountZ - 1);

        // Uses the above equations to find i and j (node indices)
        int nodeX = Mathf.RoundToInt((position.x - xMin) / ((xMax - xMin) / (nodeCountX - 1)));
        int nodeZ = Mathf.RoundToInt((position.z - zMin) / ((zMax - zMin) / (nodeCountZ - 1)));

        Node nearestNode = nodes[nodeX,nodeZ];

        //Instantiate(waypointPrefab, nearestNode.worldPosition, Quaternion.identity);

        return nearestNode;
    }

    public float CalculateHCost(Node currentNode, Node goalNode)
    {
        // Uses Manhattan Distance (number of nodes traversed)
        float manhattanDistance = Mathf.Abs(currentNode.gridPositionX - goalNode.gridPositionX) + Mathf.Abs(currentNode.gridPositionZ - goalNode.gridPositionZ);

        //Debug.Log("ManhattanDistance: " + manhattanDistance);

        return manhattanDistance;
    }

    public float CalculateGCost(Node parentNode, Node currentNode)
    {
        // distance formula
        float distance = Vector3.Magnitude(currentNode.worldPosition - parentNode.worldPosition);

        return distance + parentNode.gCost;
    }

    public void CalculateFCost(Node parentNode, Node currentNode, Node goalNode)
    {
        if (parentNode == null)
        {
            currentNode.gCost = 0;
            currentNode.hCost = CalculateHCost(currentNode, goalNode);
            currentNode.fCost = currentNode.hCost;

            return;
        }

        float gValue = CalculateGCost(parentNode, goalNode);

        float hValue = CalculateHCost(currentNode, goalNode);

        float fValue = gValue + hValue;
        currentNode.fCost = fValue;

    }
}
