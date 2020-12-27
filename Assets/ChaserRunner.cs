using UnityEngine;
using UnityEngine.AI;


/**
 * This component represents an NPC that runs randomly between targets.
 * The targets are all the objects with a Target component.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class ChaserRunner: MonoBehaviour {


    [SerializeField] private Transform playerPosition = null;

    [Header("For debugging")]
    [SerializeField] private Vector3 currentPlayerPosition;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float rotationSpeed = 5f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        currentPlayerPosition = playerPosition.position;
        SelectNewTarget();
    }

    private void SelectNewTarget() {

        navMeshAgent.SetDestination(currentPlayerPosition);
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
