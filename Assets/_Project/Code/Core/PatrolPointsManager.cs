using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opitmized patrol manager to control all the patrol point globally.
/// </summary>
public class PatrolPointsManager : MonoBehaviour
{
    public static PatrolPointsManager Instance;


    // Currently Available Patrol Points.
    private List<PatrolPoint> _freePoints = new();
    // busy points the points that are targeted from other enemies.
    private List<PatrolPoint> _busyPoints = new();

    private void Awake()
    {
        // --- Singleton Setup ---
        if (Instance != null && Instance != this)
        {
            // If an instance already exists, destroy this one.
            Destroy(gameObject);
            return;
        }
        // This is the one and only instance.
        Instance = this;


        InitializeActivePoints();
    }

    /// <summary>
    /// Get a patrol point and set as targeted.
    /// </summary>
    /// <returns></returns>
    public PatrolPoint GetRandomPatrolPoint()
    {
        if (_freePoints.Count == 0) return new PatrolPoint(-1, null);

        int newRandomIndex = Random.Range(0, _freePoints.Count);

        return ConfirmTargetedPoint(newRandomIndex);
    }

    /// <summary>
    /// Free the targeted point so others can use.
    /// </summary>
    /// <param name="point">The point stored</param>
    /// <param name="delay">The idle delay after which the point is completly free</param>
    public void FreeExitedPoint(PatrolPoint point, float delay)
    {
        StartCoroutine(ConfirmFreePoint(point, delay));
    }

    /// <summary>
    /// Mark a specific point as TARGETED.
    /// </summary>
    /// <param name="index">The patrol point index from the free list</param>
    private PatrolPoint ConfirmTargetedPoint(int index)
    {
        // Add to the new list, and update the new index of the new list.
        PushBackToList(_busyPoints, _freePoints[index]);

        // Fast Remove from the free points, (update index internally)
        FastRemoveAt(_freePoints, index);

        // Return it in the last index of its new list.
        return _busyPoints[^1];
    }
    /// <summary>
    /// Mark a specific point as FREE after the idle exit.
    /// </summary>
    /// <param name="point">The patrol point that has the updated index from the busy list</param>
    /// /// <param name="delay">The idle delay after which the point is completly free</param>
    private IEnumerator ConfirmFreePoint(PatrolPoint point, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Add to the new list, and update the new index of the new list.
        PushBackToList(_freePoints, point);

        // Fast Remove from the free points, (Update index internally)
        FastRemoveAt(_busyPoints, point.index);
    }

    /// <summary>
    /// Removes an item from a list in O(1) time by swapping it 
    /// with the last item and then removing the last item.
    /// </summary>
    private void FastRemoveAt(List<PatrolPoint> list, int index)
    {
        // Check for valid index and non-empty list
        if (list.Count == 0 || index < 0 || index >= list.Count)
        {
            Debug.LogWarning($"Invalid index {index}. Cannot remove.");
            return;
        }

        // Get the index of the very last item
        int lastIndex = list.Count - 1;

        // An updated copy of the last item after the update.
        // holding the current index, and the transform of the last item.
        PatrolPoint updatedLastItem = new(index, list[lastIndex].transform);

        // Move the last point into the slot of the point we are removing
        list[index] = updatedLastItem;

        // Remove the (now duplicate) last point. This is an O(1) operation.
        list.RemoveAt(lastIndex);
    }
    /// <summary>
    /// Push a point to the end of the list and update index
    /// </summary>
    /// <param name="list"> The list to push back in</param>
    /// <param name="point">the point to add passed value type (as a copy)</param>
    private void PushBackToList(List<PatrolPoint> list, PatrolPoint point)
    {
        // Update the new index of the new list.
        point = new(list.Count, point.transform);

        list.Add(point);
    }

    private void InitializeActivePoints()
    {
        int idx = 0;
        foreach (Transform ch in transform)
        {
            if (ch.gameObject.activeSelf)
                _freePoints.Add(new PatrolPoint(idx++, ch));
        }
    }

    [System.Serializable]
    public struct PatrolPoint
    {
        public int index;
        public Transform transform;

        public PatrolPoint(int index, Transform transform)
        {
            this.index = index; 
            this.transform = transform;
        }
    }


    // ----- DEBUGGING -----
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_freePoints == null || _busyPoints == null) return;

        Gizmos.color = Color.green;
        foreach (PatrolPoint p in _freePoints)
            Gizmos.DrawWireSphere(p.transform.position, 1);

        Gizmos.color = Color.red;
        foreach (PatrolPoint p in _busyPoints)
            Gizmos.DrawWireSphere(p.transform.position, 1);
    }

    public void DebugAllLists()
    {
        for (int i = 0; i < _freePoints.Count; i++)
        {
            PatrolPoint p = _freePoints[i];
            Debug.Log($"FreePoint[{i}]: ( {p.index}, {p.transform} )");
        }

        for (int i = 0; i < _busyPoints.Count; i++)
        {
            PatrolPoint p = _busyPoints[i];
            Debug.Log($"BusyPoint[{i}]: ( {p.index}, {p.transform} )");
        }
    }
#endif
    // ---------------------
}
