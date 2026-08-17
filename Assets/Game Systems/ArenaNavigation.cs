using UnityEngine;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshSurface))]
public class ArenaNavigation : MonoBehaviour
{
    void Awake()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
