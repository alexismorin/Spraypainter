using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Es.InkPainter;

public class Spraycan : MonoBehaviour
{

    [SerializeField]
    private Brush brush;
    [SerializeField]
    Transform sprayPosition;
    [SerializeField]
    ParticleSystem sprayParticleSystem;
    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    CanvasReloader reloadScript;

    [SerializeField]
    bool isSpraying = true;

    void Start()
    {
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    }

    void UpdateColorSelection(bool black, bool white, bool color, float hue)
    {
        if (black)
        {
            brush.Color = Color.black;
        }
        if (white)
        {
            brush.Color = Color.white;
        }
        if (color)
        {
            Color newColor = Color.HSVToRGB(hue / 360f, 1f, 1f);
            brush.Color = newColor;
        }

        var particleModule = sprayParticleSystem.main;
        particleModule.startColor = brush.Color;
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[1].color = brush.Color;
        meshRenderer.materials = newMaterials;


    }


    void Update()
    {

        if (Input.GetButtonDown("XRI_Left_MenuButton"))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        }

        audioSource.volume = Input.GetAxis("XRI_Left_Trigger") + Input.GetAxis("XRI_Right_Trigger");

        float currentWheelSelectionHorizontal = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal") + Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        float currentWheelSelectionVertical = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") + Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");

        if (currentWheelSelectionHorizontal != 0.0f || currentWheelSelectionVertical != 0.0f)
        {
            float currentRotation = Mathf.Atan2(currentWheelSelectionVertical, currentWheelSelectionHorizontal) * Mathf.Rad2Deg;
            UpdateColorSelection(false, false, true, currentRotation);
        }



        if (Input.GetAxis("XRI_Left_Trigger") > 0.5f || Input.GetAxis("XRI_Right_Trigger") > 0.5f)
        {
            isSpraying = true;
        }
        if (Input.GetAxis("XRI_Left_Trigger") < 0.5f && Input.GetAxis("XRI_Right_Trigger") < 0.5f)
        {
            isSpraying = false;
        }
        if (Input.GetButtonDown("XRI_Left_PrimaryButton") || Input.GetButtonDown("XRI_Right_PrimaryButton"))
        {
            UpdateColorSelection(true, false, false, 0f);
        }
        if (Input.GetButtonDown("XRI_Left_SecondaryButton") || Input.GetButtonDown("XRI_Right_SecondaryButton"))
        {
            UpdateColorSelection(false, true, false, 0f);
        }

        if (isSpraying)
        {


            RaycastHit hitInfo;
            if (Physics.Raycast(sprayPosition.transform.position, sprayPosition.forward, out hitInfo, 2f))
            {
                var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                if (paintObject != null)
                {
                    sprayParticleSystem.Emit(5);
                    brush.RotateAngle = Random.Range(0f, 180f);
                    brush.Scale = (0.02f / 0.42f) * hitInfo.distance;
                    paintObject.Paint(brush, hitInfo);

                }


            }


        }


    }
}
