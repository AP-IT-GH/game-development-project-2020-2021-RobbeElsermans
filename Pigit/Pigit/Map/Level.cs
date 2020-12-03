﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pigit.Movement;
using Pigit.SpriteBuild;
using Pigit.TileBuild;
using System;
using System.Collections.Generic;
using System.Text;
using Pigit.Objects;


namespace Pigit.Map
{
    class Level
    {
        private IWorldLayout mapLayout;
        private TileOpbouw blockOpbouw;
        public List<INPCObject> Enemys { get; set; }
        public List<AMovement> moveEnemys;
        private SpriteOpbouw opbouwSprites;

        public List<ITile> Tiles;

        public Level(ContentManager content, IWorldLayout layout)
        {
            this.mapLayout = layout;
            Tiles = new List<ITile>();

            InitializeTiles(content);
            InitializeNPCs(content);
            InitializeMovement();
        }

        private void InitializeMovement()
        {
            moveEnemys = new List<AMovement>();

            foreach (var enemy in Enemys)
            {
                moveEnemys.Add(new MoveCommandNPC(enemy, this));
            }
        }

        private void InitializeNPCs(ContentManager content)
        {
            opbouwSprites = new SpriteOpbouw(content);

            Enemys = new List<INPCObject>();
            
            Enemys.Add(new Pig(opbouwSprites.GetSpritePig(12)));
            Enemys[0].Positie = new Vector2(7 * 32, 4 * 32);
            Enemys.Add(new Pig(opbouwSprites.GetSpritePig(12)));
            Enemys[1].Positie = new Vector2(9 * 32, 4 * 32);
            Enemys.Add(new Pig(opbouwSprites.GetSpritePig(12)));
            Enemys[2].Positie = new Vector2(11 * 32, 4 * 32);
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

            for (int x = 0; x < mapLayout.Width; x++)
            {
                for (int y = 0; y < mapLayout.Height; y++)
                {
                    for (int i = 1; i <= blockOpbouw.BackgroundTiles.Count; i++)
                    {
                        if (i == mapLayout.BackgroundTiles[x, y])
                        {
                            Tiles.Add(new TileDefine(blockOpbouw.BackgroundTiles[i-1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.CollideTiles.Count; i++)
                    {
                        if (i == mapLayout.CollideTileLayout[x, y])
                        {
                            Tiles.Add(new CollideTileDefine(blockOpbouw.CollideTiles[i-1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.ForegroundTiles.Count; i++)
                    {
                        if (i == mapLayout.ForegroundTiles[x, y])
                        {
                            Tiles.Add(new TileDefine(blockOpbouw.ForegroundTiles[i-1], new Vector2(y * 32, x * 32)));
                        }
                    }

                    for (int i = 1; i <= blockOpbouw.PLatformTiles.Count; i++)
                    {
                        if (i == mapLayout.PlatformTile[x, y])
                        {
                            Tiles.Add(new PlatformTileDefine(blockOpbouw.PLatformTiles[i - 1], new Vector2(y * 32, x * 32)));
                        }
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

