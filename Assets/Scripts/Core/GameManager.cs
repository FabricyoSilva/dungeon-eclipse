using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonEclipse.Core
{
    public enum GameState { Jogando, Venceu, Perdeu }

    /// <summary>
    /// Orquestra os estados do jogo e os caminhos de vitória, derrota e reinício.
    /// Singleton simples por cena.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState State { get; private set; } = GameState.Jogando;

        public event System.Action OnVictory;
        public event System.Action OnDefeat;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Vitoria()
        {
            if (State != GameState.Jogando) return;
            State = GameState.Venceu;
            Messages.Raise("Você Venceu");
            OnVictory?.Invoke();
        }

        public void Derrota()
        {
            if (State != GameState.Jogando) return;
            State = GameState.Perdeu;
            Messages.Raise("Tente Novamente");
            OnDefeat?.Invoke();
        }

        public void Reiniciar()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
