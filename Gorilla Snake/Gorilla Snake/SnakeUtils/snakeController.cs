using Gorilla_Snake;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class snakeController : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 0f;
    public List<Transform> _segments = new List<Transform>();
    public Transform segprefab;
    public SnakeManager snakeManager;
    public static snakeController Main;
    public float CollisionSize = 0.04f;

    public float movementInterval = 9999999999f;
    private void Start()
    {
        _segments.Add(this.transform);
        Main = this;
    }

    private float movementTimer;

/*    void Update()
    {
*//*        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.down;
            }
        }

        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
        }*/

        /*if (Input.GetKey(KeyCode.E))
        {
            Grow();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            snakeManager.SpawnFoodItem();
        }*//*


    }
    public float segmentSpacing = 0.125f;*/

    private void FixedUpdate()
    {
        movementTimer += Time.deltaTime;
        if (movementTimer >= movementInterval)
        {
            HandleCollision();
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].localPosition = _segments[i - 1].localPosition;
            }

  

            // This is what moves the head
            Vector3 newPosition = transform.localPosition + direction * speed * Time.deltaTime;
            transform.localPosition = newPosition;
            movementTimer = 0f;
        }
    }

    public void UpdateDirection(Vector3 Direction)
    {
        direction = Direction;
    }

    void HandleCollision()
    {
        Vector3 center = transform.position;

        Collider[] colliders = Physics.OverlapSphere(center, CollisionSize);

        foreach (Collider collider in colliders)
        {
            if (collider.name.Contains("Obsticle"))
            {
                if (snakeManager.CurrentGameState == GameStates.Started)
                {
                    snakeManager.ResetGame();
                    colliders.ToList().Clear();
                }
            }
            else if (collider.name.Contains("Food"))
            {
                if (snakeManager.CurrentGameState == GameStates.Started)
                {
                    Grow();
                    Plugin.AudioSpot.PlayOneShot(Plugin.ClaimSound);
                    Destroy(collider.gameObject);
                    colliders.ToList().Clear();
                    snakeManager.foodObjects.Clear();
                    snakeManager.SpawnFoodItem();
                    snakeManager.AddScore(1);
                   
                }
            }

        }
    }

    public void Grow()
    {
        Transform segment = Instantiate(segprefab, this.transform.parent);
        segment.gameObject.SetActive(true);
        segment.localScale = new Vector3(0.124368489f, 0.1211005f, 0.000882391585f);
        _segments.Add(segment);
    }

    /*    public void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision: " + other.name);
            if (other.name.Contains("Obsticle"))
            {
                if (snakeManager.CurrentGameState == GameStates.Started)
                {
                    snakeManager.ResetGame();
                }
            }
            else if (other.name.Contains("Food"))
            {
                if (snakeManager.CurrentGameState == GameStates.Started)
                {
                    Grow();
                    Destroy(other.gameObject);
                    snakeManager.foodObjects.Clear();
                    snakeManager.SpawnFoodItem();
                }
            }*/
}