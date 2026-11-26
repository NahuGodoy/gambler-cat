using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IntocableTracker
{
    public static bool Intocable { get; private set; } = true;

    private static readonly HashSet<string> escenariosConLogro = new HashSet<string>();

    public static event Action<string> OnLogroObtenido; // para mostrar un aviso, si queres

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Inicializar()
    {
        escenariosConLogro.Clear();
        Intocable = true;
        SceneManager.sceneLoaded -= AlCargarEscena;
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    // Cada vez que se carga una escena (incluido Reiniciar) se vuelve a empezar limpio
    private static void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        Intocable = true;
    }

    public static void RegistrarGolpe()
    {
        Intocable = false;
    }

    public static void ReiniciarProgresoDeEscenarios()
    {
        escenariosConLogro.Clear();
        Intocable = true;
    }

    public static bool RegistrarVictoria()
    {
        string escenario = SceneManager.GetActiveScene().name;
        string escenarioNormalizado = escenario.ToLowerInvariant();

        if (escenarioNormalizado.Contains("tutorial")) return false;

        if (AchievementSystem.Instance != null)
        {
            if (escenarioNormalizado.Contains("escenariouno") || escenarioNormalizado.Contains("escenario1") || escenarioNormalizado.Contains("nivel1"))
            {
                AchievementSystem.Instance.Unlock(AchievementSystem.Escenario1CompletedId);
            }

            if (escenarioNormalizado.Contains("escenariodos") || escenarioNormalizado.Contains("escenario2") || escenarioNormalizado.Contains("nivel2"))
            {
                AchievementSystem.Instance.Unlock(AchievementSystem.Escenario2CompletedId);
            }

            if (Intocable)
            {
                AchievementSystem.Instance.Unlock(AchievementSystem.IntocableId);
            }
        }

        if (!Intocable) return false;

        // El logro Intocable se desbloquea con el primer escenario completado sin recibir daño.
        bool nuevoEnEsteEscenario = escenariosConLogro.Add(escenario);

        if (nuevoEnEsteEscenario)
        {
            OnLogroObtenido?.Invoke(escenario);
        }
        Debug.Log("Count escenario: " + escenariosConLogro.Count);
        return nuevoEnEsteEscenario;
    }

    public static bool TieneLogroEn(string escenario) => escenariosConLogro.Contains(escenario);
    public static int CantidadEscenarios => escenariosConLogro.Count;

}
