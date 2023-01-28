using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMovementController : MonoBehaviour
{
    public static PauseMovementController instance { get; private set; }

    [SerializeField]
    private Text counter;
    private bool paused = true;
    public bool Paused { set { paused = value; } }
    private float distanceMoved;
    public float DistanceMoved { get { return distanceMoved; }
                                 set { distanceMoved = value;
                                       UpdateCounter();      }}
    public SpiralMovement spiralMovement;
    


    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !paused)
        {
            StartCoroutine(ShowCounter());
        }
    }

    private void UpdateCounter()
    {
        counter.text = distanceMoved.ToString();
    }

    IEnumerator ShowCounter()
    {
        counter.gameObject.SetActive(true);
        spiralMovement.RestartMovement(false);
        paused = true;

        yield return new WaitForSeconds(5);

        paused = false;
        counter.gameObject.SetActive(false);
        spiralMovement.RestartMovement(true);
    }
}
