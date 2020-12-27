using UnityEngine;
using UnityEngine.AI;


/**
 * This component represents an NPC that runs randomly between targets.
 * The targets are all the objects with a Target component.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class CowardRunner: MonoBehaviour {
    [Tooltip("Minimum time to wait at target between running to the next target")]
    [SerializeField] private float minWaitAtTarget = 7f;

    [Tooltip("Maximum time to wait at target between running to the next target")]
    [SerializeField] private float maxWaitAtTarget = 15f;


    [Tooltip("A game object whose children have a Target component. Each child represents a target.")]
    [SerializeField] private Transform targetFolder = null;

    [SerializeField] private Transform playerPosition = null;
    private Target[] allTargets = null;

    [Header("For debugging")]
    [SerializeField] private Vector3 currentPlayerPosition;

    [SerializeField] private Target furthestTarget = null;
    [SerializeField] private float furthestDistance = -1;
    [SerializeField] private float timeToWaitAtTarget = 0;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float rotationSpeed = 5f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        allTargets = targetFolder.GetComponentsInChildren<Target>(false); // get components in active children only
        Debug.Log("Found " + allTargets.Length + " targets.");
        currentPlayerPosition = playerPosition.position;
        SelectNewTarget();
    }

    private void SelectNewTarget() {
        Debug.Log("SelectNewTarget");
        furthestDistance = 0;
        foreach(Target target in allTargets)
        {
            float targetDist = Vector3.Distance(target.transform.position,currentPlayerPosition);
            if(targetDist > furthestDistance)
            {
                furthestDistance = targetDist;
                furthestTarget = target;
            }
        }
        Debug.Log("New target: " + furthestTarget.name);
        navMeshAgent.SetDestination(furthestTarget.transform.position);
    }

    public bool hasPath;
    private void Update() {
        hasPath = navMeshAgent.hasPath;
        if (hasPath) {
            FaceDestination();
        } else {
            Debug.Log("has no path");
            GameObject playerObj = GameObject.Find("Player");
            currentPlayerPosition = playerObj.transform.position;
            SelectNewTarget();
        }
        
    }

    private void FaceDestination() {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));
        //transform.rotation = lookRotation; // Immediate rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Gradual rotation
    }


}
