namespace CodeBase.Scripts.Utils
{
    public static class Enums
    {
        public enum GameState
        {
            Loading,
            Lobby,
            Gameplay,
            Victory,
            Defeat
        }

        public enum VcamType
        {
            Lobby,
            Gameplay
        }

        public enum Axis : byte
        {
            None = 0,
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2
        }
    }
}
