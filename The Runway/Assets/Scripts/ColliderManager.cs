using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public List<MeshCollider> airportColliderList;
    public List<MeshCollider> sushiColliderList;
    public List<CapsuleCollider> runwayColliderList;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        switch (gameManager.currentSceneIndex)
        {
            case 0:
                foreach (var collider in airportColliderList)
                {
                    collider.enabled = true;
                }

                foreach (var collider in sushiColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in runwayColliderList)
                {
                    collider.enabled = false;
                }

                break;
            case 1:
                foreach (var collider in airportColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in sushiColliderList)
                {
                    collider.enabled = true;
                }

                foreach (var collider in runwayColliderList)
                {
                    collider.enabled = false;
                }

                break;
            case 2:
                foreach (var collider in airportColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in sushiColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in runwayColliderList)
                {
                    collider.enabled = true;
                }

                break;
            case 3:
                foreach (var collider in airportColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in sushiColliderList)
                {
                    collider.enabled = false;
                }

                foreach (var collider in runwayColliderList)
                {
                    collider.enabled = false;
                }

                break;
        }
    }
}
