﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pigit.SpriteBuild;
using Pigit.TileBuild;
using System;
using System.Collections.Generic;
using System.Text;
using Pigit.Objects;
using Pigit.SpriteBuild.Enums;
using Pigit.Movement.NPCMoveCommands;
using Pigit.Movement;

namespace Pigit.Map
{
    class Level
    {
        private IWorldLayout mapLayout;
        private TileOpbouw blockOpbouw;
        public List<INPCObject> Enemys { get; set; }
        public List<AMovement> moveEnemys;
        private SpriteOpbouw opbouwSprites;
        private ContentManager content;
        private IMoveable heroPlayer;

        public List<ITile> Tiles;

        public Level(ContentManager content, IWorldLayout layout, IMoveable hero)
        {
            heroPlayer = hero;
            this.mapLayout = layout;
            Tiles = new List<ITile>();
            this.content = content;
            opbouwSprites = new SpriteOpbouw(content);
            Enemys = new List<INPCObject>();
        }

        private void GenerateMovement()
        {
            moveEnemys = new List<AMovement>();

            foreach (var enemy in Enemys)
            {
                var temp = enemy as IMovementEnemy;
                switch (temp.MovementType)
                {
                    case MoveTypes.Static:
                        moveEnemys.Add(new MoveCommandStaticNPC((IPlayerObject)enemy, this));
                        break;
                    case MoveTypes.Walk:
                        moveEnemys.Add(new MoveCommandWalkNPC((IPlayerObject)enemy, this));
                        break;
                    case MoveTypes.GuardTime:
                        moveEnemys.Add(new MoveCommandGuardNPC((IPlayerObject)enemy, this, 5.0,3.0));
                        break;
                    case MoveTypes.GuardPosition:
                        moveEnemys.Add(new MoveCommandGuardNPC((IPlayerObject)enemy, this, (int)enemy.Positie.X - 32, (int)enemy.Positie.X + 32, 4.0));
                        break;
                    case MoveTypes.Follow:
                        moveEnemys.Add(new MoveCommandFollowNPC((IPlayerObject)enemy, this,heroPlayer));
                        break;
                    default:
                        break;
                }
                //moveEnemys.Add(new MoveCommandWalkNPC((IPlayerObject)enemy, this));
            }
        }

        private void InitializeTiles(ContentManager content)
        {
            this.blockOpbouw = new TileOpbouw(content);
        }
        public void Update(GameTime gameTime)
        {
            foreach (var moveCommand in moveEnemys)
            {
                moveCommand.CheckMovement(gameTime);
            }
        }

        public void CreateWorld()
        {
            InitializeTiles(content);
            //GenerateNPC(content);

            GenerateMapContent(content);
            GenerateMovement();
        }

        private void GenerateMapContent(ContentManager content)
        {
            for (int x = 0; x < mapLayout.Width; x++)
            {
                for (int y = 0; y < mapLayout.Height; y++)
                {
                    for (int i = 1; i <= blockOpbouw.BackgroundTiles.Count; i++)
                    {
                        if (i == mapLayout.BackgroundTiles[x, y])
                        {
                            Tiles.Add(new TileDefine(blockOpbouw.BackgroundTiles[i - 1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.CollideTiles.Count; i++)
                    {
                        if (i == mapLayout.CollideTileLayout[x, y])
                        {
                            Tiles.Add(new CollideTileDefine(blockOpbouw.CollideTiles[i - 1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.ForegroundTiles.Count; i++)
                    {
                        if (i == mapLayout.ForegroundTiles[x, y])
                        {
                            Tiles.Add(new TileDefine(blockOpbouw.ForegroundTiles[i - 1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.PLatformTiles.Count; i++)
                    {
                        if (i == mapLayout.PlatformTiles[x, y])
                        {
                            Tiles.Add(new PlatformTileDefine(blockOpbouw.PLatformTiles[i - 1], new Vector2(y * 32, x * 32)));
                        }
                    }
                    switch ((PigTypes)(mapLayout.Enemys[x, y]/10))
                    {
                        case PigTypes.Standard:
                            Enemys.Add(new Pig(opbouwSprites.GetSpritePig(12), new Vector2(y * 32, x * 32), (MoveTypes)(mapLayout.Enemys[x, y] % 10)));
                            break;
                        case PigTypes.Match:
                            break;
                        case PigTypes.HideBox:
                            break;
                        case PigTypes.TrowBox:
                            break;
                        case PigTypes.TrowBomb:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void DrawWorld(SpriteBatch spriteBatch)
        {
            foreach (var texture in Tiles)
            {
                texture.Draw(spriteBatch);
            }

            foreach (var enemy in Enemys)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }
}

