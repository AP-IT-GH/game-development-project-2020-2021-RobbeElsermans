﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pigit.Objects;

namespace Pigit
{
    interface IGameObject: IMoveable
    {
        void Update(GameTime gameTime, Vector2 verplaatsing);
        void Draw(SpriteBatch _spriteBatch);
    }
}
