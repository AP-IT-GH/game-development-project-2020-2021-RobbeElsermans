﻿using Microsoft.Xna.Framework;
using Pigit.Collison;
using Pigit.Map;
using Pigit.Objects.Interfaces;
using Pigit.TileBuild;
using Pigit.TileBuild.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pigit.Movement.Abstracts
{
    class ACollectableMovement
    {
        protected ICollectableObject item;
        protected IPlayerObject heroPlayer;
        protected Level level;
        protected bool isGround = false;

        protected float jumpHeight;
        protected float walkingSpeed;
        protected Vector2 positie;
        protected Vector2 velocity;

        protected bool hasJumped;
        public static IMoveable HeroPlayer { get; set; }

        public ACollectableMovement(ICollectableObject item, Level level, float jumpHeight = 2f, float walkSpeed = 2f)
        {
            this.item = item;
            this.level = level;
            hasJumped = true;
            isGround = false;
            this.jumpHeight = jumpHeight;
            this.walkingSpeed = walkSpeed;
        }

        public virtual void CheckMovement(GameTime gameTime)
        {
            RecastPositions();

            CheckCollide(18, 18);
            CheckGravity();

            item.Update(gameTime);
        }
        protected void RecastPositions()
        {
            positie = new Vector2(item.Positie.X, item.Positie.Y);
            velocity = new Vector2(0f, item.Velocity.Y);
        }
        protected virtual void CheckGravity()
        {
            //Hit another object
            if (isGround)
            {
                velocity.Y = 0.2f;
                hasJumped = false;
            }
            else
            {
                float i = 1f;
                velocity.Y += 0.20f * i;
            }

            item.Positie = positie;
            item.Velocity = velocity;
        }
        protected virtual void CheckCollide(int offsetHeight1, int offsetHeight2)
        {
            isGround = false;

            foreach (var tile in level.CurrTiles)
            {
                if (tile is ICollideTile)
                {
                    var temp = tile as ICollideTile;
                    Rectangle rectangle = item.Rectangle;

                    if (EndBlockCollision.isTouchingRight(velocity, temp, rectangle) || EndBlockCollision.isTouchingLeft(velocity, temp, rectangle))
                    {
                        velocity.X = 0f;
                    }
                    if (EndBlockCollision.isTouchingTop(velocity, temp, rectangle) && !isGround)
                    {
                        positie.Y = temp.Border.Y - (temp.Border.Height - offsetHeight1);
                        velocity.Y = 0.2f;
                        isGround = true;
                    }
                    if (EndBlockCollision.isTouchingBottom(velocity, temp, rectangle))
                    {
                        velocity.Y = 0.2f;
                    }
                }

                if (tile is IPlatformTile)
                {
                    var temp = tile as IPlatformTile;
                    Rectangle rectangle = item.Rectangle;

                    if (PlatformBlockCollision.isOnTopOf(rectangle, temp.Border, velocity) && velocity.Y > 0)
                    {
                        positie.Y = temp.Border.Y - (temp.Border.Height - offsetHeight2);
                        velocity.Y = 0f;
                        isGround = true;
                    }
                }
            }
        }
    }
}
