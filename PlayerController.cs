using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Transform of the warp points
    public Transform rightWarpPoint;
    public Transform leftWarpPoint;

    // Pacman's movement speed
    public float speed;

    // Variable for game controller
    GameObject gameControllerObject;
    GameController gameController;

    // Tile object for accessing map matrix of bool
    Tile tileMap;

    // Initiate spawn point
    Vector3 spawnPoint = new Vector3(8.5f, 0.6f, 0.5f);
    Vector3 currentDirection;

    int player_x, player_y;

    int layerMask;

    Vector2 currentPosition;
    Vector2 targetPosition;
    Vector3 targetPositionInVec3;
    bool haveTargetPosition;

    private void Start()
    {
        // Find GameController script and reference it to gameController
        gameController = FindObjectOfType<GameController>();

        // Set initial position
        transform.position = spawnPoint;

        // The bool matrix is 21 column by 19 row [21, 19]
        tileMap = FindObjectOfType<Tile>();
        currentPosition = new Vector2(tileMap.MapToTileX(transform.position), tileMap.MapToTileY(transform.position));
        
        // Set initial direction
        currentDirection = transform.forward;

        // Set layerMask bit to walls
        layerMask = (1 << 8);

        // Allow initial target tile checking
        haveTargetPosition = false;
    }
    
    private void Update()
    {
        // Only check direction if pacman is not moving / doesn't have a target tile
        if (!haveTargetPosition)
        {
            /* Check for player's input and set target tile accordingly */
            CheckDirectionInputAndSetTarget();

            /* Check if pacman can move forward, in case player doesn't press any directional key */
            CheckForwardAndSetTarget();
        }
        else
        {
            /* Move pacman forward in current direction until pacman arrive at the next legal tile (target) */
            if (MoveTowardsTarget(targetPositionInVec3))
            {
                // Teleport pacman if pacman arrived at a warp point on the left-side of the map
                if (tileMap.MapToTileX(transform.position) == 0 && tileMap.MapToTileY(transform.position) == 11)
                    transform.position = rightWarpPoint.position;

                // Teleport pacman if pacman arrived at a warp point on the right-side of the map
                if (tileMap.MapToTileX(transform.position) == 18 && tileMap.MapToTileY(transform.position) == 11)
                    transform.position = leftWarpPoint.position;

                // Assign a new current position after pacman arrive at the target tile
                currentPosition = new Vector2(tileMap.MapToTileX(transform.position), tileMap.MapToTileY(transform.position));

                // Allow direction checking after pacman arrive at the target tile
                haveTargetPosition = false;
            }
        }
    }

    /* Check for player's input and set target tile accordingly */
    private void CheckDirectionInputAndSetTarget()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.right))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x) + 1f, 0.6f, MapTileToTransformY(currentPosition.y));
                transform.LookAt(transform.position + Vector3.right);
                haveTargetPosition = true;
            }

        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.left))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x) - 1f, 0.6f, MapTileToTransformY(currentPosition.y));
                transform.LookAt(transform.position + Vector3.left);
                haveTargetPosition = true;
            }
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.forward))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x), 0.6f, MapTileToTransformY(currentPosition.y) + 1f);
                transform.LookAt(transform.position + Vector3.forward);
                haveTargetPosition = true;
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.back))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x), 0.6f, MapTileToTransformY(currentPosition.y) - 1f);
                transform.LookAt(transform.position + Vector3.back);
                haveTargetPosition = true;
            }
        }
    }

    /* Check forward tile  */
    private void CheckForwardAndSetTarget()
    {
        if (transform.forward == Vector3.right)
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.right))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x) + 1f, 0.6f, MapTileToTransformY(currentPosition.y));
                haveTargetPosition = true;
            }
        }
        else if (transform.forward == Vector3.left)
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.left))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x) - 1f, 0.6f, MapTileToTransformY(currentPosition.y));
                haveTargetPosition = true;
            }
        }
        if (transform.forward == Vector3.forward)
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.forward))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x), 0.6f, MapTileToTransformY(currentPosition.y) + 1f);
                haveTargetPosition = true;
            }
        }
        if (transform.forward == Vector3.back)
        {
            if (CanMoveInCurrentDirection(currentPosition, Vector3.back))
            {
                targetPositionInVec3 = new Vector3(MapTileToTransformX(currentPosition.x), 0.6f, MapTileToTransformY(currentPosition.y) - 1f);
                haveTargetPosition = true;
            }
        }
    }

    /* Check to see if the tile in the next direction is a legal tile */
    private bool CanMoveInCurrentDirection(Vector2 position, Vector3 direction)
    {
        // Return a boolean value based on a column-major matrix in Tile object
        return (tileMap.map[(int)(position.y + direction.z), (int)(position.x + direction.x)]);
    }

    /* Move current object to target position, target position can only be one tile ahead */
    public bool MoveTowardsTarget(Vector3 target)
    {
        // If current object has reached target, immediately return
        if (Mathf.Approximately(transform.position.x, target.x) && Mathf.Approximately(transform.position.y, target.y) && Mathf.Approximately(transform.position.z, target.z))
            return true;

        // Move current object smoothly
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

        // Clamp position if overshoot
        if (Mathf.Abs(Vector3.Dot(transform.forward, (target - transform.position).normalized)) + 1 < 0.1f)
            transform.position = new Vector3(target.x, 0.6f, target.y);

        // Return if current object has reached target after moving
        return Mathf.Approximately(transform.position.x, target.x) && Mathf.Approximately(transform.position.y, target.y) && Mathf.Approximately(transform.position.z, target.z);
    }

    /* Map from tile coordinate to Unity coordinate in x-axis */
    public float MapTileToTransformX(float tile_x)
    {
        return tile_x - 0.5f;
    }

    /* Map from tile coordinate to Unity coordinate in y-axis */
    public float MapTileToTransformY(float tile_y)
    {
        return tile_y - 4.5f;
    }

    /* Previous movement implementation, using raycast at every frame */
    /*
        private void Update()
            {
                GetAndSetDirectionAll();

                if (!Physics.Raycast(transform.position, transform.forward, 0.5f, layerMask))
                    transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
            }

            void GetAndSetDirectionAll()
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    currentDirection = SetDirectionAndCheckDetection(Vector3.right, currentDirection);
                    transform.LookAt(transform.position + currentDirection);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    currentDirection = SetDirectionAndCheckDetection(Vector3.left, currentDirection);
                    transform.LookAt(transform.position + currentDirection);
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    currentDirection = SetDirectionAndCheckDetection(Vector3.forward, currentDirection);
                    transform.LookAt(transform.position + currentDirection);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    currentDirection = SetDirectionAndCheckDetection(Vector3.back, currentDirection);
                    transform.LookAt(transform.position + currentDirection);
                }
            }

            Vector3 SetDirectionAndCheckDetection(Vector3 directionToCheck, Vector3 currentDirection)
            {
                if (!Physics.Raycast(transform.position - transform.forward * 0.2f, directionToCheck, 1f, layerMask))
                {
                    return directionToCheck;
                }

                return currentDirection;
            }
    */

    /* Check for collision with dot, super dot, and bonus item */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dot"))
        {
            gameController.AddScore();
            gameController.AddDotCount();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Super Dot"))
        {
            gameController.AddScoreSuper();
            gameController.AddDotCount();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Bonus"))
        {
            // Set pacman game object to false momentarily
            gameObject.SetActive(false);
            Invoke("SetEnable", 0.4f);

            gameController.AddScoreBonus();
            Destroy(other.gameObject);
            
        }
        
    }

    /* Enable pacman game object, to be used with Invoke function */
    private void SetEnable()
    {
        gameObject.SetActive(true);
    }
}
