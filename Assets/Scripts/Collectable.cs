using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected string collectableName;
    [SerializeField] protected string collectableDescription;
    [SerializeField] protected float value; //Sube la vida, el mana o la experiencia dependiendo del tipo de collectable


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }






}
