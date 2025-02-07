using System.Collections.Generic;
using Godot;

namespace SnakeGame
{
	public partial class Grid : Node2D
	{
		[Export] private string _cellScenePath = "res://Levels/Cell.tscn";
		[Export] private int _width = 0;
		[Export] private int _height = 0;

		// Vector2I on integeriä kullekin koordinaatille yksikkönä käyttävä vektorityyppi.
		[Export] private Vector2I _cellSize = Vector2I.Zero;

		// TODO: Kirjoita julkinen property, joka mahdollistaa _width jäsenmuuttujan lukemisen,
		// muttei asettamista. Anna propertyn nimeksi Width.
		public int Width => _width;
		// TODO: Kirjoita vastaava property _height jäsenmuuttujalle. Propertyn nimi tulee olla Height.
		public int Height => _height;
		// Tähän 2-uloitteiseen taulukkoon on tallennettu gridin solut. Alussa taulukkoa ei ole, vaan
		// muuttujassa on tyhjä viittaus (null). Taulukko pitää luoda pelin alussa (esim. _Ready-metodissa).
		private Cell[,] _cells = null;

		public override void _Ready()
		{
			// TODO: Alusta _cells taulukko
			_cells = new Cell[_width, _height];
			// Lataa Cell-scene. Luomme tästä uuden olion kutakin ruutua kohden.
			PackedScene cellScene = ResourceLoader.Load<PackedScene>(_cellScenePath);
			if (cellScene == null)
			{
				GD.PrintErr("Cell sceneä ei löydy! Gridiä ei voi luoda!");
				return;
			}

			Vector2 viewportSize = GetViewportRect().Size;
			Vector2 gridSize = new Vector2(_width * _cellSize.X, _height * _cellSize.Y);
			Vector2 gridStart = (viewportSize - gridSize) / 2;

			// Alustetaan Grid kahdella sisäkkäisellä for-silmukalla.
			for (int x = 0; x < _width; ++x)
			{
				for (int y = 0; y < _height; ++y)
				{
					// Luo uusi olio Cell-scenestä.
					Cell cell = cellScene.Instantiate<Cell>();
					// Lisää juuri luotu Cell-olio gridin Nodepuuhun.
					AddChild(cell);

					// TODO: Laske ja aseta ruudun sijainti niin maailman koordinaatistossa kuin
					// ruudukonkin koordinaatistossa. Aseta ruudun sijainti käyttäen cell.Position propertyä.
					Vector2 worldPosition = gridStart + new Vector2(
						x * _cellSize.X + _cellSize.X / 2,
						y * _cellSize.Y + _cellSize.Y / 2
					);

					cell.GridPosition = new Vector2I(x, y);
					cell.Position = worldPosition;
					// TODO: Tallenna ruutu tietorakenteeseen oikealle paikalle.
					_cells[x, y] = cell;
				}
			}
		}
	}
}