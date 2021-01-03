﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pigit.Animatie;
using Pigit.Collison;
using Pigit.Input.Interfaces;
using Pigit.Map;
using Pigit.Music;
using Pigit.Music.Interface;
using Pigit.Objects;
using Pigit.Objects.Enums;
using Pigit.Objects.Interfaces;
using Pigit.SpriteBuild.Enums;
using Pigit.TileBuild;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pigit.Movement
{
    class MoveCommandHero : AMovement
    {
        IInputReader inputs;

        public MoveCommandHero(IPlayerObject player, Level level, IInputReader input, IEffectMusic effects) : base(player, level, effects)
        {
            inputs = input;
        }

        public override void CheckMovement(GameTime gameTime)
        {
            RecastPositions();

            //bool attack = false;
            //player.IsAttacking = false;
            inputs.ReadInput();
            player.Direction = inputs.Direction;

            if (inputs.Move)
            {
                isGround = false;

                if (inputs.Direction)
                {
                    velocity.X -= walkingSpeed;
                }
                else
                {
                    velocity.X += walkingSpeed;
                }
            }
            if (inputs.Attack)
            {
                player.IsAttacking = true;
            }
            else
            {
                player.IsAttacking = false;
            }

            //BRON jump werkend krijgen: https://www.youtube.com/watch?v=ZLxIShw-7ac&list=PL667AC2BF84D85779&index=25&t=5s 
            if (inputs.Jump && !hasJumped)
            {
                effects.PlayJump();
                velocity.Y = -jumpHeight;
                hasJumped = true;
                isGround = false;
            }

            this.CheckCollide(-13, -13);

            CheckGravity();

            if (player.IsAttacking && !player.HasAttacked && isGround == true)
            {
                effects.PlayAttack();
                player.Attack.Attack(level.CurrEnemys, player, gameTime, effects);
            }
            else
            {
                effects.StopAttack();
            }

            player.Positie = positie;
            player.Velocity = velocity;

            player.Update(gameTime);
        }
        protected override void CheckCollide(int offsetHeight1, int offsetHeight2)
        {
            base.CheckCollide(offsetHeight1, offsetHeight2);

            foreach (var item in level.CurrCollectable)
            {
                if (ItemCollision.IsTouchingItem(player.Rectangle, item.Rectangle) && !item.IsCollected)
                {
                    item.IsCollected = true;
                    ICollectMusic temp = effects as ICollectMusic;
                    switch (item.ItemType)
                    {
                        case CollectableTypes.BigHeart:
                        case CollectableTypes.SmallHeart:
                            temp.PlayHeartCollect();
                            player.Hearts += item.Value;
                            break;
                        case CollectableTypes.BigDiamond:
                        case CollectableTypes.SmallDiamond:
                            temp.PlayDiamondCollect();
                            player.Points += item.Value;
                            //Score verhogen.
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
