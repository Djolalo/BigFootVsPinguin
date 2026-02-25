using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; 
    private Rigidbody rb;
    [SerializeField] 
    private float lowestAngle;
    [SerializeField] 
    private float highestAngle; 
    [SerializeField] 
    private float mouseSensitivity; 
    [SerializeField]
    GameObject cam; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(UnityEngine.InputSystem.InputValue inputVal)
    {
        Vector2 inputVec = inputVal.Get<Vector2>(); 
        this.rb.velocity =  new Vector3(inputVec.x,0,inputVec.y)* moveSpeed*10; 
    }

    // Variables à déclarer en haut de classe
    private float verticalRotation = 0f;

    void OnLook(UnityEngine.InputSystem.InputValue inputVal)
    {
        Vector2 mouseDelta = inputVal.Get<Vector2>();

        // 1. Calcul de la rotation horizontale (Corps)
        float rotationY = mouseDelta.x * mouseSensitivity*Time.deltaTime;

        // 2. Calcul et Limitation de la rotation verticale (Regard)
        // On soustrait souvent car l'axe Y de la souris est inversé par rapport à l'axe X de rotation
        verticalRotation -= mouseDelta.y * mouseSensitivity* Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, lowestAngle, highestAngle);

        // 3. Application
        // Rotation horizontale appliquée au Rigidbody
        Quaternion deltaRotation = Quaternion.Euler(0, rotationY, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // 4. Application verticale (sur la caméra directement) 
        // Si ce script est sur la caméra, utilise localRotation
        this.cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

    }

    void OnAttack(UnityEngine.InputSystem.InputValue inputVal)
    {
    }
}
