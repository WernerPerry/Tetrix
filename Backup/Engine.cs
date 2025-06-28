using System;
using System.Data;
using System.Windows.Forms;
//using System.Windows.Forms.

namespace Tetrix
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine
	{
		private int m_intRows;
		private int m_intCols;
		private int[,] m_intWell;
		private int[,] m_intShape;
		private int[,] m_intShapeOriginal;
		private int m_intCurrentRow;
		private int m_intCurrentCol;
		private Shapes m_Shapes;
		private bool m_blnActive;

		//  Note:Shape height and width are actual tetramino sizes not buffer sizes
		private int m_intShapeHeight;
		private int m_intShapeWidth;

		// Enum: public enum for rotation direction
		public enum ShapeRotateEnum {RotateNone =0, RotateClockwise = 1, RotateCounterClockwise = 2};

		// Constuctor: Engine(r,c) 
		public Engine(int rows, int cols)
		{
			//
			// TODO: Add constructor logic here
			//
			m_intRows = rows;
			m_intCols = cols;
			m_blnActive = true;
			m_intWell = new int[m_intRows, m_intCols];
			m_Shapes = new Shapes();
			GetNextShape();
		}

		//
		// Method: Indexer for the WELL
		//
		public int this[int row, int col]
		{
			get 
			{
				if (row >= m_intRows) 
				{
					throw new Exception("Maximum rows " + m_intRows + " exceeded!", null);
				}
				else if (col >= m_intCols)
				{
					throw new Exception("Maximum cols " + m_intCols + " exceeded!", null);	
				}

				return m_intWell[row, col];
			}

			set 
			{
				if (row >= m_intRows) 
				{
					throw new Exception("Maximum rows " + m_intRows + " exceeded!", null);
				}
				else if (col >= m_intCols)
				{
					throw new Exception("Maximum cols " + m_intCols + " exceeded!", null);	
				}

				m_intWell[row, col] = value;
			}
		}

		//
		// Method: TimerMove(), moves tetramino down one and 
		//						returns true if new shape is selected.
		//
		public bool TimerMove()
		{
			try 
			{
				// check if currently requested move is valid
				if (IsMoveValid(1, 0) == true) 
				{
					// institute move
					++m_intCurrentRow;

					// show shape
					XorShape();

					// no new shape 
					return false;
				}
				else  
				{

					// can't move shape into playing well, game over!
					if (m_intCurrentRow < 0) 
					{
						m_blnActive = false;
						return false;
					}

					// can't move any more so generate new shape and save it
					GetNextShape();

					// new shape
					return true;
				}
			}
			catch
			{
				throw new Exception("Engine::TimerMove() - bad stuff happening!");
			}
		}

		//
		// Method: ProcessKeys(), handle Tetramino control keys (left, right, spin, etc).
		//
		public void ProcessKey(Keys kc)
		{
			try 
			{
				if (kc == Keys.Left) 
				{
					// check if currently requested move is valid
					if (IsMoveValid(0, -1) == true) 
					{
						// institute move
						--m_intCurrentCol;

						// show shape
						XorShape();
					}			
				}
				else if (kc == Keys.Right)
				{
					// check if currently requested move is valid
					if (IsMoveValid(0, 1) == true) 
					{
						// institute move
						++m_intCurrentCol;

						// show shape
						XorShape();
					}			
				}
				else if (kc == Keys.Up) 
				{
					// check if currently requested move is valid
					if (IsMoveValid(0, 0, ShapeRotateEnum.RotateCounterClockwise) == true) 
					{
						// show shape
						XorShape();
					}			
				}
				else if (kc == Keys.Down)
				{
					// check if currently requested move is valid
					if (IsMoveValid(0, 0, ShapeRotateEnum.RotateClockwise) == true) 
					{
						// show shape
						XorShape();
					}			
				}
				else if (kc == Keys.Space)
				{
					while (IsMoveValid(1, 0) == true)
					{
						// institute move
						++m_intCurrentRow;

						// show shape
						XorShape();
					}
				}
			}
			catch 
			{
				throw new Exception("Engine::ProcessKey() - bad stuff happening!");
			}
		}

		//
		// Method: IsMoveValid() - checks if requested move would be valid
		//
		private bool IsMoveValid(int x, int y)
		{
			return IsMoveValid(x, y, ShapeRotateEnum.RotateNone);
		}

		private bool IsMoveValid(int x, int y, ShapeRotateEnum r)
		{
			int NextRow = m_intCurrentRow + x;
			int NextCol = m_intCurrentCol + y;

			// remove current object
			XorShape();

			// rotate shape if requested
			if (r != ShapeRotateEnum.RotateNone)
			{
				GetRotateShape(r);
			}
			
			// check for collision
			if (Collision(NextRow, NextCol))
			{
				// if so restore shape and return failed
				if (r == ShapeRotateEnum.RotateClockwise)
				{
					// reverse clockwise
					GetRotateShape(ShapeRotateEnum.RotateCounterClockwise);
				}
				else if (r == ShapeRotateEnum.RotateCounterClockwise)
				{
					// reverse counterclockwise
					GetRotateShape(ShapeRotateEnum.RotateClockwise);
				}

				// put shape back in well 
				XorShape();
				return false;
			}

			// ok to move
			return true;
		}

		//
		// Method: XorShape() - mask current shape in or out of well
		//
		private void XorShape()
		{
			int wellX  = m_intWell.GetLength(0);
			int wellY  = m_intWell.GetLength(1);
			int shapeX = m_intShape.GetLength(0);
			int shapeY = m_intShape.GetLength(1);

			try 
			{
				for (int i=0, x=m_intCurrentRow; i<shapeX; i++, x++) 
				{
					for (int j=0, y=m_intCurrentCol; j<shapeY; j++, y++) 
					{
						if ((x >=0) && (y>=0) && (x < wellX) && (y < wellY)) 
						{
							m_intWell[x,y] ^= m_intShape[i,j];
						}
					}
				}
			}
			catch 
			{
				throw new Exception("Engine::XorShape() - bad stuff happening!");
			}
		}

		//
		// Method: Collision() - check if proposed move is valid
		//
		private bool Collision(int xTarget, int yTarget)
		{
			int wellX  = m_intWell.GetLength(0);
			int wellY  = m_intWell.GetLength(1);
			int shapeX = m_intShape.GetLength(0);
			int shapeY = m_intShape.GetLength(1);

			try 
			{
				for (int i=0, x=xTarget; i<shapeX; i++, x++) 
				{
					for (int j=0, y=yTarget; j<shapeY; j++, y++) 
					{
						// trying to place visible element of shape
						if (m_intShape[i,j] !=0) 
						{
							// is out of bounds or is cell taken already?
							if ((y < 0) || 
								(y >=wellY) || 
								(x >= wellX) ||
							    ((x >= 0) && (m_intWell[x,y] != 0)))
							{
								return true;
							}
						}
					}
				}
			}
			catch 
			{
				throw new Exception("Engine::Collision() - bad stuff happening!");
			}
			return false;
		}

		//
		// Method: GetShape() returns current shape array
		//
		public int [,] GetShape()
		{
			return this.m_intShapeOriginal;
		}

		//
		// Method: GetNextShape(), 
		//
		public void GetNextShape()
		{
			// get next shape
			m_Shapes.NextShape();
			m_intShape = m_Shapes.GetShape();

			// save original orientation
			m_intShapeOriginal = (int[,]) m_intShape.Clone();

			// initialize well location
			m_intShapeHeight = m_Shapes.GetHeight;
			m_intShapeWidth  = m_Shapes.GetWidth;
			m_intCurrentRow    = (0 - m_Shapes.GetHeight);
			m_intCurrentCol    = (10 - m_Shapes.GetWidth) / 2;

		}

		//
		//	Method: GetRotateShape(),
		//
		private void GetRotateShape(ShapeRotateEnum r)
		{
			try 
			{
				if (r == ShapeRotateEnum.RotateClockwise)
				{
					m_Shapes.Rotate(r);
					m_intShape = m_Shapes.GetShape();
				}
				else if (r == ShapeRotateEnum.RotateCounterClockwise)
				{
					m_Shapes.Rotate(r);
					m_intShape = m_Shapes.GetShape();
				}
			}
			catch 
			{
				MessageBox.Show("GetRotateShape(), bad stuff happening!");
			}
		}

		//
		//	Method: 
		//
		public bool ShrinkWell(bool blnDestructive, int intBrushColorIndex)
		{
			bool blnShrink = false;

			// each row
			for (int row=0; row<m_intRows; row++) 
			{
				// assume row full
				blnShrink = true;

				// search for non-full row
				for (int col=0; col<m_intCols; col++) 
				{
					if (m_intWell[row,col] == 0) 
					{
						blnShrink = false;
						break;
					}
				}

				// remove full row
				if (blnShrink == true) 
				{
					if (blnDestructive) 
					{
						// zap first row
						for (int c=0; c<m_intCols; c++) 
						{
							m_intWell[0,c] = 0;
						}

						// copy upper rows down one
						for (int r=row; r>0; r--) 
						{
							for (int c=0; c<m_intCols; c++) 
							{
								m_intWell[r,c] = m_intWell[r-1,c];
							}
						}
					}
					else 
					{
						for (int c=0; c<m_intCols; c++) 
						{
							m_intWell[row,c] = intBrushColorIndex;
						}
					}

					// one row at a time
					break;
				}
			}
			return blnShrink;
		}

		//
		//	Properties: various size and shape properties.
		//
		public int Rows
		{
			get { return this.m_intRows; }
		}

		public int Cols
		{
			get { return this.m_intCols; }
		}

		public int ShapeWidth
		{
			get { return this.m_intShapeWidth; }
		}

		public int ShapeHeight
		{
			get { return this.m_intShapeHeight; }
		}

		public bool Active
		{
			get { return this.m_blnActive; }
		}
	}
}
