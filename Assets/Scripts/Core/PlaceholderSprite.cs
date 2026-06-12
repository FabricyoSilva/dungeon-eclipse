using UnityEngine;

namespace DungeonEclipse.Core
{
    /// <summary>
    /// Sprite quadrado branco gerado em runtime (1 unidade de mundo). Tingido
    /// via SpriteRenderer.color. Evita importar assets de arte.
    /// </summary>
    public static class PlaceholderSprite
    {
        private static Sprite _square;

        public static Sprite Square
        {
            get
            {
                if (_square == null)
                {
                    var tex = Texture2D.whiteTexture;
                    _square = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f),
                        tex.width);
                }
                return _square;
            }
        }
    }
}
