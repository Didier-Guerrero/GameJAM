using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        // Para detener la reproducci�n del juego en el editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Para cerrar el juego cuando est� en ejecuci�n como aplicaci�n
        Application.Quit();
#endif
    }
}
