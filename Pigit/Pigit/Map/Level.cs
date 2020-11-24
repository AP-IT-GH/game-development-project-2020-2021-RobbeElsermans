﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pigit.TileBuild;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pigit.Map
{
    class Level
    {
        private IWorldLayout mapLayout;
        private TileOpbouw blockOpbouw;

        public List<ITile> Tiles;

        public Level(ContentManager content, IWorldLayout layout)
        {
            this.mapLayout = layout;
            Tiles = new List<ITile>();

            InitializeTiles(content);
        }

        private void InitializeTiles(ContentManager content)
        {
            this.blockOpbouw = new TileOpbouw(content);
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
                }
            }

        }
        public void DrawWorld(SpriteBatch spriteBatch)
        {
            foreach (var texture in Tiles)
            {
                texture.Draw(spriteBatch);
            }
        }
    }
}
