using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    [SerializeField]
    private Transform centerObject;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float accelerationTime;
    [SerializeField][Range(0.0f, 89.9f)][Tooltip("For wider movements use angles between 80 and 85 degrees")]
    private float angularSpeed; //rotation angle

    private Bounds centerObjectBounds;
    private Material material;
    private float radius;
    private float currentSpeed;
    private ParticleSystem particles;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        PauseMovementController.instance.DistanceMoved = 0;
        centerObjectBounds = new Bounds(centerObject.position, new Vector3(speed / 20, speed / 20, speed / 20)); //5% measurment error for reaching center point
        material = GetComponent<Renderer>().material;
        transform.LookAt(centerObject);       
        currentSpeed = 0.0f;
        particles = GetComponent<ParticleSystem>();

        InitColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
            return;

        UpdateSpeed();
        UpdateMovement();
        UpdateColor();

        if (centerObjectBounds.Contains(transform.position))
        {
            moving = false;
            StartCoroutine(Fireworks());
            return;
        }
    }

    private void InitColor()
    {

        radius = Mathf.Abs(Vector3.Distance(centerObject.position, transform.position));
        Vector3 distanceVector = centerObject.position - transform.position;
        material.color = new Color((distanceVector.normalized.x / 2) + 0.5f,
            (distanceVector.normalized.y / 2) + 0.5f,
            (distanceVector.normalized.z / 2) + 0.5f);
    }

    private void UpdateSpeed()
    {
        if (currentSpeed >= speed)
            return;

        currentSpeed += speed / accelerationTime * Time.deltaTime;
        if (currentSpeed >= speed)
            currentSpeed = speed;
    }

    private void UpdateMovement()
    {
        Vector3 direction = centerObject.position - transform.position;
        direction = Quaternion.Euler(Vector3.up * angularSpeed) * direction;
        float distanceThisFrame = currentSpeed * Time.deltaTime;
        PauseMovementController.instance.DistanceMoved += distanceThisFrame;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    private void UpdateColor()
    {
        Vector3 distanceVector = (centerObject.position - transform.position)/radius;
        material.color = new Color((distanceVector.x / 2) + 0.5f,
            (distanceVector.y / 2) + 0.5f,
            (distanceVector.z / 2) + 0.5f);
    }

    IEnumerator Fireworks()
    {
        Vector3 initialScale = transform.localScale;
        for (float size = 1f; size >= 0; size -= 0.05f)
        {
            transform.localScale = initialScale * size;
            yield return new WaitForSeconds(0.05f);
        }
        particles.Play();
        yield return null;
    }

    public void RestartMovement(bool pause)
    {
        if (pause)
        {
            currentSpeed = 0;
            moving = true;
        }
        else
            moving = false;
    }
}

