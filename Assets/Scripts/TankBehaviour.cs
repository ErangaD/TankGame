using UnityEngine;
using System.Collections;

public class TankBehaviour : MonoBehaviour
{

    private float moveSpeed = 3f;
    private float gridSize = 1f;
    private enum Orientation
    {
        Horizontal,
        Vertical
    };
    private Orientation gridOrientation = Orientation.Horizontal;
    private bool allowDiagonals = false;
    private bool correctDiagonalSpeed = false;
    private Vector2 input;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;
    private float factor;

    private string lastDir;

    public float smooth = 2.0F;
    public float tiltAngle = 360.0F;

    public Mesh mesh;
    public Material material;

    private bool colided;


    void Start()
    {
        
    }

    public void Update()
    {

        Debug.logger.Log(GameObject.FindObjectsOfType(typeof(MonoBehaviour)));

        for (int i = 0; i < 5; i++)
        {
            Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
        }

        rotateTank();
        move(transform);

        if (!isMoving)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (!allowDiagonals)
            {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }
            }

            if (input != Vector2.zero)
            {
                Debug.logger.Log(getKey());
                StartCoroutine(move(transform));
            }
        }
    }

    public void rotateTank()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = new Quaternion(0, 0, 0, 1);

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = new Quaternion(1, 1, 0, 0);

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = new Quaternion(0, 0, 1, 0);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = new Quaternion(0, 0, 1, 1);

        }
    }

    public string getKey()
    {
        string key = null;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            key = "Up";

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            key = "Down";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            key = "Right";
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            key = "Left";
        }

        if (key != null)
        {
            lastDir = key;
        }

        return lastDir;
    }

    public void rotate()
    {

    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize, startPosition.y + System.Math.Sign(input.y) * gridSize);
        }
        else
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize, startPosition.y + System.Math.Sign(input.y) * gridSize);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.7071f;
        }
        else
        {
            factor = 1f;
        }

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
   
}
