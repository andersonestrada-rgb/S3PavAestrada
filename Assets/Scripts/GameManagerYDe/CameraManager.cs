using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject playerA;
    public GameObject playerB;

    public float minSize;
    public float maxSize;

    void Start()
    {

    }

    void Update()
    {        
        // Si el playerA O el playerB han sido destruidos, salimos del Update
        if (playerA == null || playerB == null) return;

        // Si ambos existen, la cámara hace sus cálculos normalmente
        float size = Vector3.Distance(playerA.transform.position, playerB.transform.position) / 2;
        if (size < minSize) size = minSize;
        if (size > maxSize) size = maxSize;

        Vector3 MediumPos = (playerA.transform.position + playerB.transform.position) / 2;

        MediumPos.z = -10;
        Camera.main.transform.position = MediumPos;
        Camera.main.orthographicSize = size;
    }
}