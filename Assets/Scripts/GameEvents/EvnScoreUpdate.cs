
namespace SpaceInvaders
{
    public class EvnScoreUpdate : FrameLord.EventDispatcher.GameEvent
    {
        public const string Name = "EvnScoreUpdate";

        // Score value increment
        public int scoreIncrementValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public EvnScoreUpdate(int scoreIncrementValue)
        {
            eventName = Name;
            this.scoreIncrementValue = scoreIncrementValue;
        }
    }
}