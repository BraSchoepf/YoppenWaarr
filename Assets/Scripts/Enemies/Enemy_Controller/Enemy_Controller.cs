// using UnityEngine;
// using UnityEngine.AI;

// public class Enemy_Controller : MonoBehaviour
// {
//     private Transform _target;
//     private NavMeshAgent _agent;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         _agent = GetComponent<NavMeshAgent>();
//         _agent.updateRotation = false;
//         _agent.updateUpAxis = false;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (_target == null) return;
//         _agent.SetDestination(_target.position);
//     }

//     public void SetTarget(Transform target)
//     {
//         _target = target;
//     }
// }

using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _agent;
    private Vector3 _initialPosition;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _initialPosition = transform.position;  // Guardamos la posición inicial
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void Update()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
        }
        else
        {
            _agent.SetDestination(_initialPosition);  // Si no hay target, vuelve al inicio
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void ClearTarget()
    {
        _target = null;
    }
}
