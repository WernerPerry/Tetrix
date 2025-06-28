using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace Tetrix
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Tetrix : System.Windows.Forms.Form
	{
		private PictureBox pbWell;
		private Button btnStopGo;
		private IContainer components;
		private PictureBox pbShape;
		private Timer timer1;

        private Image pirateFemale;
        private Image pirateCartoon;
        private Image pirateMale;

		// 
		//	My class constants
		//
		private const int ROWS = 20;
		private const int COLS = 10;

		private const int TIMER_START = 850;
		private const int TIMER_DELTA = 50;
		private const int TIMER_MINIMUM = 100;

		private const int SHAPES_PER_LEVEL = 50;
		
		private const int INDEX_NULL = 0;
		private const int INDEX_BLACK = 8;
		private const int INDEX_DK_GRAY = 9;
		private const int INDEX_LT_GRAY = 10;
		private const int INDEX_WHITE = 11;

		// 
		//	My class members
		//
		private int m_intLevel;
		private int m_intScore;
		private int m_intRowCount;
		private int m_intShapeCount;
		private int m_intShapeHeight;
		private int m_intShapeWidth;
		private Engine m_objEngine;
		private int[,] m_intShape;
		private System.Windows.Forms.Button btnQuit;
	
		private enum TetrixStateEnum { StoppedState = 0, StartedState = 1, PausedState = 2, GameOverState = 3 };
		private Brush[] aBrush = {
						Brushes.Black,		Brushes.DarkBlue,	Brushes.Magenta,
						Brushes.Green,		Brushes.Gray,		Brushes.Red, 
						Brushes.Yellow,		Brushes.LightBlue,	Brushes.Black, 
						Brushes.DarkGray,	Brushes.LightGray,	Brushes.WhiteSmoke,
		};
		private Label lblScore;
		private Label lblRows;
		private Label lblLevel;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;

		private TetrixStateEnum m_enumTetrixState;
		private TetrixStateEnum TetrixState
		{
			set 
			{
				m_enumTetrixState = value;
				switch (m_enumTetrixState)
				{
					case TetrixStateEnum.StoppedState:
						this.btnStopGo.Text = "Start";
						break;

					case TetrixStateEnum.StartedState:
						this.btnStopGo.Text = "Pause";
						break;

					case TetrixStateEnum.PausedState:
						this.btnStopGo.Text = "Resume";
						break;
				}
			}
			get
			{
				return m_enumTetrixState;
			}
		}

		//
		// Constructor:
		//
		public Tetrix()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			try 
			{
                Assembly loadedAssembly = Assembly.LoadFrom("Tetrix.exe");

                Stream stream = loadedAssembly.GetManifestResourceStream("Tetrix.Pirate.jpg");
                if (stream != null)
                {
                    radioButton1.Visible = true;
                    radioButton1.Checked = true;
                    pirateFemale = Image.FromStream(stream);
                    pbWell.BackgroundImage = pirateFemale;
                }

                stream = loadedAssembly.GetManifestResourceStream("Tetrix.pirate3.jpg");
                if (stream != null)
                {
                    radioButton2.Visible = true;
                    radioButton2.Checked = true;
                    pirateMale = Image.FromStream(stream);
                    pbWell.BackgroundImage = pirateMale;
                }
 
                stream = loadedAssembly.GetManifestResourceStream("Tetrix.pirate2.jpg");
                if (stream != null)
                {
                    radioButton3.Visible = true;
                    radioButton3.Checked = true;
                    pirateCartoon = Image.FromStream(stream);
                    pbWell.BackgroundImage = pirateCartoon;
                }
            }
			catch (Exception ex)
			{
				MessageBox.Show("Open background file failed: " + ex.Message);
			}
		}

		//
		// Method: NewGame();
		//
		private void NewGame()
		{
			TetrixState = TetrixStateEnum.StoppedState;
			m_objEngine = new Engine(ROWS, COLS);
			m_intShape = m_objEngine.GetShape();
			m_intShapeHeight = m_objEngine.ShapeHeight;
			m_intShapeWidth = m_objEngine.ShapeWidth;
			timer1.Interval = TIMER_START;
			m_intScore = 0;
			m_intLevel = 1;
			m_intRowCount = 0;
			m_intShapeCount = 0;
			Refresh();
		}

		//
		//
		//
		private void GameOver()
		{
			this.TetrixState = TetrixStateEnum.GameOverState;
			StopGoToggle();
			MessageBox.Show("No more valid moves can be made!", "Game over!");
		}

		//
		//
		//
		private void UpdateDisplay()
		{
			lblScore.Text = "Score: " + m_intScore.ToString();
			lblRows.Text  = "Rows:  " + m_intRowCount.ToString();
			lblLevel.Text = "Level: " + m_intLevel.ToString();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.lblLevel = new System.Windows.Forms.Label();
            this.pbWell = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblScore = new System.Windows.Forms.Label();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pbShape = new System.Windows.Forms.PictureBox();
            this.btnStopGo = new System.Windows.Forms.Button();
            this.lblRows = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbWell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShape)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLevel
            // 
            this.lblLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLevel.Location = new System.Drawing.Point(296, 232);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(100, 23);
            this.lblLevel.TabIndex = 6;
            // 
            // pbWell
            // 
            this.pbWell.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbWell.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbWell.Location = new System.Drawing.Point(40, 40);
            this.pbWell.Name = "pbWell";
            this.pbWell.Size = new System.Drawing.Size(204, 404);
            this.pbWell.TabIndex = 0;
            this.pbWell.TabStop = false;
            this.pbWell.Paint += new System.Windows.Forms.PaintEventHandler(this.pbWell_Paint);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblScore
            // 
            this.lblScore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScore.Location = new System.Drawing.Point(296, 168);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(100, 23);
            this.lblScore.TabIndex = 4;
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(168, 480);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 0;
            this.btnQuit.TabStop = false;
            this.btnQuit.Text = "Quit";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // pbShape
            // 
            this.pbShape.BackColor = System.Drawing.Color.White;
            this.pbShape.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbShape.Location = new System.Drawing.Point(296, 40);
            this.pbShape.Name = "pbShape";
            this.pbShape.Size = new System.Drawing.Size(100, 100);
            this.pbShape.TabIndex = 3;
            this.pbShape.TabStop = false;
            this.pbShape.Paint += new System.Windows.Forms.PaintEventHandler(this.pbShape_Paint);
            // 
            // btnStopGo
            // 
            this.btnStopGo.Location = new System.Drawing.Point(40, 480);
            this.btnStopGo.Name = "btnStopGo";
            this.btnStopGo.Size = new System.Drawing.Size(75, 23);
            this.btnStopGo.TabIndex = 0;
            this.btnStopGo.TabStop = false;
            this.btnStopGo.Text = "Start";
            this.btnStopGo.Click += new System.EventHandler(this.btnStopGo_Click);
            // 
            // lblRows
            // 
            this.lblRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRows.Location = new System.Drawing.Point(296, 200);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(100, 23);
            this.lblRows.TabIndex = 5;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(296, 279);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(89, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Female Pirate";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(296, 303);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(78, 17);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Male Pirate";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(296, 327);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(92, 17);
            this.radioButton3.TabIndex = 9;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Cartoon Pirate";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Visible = false;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // Tetrix
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(430, 515);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblRows);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.pbShape);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnStopGo);
            this.Controls.Add(this.pbWell);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Tetrix";
            this.Text = "Tetrix";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetrix_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbWell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShape)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Tetrix());
		}

		//
		//
		//
		private void btnStopGo_Click(object sender, System.EventArgs e)
		{
			StopGoToggle();
		}

		//
		//
		//
		private void StopGoToggle()
		{
			if (this.TetrixState == TetrixStateEnum.StartedState)
			{
				TetrixState = TetrixStateEnum.PausedState;
				timer1.Enabled = false;
				btnStopGo.Visible = true;
				btnQuit.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                btnStopGo.Focus();
			}
			else if ((this.TetrixState == TetrixStateEnum.StoppedState)) 
			{
				NewGame();
				TetrixState = TetrixStateEnum.StartedState;
				timer1.Enabled = true;
				btnStopGo.Visible = false;
				btnQuit.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
				Focus();
			}
			else if ((this.TetrixState == TetrixStateEnum.PausedState))
			{
				TetrixState = TetrixStateEnum.StartedState;
				timer1.Enabled = true;
				btnStopGo.Visible = false;
				btnQuit.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
				Focus();
			}
			else if ((this.TetrixState == TetrixStateEnum.GameOverState))
			{
				TetrixState = TetrixStateEnum.StoppedState;
				timer1.Enabled = false;
				btnStopGo.Visible = true;
				btnStopGo.Text = "New Game";
				btnQuit.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
				this.Focus();
			}
			else 
			{	
				throw new Exception("Tetrix in unknown State (1)");
			}
		}

		//
		//
		//
		private void btnQuit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		//
		//	Method: timer tick handler
		//
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			// stop timer
			timer1.Stop();

			// shape changed?
			if (m_objEngine.TimerMove() == true) 
			{

				// shrink well
				while( m_objEngine.ShrinkWell(false, INDEX_BLACK) == true)
				{
					// paint black
					this.Refresh();
					System.Threading.Thread.Sleep(100);

					// paint dark gray
					m_objEngine.ShrinkWell(false, INDEX_DK_GRAY);
					this.Refresh();
					System.Threading.Thread.Sleep(100);

					// paint light gray
					m_objEngine.ShrinkWell(false, INDEX_LT_GRAY);
					this.Refresh();
					System.Threading.Thread.Sleep(100);

					// paint white
					m_objEngine.ShrinkWell(false, INDEX_WHITE);
					this.Refresh();
					System.Threading.Thread.Sleep(100);

					// remove line
					m_objEngine.ShrinkWell(true, INDEX_NULL);

					// keep count of rows
					++m_intRowCount;

					// give points
					m_intScore += 100 * m_intLevel;
				}

				// get new shape matrix
				m_intShape = m_objEngine.GetShape();
				m_intShapeHeight = m_objEngine.ShapeHeight;
				m_intShapeWidth = m_objEngine.ShapeWidth;

				// give score update
				m_intScore += 10 * m_intLevel;

				// count new shape
				if ( (++m_intShapeCount % SHAPES_PER_LEVEL) == 0)
				{
					++m_intLevel;
					if (timer1.Interval > TIMER_MINIMUM) 
					{
						timer1.Interval -= TIMER_DELTA;
					}
				}
			}

			// engine still active?
			if (m_objEngine.Active == false)
			{
				GameOver();
				return;
			}

			// restart timer
			timer1.Start();

			// repaint screen
			this.Refresh();
		}

		//
		// Method: Paint the shape preview window
		//
		private void pbShape_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

			// check if valid object
			if (m_intShape == null) 
			{
				return;
			}

			int row, col;
			int rowStart = (100 - (m_intShapeHeight * 20)) / 2;
			int colStart = (100 - (m_intShapeWidth * 20)) / 2;

			for (row=0; row<m_intShape.GetLength(0); row++)
			{
				for (col=0; col<m_intShape.GetLength(1); col++) 
				{
					if (m_intShape[row,col] != 0)
					{
						try 
						{
							int c = (col * 20) + colStart;
							int r = (row * 20) + rowStart;

							e.Graphics.FillRectangle(aBrush[m_intShape[row,col]], (c + 1), (r + 1), 18, 18);
							e.Graphics.DrawLine(System.Drawing.Pens.DarkGray, c, r, c, r+19);			// left
							e.Graphics.DrawLine(System.Drawing.Pens.DarkGray, c, r, c+19, r);		// top
							e.Graphics.DrawLine(System.Drawing.Pens.Black, c, r+19, c+19, r+19);	// bottom
							e.Graphics.DrawLine(System.Drawing.Pens.Black, c+19, r, c+19, r+19); // right
						}
						catch
						{
							MessageBox.Show("Tetrix::pbShape_Paint(), (" + row, "," + col + "=" + m_objEngine[row,col]);
						}
					}
				}
			}

			// paint scores, etc.
			UpdateDisplay();
		}

		//
		// Method: Paint the well
		//
		private void pbWell_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// check if valid object
			if (m_objEngine == null) 
			{
				return;
			}

			int row, col;

			for (row=0; row<m_objEngine.Rows; row++)
			{
				for (col=0; col<m_objEngine.Cols; col++) 
				{
					if (m_objEngine[row,col] != 0)
					{
						try
						{
							int c = col * 20;
							int r = row * 20;

							e.Graphics.FillRectangle(aBrush[m_objEngine[row,col]],
								(c + 1), (r + 1), 18, 18);
							e.Graphics.DrawLine(System.Drawing.Pens.DarkGray, c, r, c, r+19);			// left
							e.Graphics.DrawLine(System.Drawing.Pens.DarkGray, c, r, c+19, r);		// top
							e.Graphics.DrawLine(System.Drawing.Pens.Black, c, r+19, c+19, r+19);	// bottom
							e.Graphics.DrawLine(System.Drawing.Pens.Black, c+19, r, c+19, r+19); // right
						}
						catch
						{
							MessageBox.Show("Tetrix::pbWell_Paint(), (" + row, "," + col + "=" + m_objEngine[row,col]);
						}
					}
				}
			}

			// paint scores, etc.
			UpdateDisplay();
		}

		//
		// Method: Textri_KeyDown() - process key events
		//
		private void Tetrix_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//  Stop/Go/Pause toggle key
			if (e.KeyCode == Keys.Escape) 
			{
				StopGoToggle();
				e.Handled = true;
			}
			// Tetramino control key
			else if ((e.KeyCode == Keys.Right) ||
					 (e.KeyCode == Keys.Left) ||
					 (e.KeyCode == Keys.Up) ||
					 (e.KeyCode == Keys.Down) ||
					 (e.KeyCode == Keys.Space))
			{
				m_objEngine.ProcessKey(e.KeyCode);
				this.Refresh();	
				e.Handled = true;
			}
		}

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            pbWell.BackgroundImage = pirateCartoon;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            pbWell.BackgroundImage = pirateMale;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pbWell.BackgroundImage = pirateFemale;
        }
	}
}
