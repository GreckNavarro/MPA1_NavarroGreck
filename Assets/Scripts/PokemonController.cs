using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonController : MonoBehaviour
{
    [SerializeField] private Material pokemonMaterial;
    [SerializeField] private float timeChange;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isAgressive;
    void Start()
    {
        pokemonMaterial.color = Color.green;
        currentTime = timeChange;
    }

    private void Update()
    {

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            isAgressive = !isAgressive;
            ChangeStatePokemon();
            currentTime = timeChange;
        }
        
    }

    public void ChangeStatePokemon()
    {
        if (isAgressive)
        {
            pokemonMaterial.color = Color.red;
        }
        else
        {
            pokemonMaterial.color = Color.green;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Pokeball")
        {
            if (isAgressive)
            {
                Debug.Log("No puedo ser capturado");
            }
            else
            {
                gameObject.SetActive(false);
                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("He sido Capturado");
            }
        }
    }


}
