using System;

namespace DungeonEclipse.Core
{
    /// <summary>Canal simples de mensagens curtas para o HUD.</summary>
    public static class Messages
    {
        public static event Action<string> OnMessage;
        public static void Raise(string text) => OnMessage?.Invoke(text);
    }
}
