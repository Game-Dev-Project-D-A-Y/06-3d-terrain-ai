using UnityEngine;
using UnityEngine.AI;


/**
 * This component represents an NPC that runs randomly between targets.
 * The targets are all the objects with a Target component.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class BraveRunner: MonoBehaviour {

    [Tooltip("A game object whose children have a Target component. Each child represents a target.")]
    [SerializeField] private Transform targetFolder = null;

    [SerializeField] private Transform playerPosition = null;
    private Target[] allTargets = null;

    [Header("For debugging")]
    [SerializeField] private Vector3 currentPlayerPosition;

//furthestTarget   furthestDistance
    [SerializeField] private Target closestTarget = null;
    [SerializeField] private float closestDistance = 1000000f;

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
        closestDistance = 1000000;
        foreach(Target target in allTargets)
        {
            float targetDist = Vector3.Distance(target.transform.position,currentPlayerPosition);
            if(targetDist < closestDistance)
            {
                closestDistance = targetDist;
                closestTarget = target;
            }
        }
        navMeshAgent.SetDestination(closestTarget.transform.position);
    }

    public bool hasPath;
    private void Update() {
        hasPath = navMeshAgent.hasPath;
        if (hasPath) {
            FaceDestination();
        }
        GameObject playerObj = GameObject.Find("Player");
        currentPlayerPosition = playerObj.transform.position;
        SelectNewTarget();
    }

    private void FaceDestination() {
        Vector3 directionToDestination = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToDestination.x, 0, directionToDestination.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Gradual rotation
    }


}
