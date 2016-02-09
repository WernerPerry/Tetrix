using System;

namespace Tetrix
{
	/// <summary>
	/// Summary description for Tetramino.
	/// </summary>
	public class Shapes
	{
		// private members
		private const int MAX_SHAPE = 7;
		private const int MAX_ORIENTATION = 4;
		private const int HEIGHT_INDEX = 4;
		private const int WIDTH_INDEX = 5;
		private const int COLOR_INDEX = 6;
		private int m_currentShape;
		private int m_currentOrientation;
		private Random rand;
		private int [,] m_masks = { 
									{0x44C0, 0x8E00, 0xC880, 0xE200, 3, 2, 1}, // "J"
								    {0x88C0, 0xE800, 0xC440, 0x2E00, 3, 2, 2}, // "L"
									{0x6C00, 0x8C40, 0x6C00, 0x8C40, 2, 3, 3}, // "S"
									{0xCC00, 0xCC00, 0xCC00, 0xCC00, 2, 2, 4}, // "O"
									{0xC600, 0x4C80, 0xC600, 0x4C80, 2, 3, 5}, // "Z"
									{0xE400, 0x2620, 0x4E00, 0x8C80, 2, 3, 6}, // "T"
									{0x4444, 0x0F00, 0x4444, 0x0F00, 4, 3, 7}, // "I"
		};

		//
		// Properties:
		//
		public int GetHeight
		{
			get { return m_masks[m_currentShape, HEIGHT_INDEX]; }
		}

		public int GetWidth
		{
			get { return m_masks[m_currentShape, WIDTH_INDEX]; }
		}

		//
		// Method: Constructor
		//
		public Shapes()
		{
			//
			// TODO: Add constructor logic here
			//

			// Seed the random number generator
			rand = new Random();

			// generate random first shape
			NextShape();
		}

		//
		//	Method: Randomize next tetramino and reset orientation
		//
		public void NextShape()
		{
			// generate random shape
			m_currentShape = rand.Next(MAX_SHAPE);

			// reset orientation
			m_currentOrientation = 0;
		}

		//
		//  GetShape: Returns the current tetramino and orientation information.
		//
		public int [,] GetShape()
		{
			int color = m_masks[m_currentShape, COLOR_INDEX];
			int [,] aShape = new int[4,4];
			int mask = m_masks[m_currentShape, m_currentOrientation];
			for (int i=0; i<4; i++) 
			{
				for (int j=0; j<4; j++) 
				{
					aShape[i,j] = (mask & 0x8000)!= 0 ? (byte) color : (byte) 0;
					mask <<= 1;
				}
			}
			return aShape;
		}

		//
		// Method: Rotate tetramino orientation left or right
		//
		public void Rotate (Engine.ShapeRotateEnum rotate)
		{
			int tmpInt = m_currentOrientation;

			switch (rotate)
			{
				case Engine.ShapeRotateEnum.RotateCounterClockwise:
					tmpInt = (tmpInt + 3) % MAX_ORIENTATION;
					break;

				case Engine.ShapeRotateEnum.RotateClockwise:
					tmpInt = (tmpInt + 1) % MAX_ORIENTATION;
					break;

				case Engine.ShapeRotateEnum.RotateNone:
				default:
					throw new Exception("Invalid rotation orientation!");
			}

			m_currentOrientation = tmpInt;
		}
	}
}
