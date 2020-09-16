
namespace SpaceInvaders
{
    public class EvnPlayerRespawn : FrameLord.EventDispatcher.GameEvent
    {
        public const string Name = "EvnPlayerRespawn";

        /// <summary>
        /// Constructor
        /// </summary>
        public EvnPlayerRespawn()
        {
            eventName = Name;
        }
    }
}
