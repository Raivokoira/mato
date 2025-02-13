using Godot;
using SnakeGame.UI;
using System;

namespace SnakeGame
{
	public partial class Level : Node2D
	{
		// Static on luokan ominaisuus, ei siitä luotujen ominaisuuksien.
		// Kaikki luokasta luodut olio jakavat saman staattisen muuttujan.
		private static Level _current = null;
		public static Level Current
		{
			get { return _current; }
		}

		[Export] private string _snakeScenePath = "res://Character/Snake.tscn";
		[Export] private string _appleScenePath = "res://Levels/Collectables/Apple.tscn";
		[Export] private string _nuclearWastePath = "res://Levels/Collectables/NuclearWaste.tscn";
		[Export] private TopUIControl _topUIControl = null;

		private PackedScene _snakeScene = null;
		private PackedScene _appleScene = null;
		private PackedScene _nuclearWasteScene = null;
		private int _score = 0;
		private Grid _grid = null;
		private Snake _snake = null;
		// Omenoita voi olla olemassa yksi kerrallaan.
		private Apple _apple = null;
		private NuclearWaste _nuclearWaste = null;

		public int Score
		{
			get { return _score; }
			set
			{
				if (value < 0)
				{
					_score = 0;
				}
				else
				{
					_score = value;
				}

				if (_topUIControl != null)
				{
					_topUIControl.SetScore(_score);
				}
			}
		}

		public Grid Grid
		{
			get { return _grid; }
		}

		public Snake Snake
		{
			get { return _snake; }
		}

		// Rakentaja. Käytetään alustamaan olio.
		public Level()
		{
			// _current muuttujaan asetetaan viittaus juuri juotuun Level-olioon.
			// Tälläön Current-propertyn kautta muut oliot pääsevät käsiksi Level-olioon.
			_current = this;
		}

		public override void _Ready()
		{
			_grid = GetNode<Grid>("Grid");
			if (_grid == null)
			{
				GD.PrintErr("Gridiä ei löytynyt Levelin lapsinodeista!");
			}

			ResetGame();
		}

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed("RestartGame"))
			{
				RestartGame();
			}
		}

		public void RestartGame()
		{
			// Tuhoa edellinen mato
			if (_snake != null)
			{
				_snake.QueueFree();
				_snake = null;
			}

			// Tuhoa kerättävät esineet
			if (_apple != null)
			{
				_apple.QueueFree();
				_apple = null;
			}

			if (_nuclearWaste != null)
			{
				_nuclearWaste.QueueFree();
				_nuclearWaste = null;
			}

			// Luo uusi mato
			_snake = CreateSnake();
			AddChild(_snake);

			// Nollaa pisteet
			Score = 0;

			// Luo uudet kerättävät esineet
			ReplaceApple();
			ReplaceNuclearWaste();
		}

		/// <summary>
		/// Aloittaa uuden pelin.
		/// </summary>
		public void ResetGame()
		{
			// Tuhoa edellinen mato, jos se on olemassa.
			if (_snake != null)
			{
				_snake.QueueFree();
				_snake = null;
			}

			// Luo mato
			_snake = CreateSnake();
			AddChild(_snake);

			// Nollaa pisteet
			Score = 0;

			// Luo omena
			ReplaceApple();

			// Luo nuclear waste
			ReplaceNuclearWaste();
		}

		private Snake CreateSnake()
		{
			if (_snakeScene == null)
			{
				_snakeScene = ResourceLoader.Load<PackedScene>(_snakeScenePath);
				if (_snakeScene == null)
				{
					GD.PrintErr("Madon sceneä ei löydy!");
					return null;
				}
			}

			return _snakeScene.Instantiate<Snake>();
		}

		public void ReplaceApple()
		{
			if (_apple != null)
			{
				Grid.ReleaseCell(_apple.GridPosition);

				_apple.QueueFree();
				_apple = null;
			}

			if (_appleScene == null)
			{
				_appleScene = ResourceLoader.Load<PackedScene>(_appleScenePath);
				if (_appleScene == null)
				{
					GD.PrintErr("Can't load apple scene!");
					return;
				}
			}

			_apple = _appleScene.Instantiate<Apple>();
			AddChild(_apple);

			Cell freeCell = Grid.GetRandomFreeCell();
			if (Grid.OccupyCell(_apple, freeCell.GridPosition))
			{
				_apple.SetPosition(freeCell.GridPosition);
			}
		}

		public void ReplaceNuclearWaste()
		{
			if (_nuclearWaste != null)
			{
				Grid.ReleaseCell(_nuclearWaste.GridPosition);

				_nuclearWaste.QueueFree();
				_nuclearWaste = null;
			}

			if (_nuclearWasteScene == null)
			{
				_nuclearWasteScene = ResourceLoader.Load<PackedScene>(_nuclearWastePath);
				if (_nuclearWasteScene == null)
				{
					GD.PrintErr("Can't load nuclear waste scene!");
					return;
				}
			}

			_nuclearWaste = _nuclearWasteScene.Instantiate<NuclearWaste>();
			AddChild(_nuclearWaste);

			Cell freeCell = Grid.GetRandomFreeCell();
			if (Grid.OccupyCell(_nuclearWaste, freeCell.GridPosition))
			{
				_nuclearWaste.SetPosition(freeCell.GridPosition);
			}
		}
	}
}