using Player;

namespace DialogSystem
{
    /// <summary>
    /// Interfaz que define un método para la interacción con un objeto en el juego.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Método llamado cuando un jugador interactúa con el objeto.
        /// </summary>
        /// <param name="player">El controlador del jugador que realiza la interacción.</param>
        void Interact(PlayerController player);
    }
}