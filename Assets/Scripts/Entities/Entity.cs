using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsBehavior))]
[AddComponentMenu("Survival Game/Entities/Entity")]
public class Entity : MonoBehaviour
{

    [Header("Misc")]
    public World world;

    public virtual void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public virtual void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;

    }

    // converts position to Chunk Position
    public virtual Vector3Int GetChunkPosition()
    {
        return ChunkPositionUtilities.ToChunkPosition(transform.position, world);
    }

    protected virtual void Awake()
    {
        GetComponents();
    }

    protected virtual void OnEnable()
    {
        AddEntityToWorld();
    }

    protected virtual void OnDisable()
    {
        RemoveEntityFromWorld();
    }


    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        MoveEntity();
    }

    // handles ai / input
    protected virtual void MoveEntity()
    {
    }

    // adds the entity to entites list in the current world
    protected virtual void AddEntityToWorld()
    {
        world.AddEntity(this);
    }

    protected virtual void RemoveEntityFromWorld()
    {
        world.RemoveEntity(this);
    }

    protected virtual void GetComponents()
	{
	}
}
