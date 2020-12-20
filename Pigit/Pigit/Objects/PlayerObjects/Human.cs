﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pigit.Animatie;
using Pigit.Objects;
using Pigit.Objects.Abstracts;
using Pigit.SpriteBuild;
using Pigit.SpriteBuild.Enums;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pigit.Objects.PlayerObjects
{
    class Human : APlayerObject
    {
        public Human(Dictionary<AnimatieTypes, SpriteDefine> spriteOpbouw, Vector2 beginPosition) :base(spriteOpbouw, beginPosition)
        {

        }
        protected override void RectBuild()
        {
            Rectangle = new Rectangle((int)Positie.X + 36, (int)Positie.Y + 20, CurrentSprite.AnimatieL.CurrentFrame.SourceRect.Width - 45, CurrentSprite.AnimatieL.CurrentFrame.SourceRect.Height - 33);
        }
    }
}