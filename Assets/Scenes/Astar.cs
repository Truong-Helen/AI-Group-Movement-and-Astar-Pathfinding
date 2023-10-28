using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class Astar : MonoBehaviour
{
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

    GameObject startPoint;
    GameObject endPoint;
    Vector3 startPointPosition;
    Vector3 endPointPosition;

    List<Node> frontier;
    HashSet<Node> explored;
    Dictionary<(int, int), float> nodeFValues;

    // Start is called before the first frame update
    void Start()
    {
        frontier = new List<Node>();
        explored = new HashSet<Node>();
        nodeFValues = new Dictionary<(int, int), float>();
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
                nodes[i,j].worldPosition = new Vector3(xPos, 0, zPos);
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, startLayerMask)) 
            {
                if (startPoint != null)
                {
                    Destroy(startPoint);
                }
                
                
                Debug.Log("Starting Position: " + hit.point);
                startPoint = Instantiate(startPointPrefab, hit.point, Quaternion.identity);
                startPointPosition = hit.point;
                
                if (endPoint != null)
                {
                    Debug.DrawLine(startPoint.transform.position, endPoint.transform.position);
                    Debug.Log("Path generated");
                    GeneratePathWaypoints(FindPath());
                }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, endLayerMask)) 
            {
                if (endPoint != null)
                {
                    Destroy(endPoint);
                }
                
                
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
        frontier.Clear();

        explored.Clear();

        nodeFValues.Clear();

        Node node = FindNearestNode(startPointPosition);
        Node endNode = FindNearestNode(endPointPosition);
        node.fValue = 0;

        frontier.Add(node);
        UpdateFValues(node);
        

        while (frontier.Count > 0)
        {
            if (explored.Count > 100)
            {
                Debug.Log("Encountered infinite loop");
                Debug.Log($"frontier.Count: {frontier.Count}");
                Debug.Log($"nodeFValues.Count: {nodeFValues.Count}");
                Stack<Node> solution = new Stack<Node>();
                Node pathNode = node;
                while (node != null)
                {
                    solution.Push(node);
                    node = node.parentNode;
                }
                return solution.ToList(); 
            }

            if (frontier.Count == 0)
            {
                Debug.Log("Path not found");
                return null;
            }
            node = frontier[0];
            frontier.RemoveAt(0);
            nodeFValues.Remove((node.gridPositionX, node.gridPositionZ));


            if (AtGoal(node, endNode))
            {
                Stack<Node> solution = new Stack<Node>();
                Node pathNode = node;
                while (node != null)
                {
                    solution.Push(node);
                    node = node.parentNode;
                }

                return solution.ToList();
            }
            
            
            explored.Add(node);


            //Debug.Log(node.gridPositionX + ", " + node.gridPositionZ);
            foreach (Node nearbyNode in FindNearbyNodes(node))
            {
                //Debug.Log("checking nearby node");
                if (!explored.Contains(nearbyNode) || !nodeFValues.ContainsKey((nearbyNode.gridPositionX, nearbyNode.gridPositionZ)))
                {
                    //Debug.Log("New nearby node");
                    nearbyNode.parentNode = node;
                    FValue(node, nearbyNode, endNode);
                    PriorityInsert(nearbyNode);
                }
                else if (nodeFValues.ContainsKey((nearbyNode.gridPositionX, nearbyNode.gridPositionZ)))
                {
                    nearbyNode.parentNode = node;
                    UpdateFValues(nearbyNode);
                }
            }
        }

        Debug.Log("No path!");
        return null;

    }

    public void UpdateFValues(Node node)
    {
        nodeFValues[(node.gridPositionX, node.gridPositionZ)] = node.fValue;
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

        Debug.Log($"Amount of available nodes: {nearbyNodes.Count}");
        return nearbyNodes;
    }

    public bool AtGoal(Node node, Node goalNode)
    {
        if (node.worldPosition == goalNode.worldPosition)
        {
            return true;   
        }
        else
        {
            return false;
        }
    }

    public void PriorityInsert(Node node) // uses frontier queue
    {
        if (frontier.Count == 0)
        {
            frontier.Add(node);
            
        }
        else
        {
            for (int i = 0; i <= frontier.Count; i++) 
            {
                if (i == frontier.Count) 
                {
                    frontier.Add(node);
                    break;
                    
                }
                else if (frontier[i].fValue > node.fValue)
                {
                    frontier.Insert(i, node); // we assume that this doesn't replace the node
                    
                    break;
                }
            }

        }
        UpdateFValues(node);


    }

    public List<Node> GeneratePathWaypoints(List<Node> path)
    {
        // filter through path and find the waypoints
        // display waypoints

        if (path == null)
        {
            Debug.Log("path is null");
        }
        else
        {
            Debug.Log("Displaying path:");
            foreach (Node node in path)
            {
                Instantiate(waypointPrefab, node.worldPosition, Quaternion.identity);
            }
        }
        return null;
    }
     
    public Node FindNearestNode(Vector3 position) // Does not account for spawning in obstacles
    {
        //float xPos = xMin + i * (xMax - xMin) / (nodeCountX - 1);
        //float zPos = zMin + j * (zMax - zMin) / (nodeCountZ - 1);

        // Uses the above equations to find i and j (node indices)
        int nodeX = Mathf.RoundToInt((position.x - xMin) / ((xMax - xMin) / (nodeCountX - 1)));
        int nodeZ = Mathf.RoundToInt((position.z - zMin) / ((zMax - zMin) / (nodeCountZ - 1)));

        Node nearestNode = nodes[nodeX,nodeZ];

        Instantiate(waypointPrefab, nearestNode.worldPosition, Quaternion.identity);

        return nearestNode;
    }

    public float HValue(Node parentNode, Node goalNode)
    {
        // Uses Manhattan Distance (number of nodes traversed)
        float manhattanDistance = Mathf.Abs(parentNode.gridPositionX - goalNode.gridPositionX) + Mathf.Abs(parentNode.gridPositionZ - goalNode.gridPositionZ);

        Debug.Log("ManhattanDistance: " + manhattanDistance);

        return manhattanDistance;
    }

    public float GValue(Node parentNode, Node currentNode)
    {
        float distance = Vector3.Magnitude(currentNode.worldPosition - parentNode.worldPosition);

        return distance + parentNode.gValue;
    }

    public void FValue(Node parentNode, Node currentNode, Node goalNode)
    {
        float gValue = GValue(parentNode, goalNode);
        currentNode.gValue = gValue;

        float hValue = HValue(parentNode, goalNode);
        currentNode.hValue = hValue;

        float fValue = gValue + hValue;
        currentNode.fValue = fValue;

    }
}
