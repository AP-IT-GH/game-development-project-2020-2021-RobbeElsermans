﻿using Microsoft.Xna.Framework;
using Pigit.Collison;
using Pigit.Map;
using Pigit.Objects;
using Pigit.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pigit.Movement.NPCMoveCommands
{
    class MoveCommandStaticNPC : AMoveCommandFollowWhenNearby
    {
        public MoveCommandStaticNPC(IPlayerObject player, Level level) : base(player, level, 4, 2)
        {

        }
        public override void CheckMovement(GameTime gameTime)
        {
            RecastPositions();

            if (player is IMovementEnemy)
            {
                var temp = player as IMovementEnemy;
                if (NPCCollision.IsAroundNPC(HeroPlayer.Positie, positie))
                {
                    base.CheckMovement(gameTime);
                }
                else
                {

                    CheckCollide(4,5);
                    CheckGravity();

                    player.Update(gameTime);
                }
            }
        }
    }
}
