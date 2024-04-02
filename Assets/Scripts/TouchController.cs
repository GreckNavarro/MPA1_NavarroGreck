using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

    // DOUBLE TAP;
    bool tapping = false;
    float lastTapTime = 0;
    float doubleTapThreshold = 0.2f;

    //PRESS AND DRAG
    bool presstouch = false;
   [SerializeField] GameObject FiguraMover;



    //SWIPE
    Vector2 startPos;
    Vector2 endPos;
    [SerializeField] float swipeThreshold = 300f;
    bool endswipe = true;
    bool swiperealizado = false;


    [Header("Pokemon Mecanics")]
    [SerializeField] private Rigidbody myRB;
    [SerializeField] private float _force;






    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myRB.GetComponent<Rigidbody>().isKinematic = true;
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);






            if (touch.phase == TouchPhase.Began)
            {

                startPos = touch.position;
                swiperealizado = false;

                Vector3 funcionPerspective = GetWorldPositionOnPlane(touch.position, -5.85f);

                Debug.Log(funcionPerspective);
                Vector3 origenRaycast = new Vector3(funcionPerspective.x, funcionPerspective.y, -10f);
                Debug.Log(origenRaycast);

                RaycastHit hit;

                Debug.DrawRay(origenRaycast, Vector3.forward * 100, Color.green, 1.0f);

                if (Physics.Raycast(origenRaycast, Vector3.forward * 100, out hit))
                {
                    if(hit.collider.tag == "Pokeball")
                    {
                       

                        Debug.Log("PokeballDetected");
                        FiguraMover = hit.collider.gameObject;
                        presstouch = true;
                    }

                }




                if (!tapping)
                {
                    tapping = true;
                    StartCoroutine(SingleTap(touch.position));
                }


                if ((Time.time - lastTapTime) < doubleTapThreshold)
                {
                    tapping = false;
                    Debug.Log("DoubleTap");
                }
                lastTapTime = Time.time;

            }

            if (touch.phase == TouchPhase.Moved)
            {
                presstouch = true;
                Vector3 funcionPress = GetWorldPositionOnPlane(touch.position, -5.85f);
                PressAndDrag(funcionPress);


            }

            if (touch.phase == TouchPhase.Ended)
            {
                presstouch = false;
                FiguraMover = null;

                endPos = touch.position;
                Vector2 DiferencePosition = endPos - startPos;
                if (DiferencePosition.magnitude > swipeThreshold && endswipe == true)
                {

                    Debug.Log("Swipe Realizado");
                    LaunchPokeball(DiferencePosition, DiferencePosition.magnitude);

                    swiperealizado = true;
                }

                endswipe = true;

            }


        }
    }

    IEnumerator SingleTap(Vector2 touchposition)
    {
        yield return new WaitForSeconds(doubleTapThreshold);
        if (tapping && presstouch == false && swiperealizado == false)
        {
            
            Debug.Log("SingleTap");
         

        }
        tapping = false;
    }


    void PressAndDrag(Vector2 touchPosition)
    {
        Vector3 touchpos = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);
        if (FiguraMover != null)
        {
            FiguraMover.transform.position = touchpos;
            endswipe = false;
            FiguraMover.transform.Rotate(2, 2, 0);
        }
    }

    private void LaunchPokeball(Vector2 direccion, float force)
    {
        Vector2 direccionN = direccion.normalized;
        myRB.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 direction = new Vector3(direccionN.x, direccionN.y, 1.0f);
        _force = force / 5;
        myRB.AddForce(direction * _force);
    }

    private void CurveLunch(Vector2 direccion)
    {

    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }



}
