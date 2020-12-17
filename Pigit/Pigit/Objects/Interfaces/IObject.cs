﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pigit.Animatie;
using Pigit.Movement;
using Pigit.SpriteBuild;
using Pigit.SpriteBuild.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pigit.Objects.Interfaces
{ 
    interface IObject: IMoveable
    {
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch _spriteBatch);
        public Rectangle Rectangle { get; set; }
        public SpriteDefine CurrentSprite { get; }
        public Dictionary<AnimatieTypes, SpriteDefine> Sprites { get; set; }
    }
}
