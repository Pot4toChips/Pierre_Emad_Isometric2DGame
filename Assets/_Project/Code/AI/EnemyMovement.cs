using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    /*// --------- TEST ONLY -----------------
    private void Update()
    {
        if (InputManager.Instance.AttackPressed)
        {
            Vector3 des = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            MoveTo(des);
        }
    }
    // -------------------------------------*/

    public void MoveTo(Vector3 destination)
    {
        _agent.isStopped = false;
        _agent.SetDestination(destination);
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public bool HasReachedDestination()
    {
        if (_agent.pathPending) return false;
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }
}