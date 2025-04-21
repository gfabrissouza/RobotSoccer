namespace AmbienteDeSimulacao
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.canvas1 = new SpriteCanvas.Canvas();
            this.canvas2 = new SpriteCanvas.Canvas();
            this.canvas3 = new SpriteCanvas.Canvas();
            this.canvas4 = new SpriteCanvas.Canvas();
            this.canvas5 = new SpriteCanvas.Canvas();
            this.timerAmbiente = new System.Windows.Forms.Timer(this.components);
            this.btnIniciaSimulacao = new System.Windows.Forms.Button();
            this.btnFinalizaSimulacao = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxPontosOrientacao = new System.Windows.Forms.CheckBox();
            this.campoBackground = new System.Windows.Forms.PictureBox();
            this.checkBoxMovTeclado = new System.Windows.Forms.CheckBox();
            this.gBPlacar = new System.Windows.Forms.GroupBox();
            this.lbPlacarTimeVermelho = new System.Windows.Forms.Label();
            this.lbPlacarTimeAzul = new System.Windows.Forms.Label();
            this.lbTimeVermelho = new System.Windows.Forms.Label();
            this.lbTimeAzul = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReposicBola = new System.Windows.Forms.Button();
            this.btnPausarPartida = new System.Windows.Forms.Button();
            this.btnIniciaPartida = new System.Windows.Forms.Button();
            this.btnPosicMeioCampo = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxTabelaQAtacante = new System.Windows.Forms.CheckBox();
            this.checkBoxTabelaQGoleiro = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numericUpDownEpisodios = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLog = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campoBackground)).BeginInit();
            this.gBPlacar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodios)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas1
            // 
            this.canvas1.BackColor = System.Drawing.SystemColors.GrayText;
            this.canvas1.ImageLayout = new System.Drawing.Point(1, 1);
            this.canvas1.Location = new System.Drawing.Point(88, 22);
            this.canvas1.Name = "canvas1";
            this.canvas1.PictureFile = global::AmbienteDeSimulacao.Properties.Resources.bola;
            this.canvas1.Size = new System.Drawing.Size(30, 30);
            this.canvas1.TabIndex = 3;
            this.canvas1.Visible = false;
            // 
            // canvas2
            // 
            this.canvas2.BackColor = System.Drawing.SystemColors.GrayText;
            this.canvas2.ImageLayout = new System.Drawing.Point(6, 4);
            this.canvas2.Location = new System.Drawing.Point(22, 75);
            this.canvas2.Name = "canvas2";
            this.canvas2.PictureFile = global::AmbienteDeSimulacao.Properties.Resources.sprite_jogador1_time1;
            this.canvas2.Size = new System.Drawing.Size(40, 40);
            this.canvas2.TabIndex = 3;
            this.canvas2.Visible = false;
            // 
            // canvas3
            // 
            this.canvas3.BackColor = System.Drawing.SystemColors.GrayText;
            this.canvas3.ImageLayout = new System.Drawing.Point(6, 4);
            this.canvas3.Location = new System.Drawing.Point(22, 22);
            this.canvas3.Name = "canvas3";
            this.canvas3.PictureFile = global::AmbienteDeSimulacao.Properties.Resources.sprite_jogador2_time1;
            this.canvas3.Size = new System.Drawing.Size(40, 40);
            this.canvas3.TabIndex = 3;
            this.canvas3.Visible = false;
            // 
            // canvas4
            // 
            this.canvas4.BackColor = System.Drawing.SystemColors.GrayText;
            this.canvas4.ImageLayout = new System.Drawing.Point(6, 4);
            this.canvas4.Location = new System.Drawing.Point(22, 150);
            this.canvas4.Name = "canvas4";
            this.canvas4.PictureFile = global::AmbienteDeSimulacao.Properties.Resources.sprite_jogador1_time2;
            this.canvas4.Size = new System.Drawing.Size(40, 40);
            this.canvas4.TabIndex = 3;
            this.canvas4.Visible = false;
            // 
            // canvas5
            // 
            this.canvas5.BackColor = System.Drawing.SystemColors.GrayText;
            this.canvas5.ImageLayout = new System.Drawing.Point(6, 4);
            this.canvas5.Location = new System.Drawing.Point(22, 220);
            this.canvas5.Name = "canvas5";
            this.canvas5.PictureFile = global::AmbienteDeSimulacao.Properties.Resources.sprite_jogador2_time2;
            this.canvas5.Size = new System.Drawing.Size(40, 40);
            this.canvas5.TabIndex = 3;
            this.canvas5.Visible = false;
            // 
            // timerAmbiente
            // 
            this.timerAmbiente.Enabled = true;
            this.timerAmbiente.Tick += new System.EventHandler(this.TimerAmbiente_Tick);
            // 
            // btnIniciaSimulacao
            // 
            this.btnIniciaSimulacao.Location = new System.Drawing.Point(6, 19);
            this.btnIniciaSimulacao.Name = "btnIniciaSimulacao";
            this.btnIniciaSimulacao.Size = new System.Drawing.Size(75, 23);
            this.btnIniciaSimulacao.TabIndex = 0;
            this.btnIniciaSimulacao.Text = "Iniciar";
            this.btnIniciaSimulacao.UseVisualStyleBackColor = true;
            this.btnIniciaSimulacao.Click += new System.EventHandler(this.btnIniciaSimulacao_Click);
            // 
            // btnFinalizaSimulacao
            // 
            this.btnFinalizaSimulacao.Enabled = false;
            this.btnFinalizaSimulacao.Location = new System.Drawing.Point(92, 19);
            this.btnFinalizaSimulacao.Name = "btnFinalizaSimulacao";
            this.btnFinalizaSimulacao.Size = new System.Drawing.Size(75, 23);
            this.btnFinalizaSimulacao.TabIndex = 3;
            this.btnFinalizaSimulacao.Text = "Finalizar";
            this.btnFinalizaSimulacao.UseVisualStyleBackColor = true;
            this.btnFinalizaSimulacao.Click += new System.EventHandler(this.btnFinalizaSimulacao_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnIniciaSimulacao);
            this.groupBox1.Controls.Add(this.btnFinalizaSimulacao);
            this.groupBox1.Location = new System.Drawing.Point(632, 409);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 53);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Simulação";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Highlight;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 450);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // checkBoxPontosOrientacao
            // 
            this.checkBoxPontosOrientacao.AutoSize = true;
            this.checkBoxPontosOrientacao.Enabled = false;
            this.checkBoxPontosOrientacao.Location = new System.Drawing.Point(632, 209);
            this.checkBoxPontosOrientacao.Name = "checkBoxPontosOrientacao";
            this.checkBoxPontosOrientacao.Size = new System.Drawing.Size(167, 17);
            this.checkBoxPontosOrientacao.TabIndex = 7;
            this.checkBoxPontosOrientacao.Text = "Pontos de Orientação (Física)";
            this.checkBoxPontosOrientacao.UseVisualStyleBackColor = true;
            this.checkBoxPontosOrientacao.CheckedChanged += new System.EventHandler(this.checkBoxPontosOrientacao_CheckedChanged);
            // 
            // campoBackground
            // 
            this.campoBackground.Image = global::AmbienteDeSimulacao.Properties.Resources.campo_futebol;
            this.campoBackground.Location = new System.Drawing.Point(426, 22);
            this.campoBackground.Name = "campoBackground";
            this.campoBackground.Size = new System.Drawing.Size(175, 86);
            this.campoBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.campoBackground.TabIndex = 6;
            this.campoBackground.TabStop = false;
            // 
            // checkBoxMovTeclado
            // 
            this.checkBoxMovTeclado.AutoSize = true;
            this.checkBoxMovTeclado.Enabled = false;
            this.checkBoxMovTeclado.Location = new System.Drawing.Point(632, 186);
            this.checkBoxMovTeclado.Name = "checkBoxMovTeclado";
            this.checkBoxMovTeclado.Size = new System.Drawing.Size(155, 17);
            this.checkBoxMovTeclado.TabIndex = 12;
            this.checkBoxMovTeclado.Text = "Movimentar com o Teclado";
            this.checkBoxMovTeclado.UseVisualStyleBackColor = true;
            // 
            // gBPlacar
            // 
            this.gBPlacar.Controls.Add(this.lbPlacarTimeVermelho);
            this.gBPlacar.Controls.Add(this.lbPlacarTimeAzul);
            this.gBPlacar.Controls.Add(this.lbTimeVermelho);
            this.gBPlacar.Controls.Add(this.lbTimeAzul);
            this.gBPlacar.Controls.Add(this.pictureBox3);
            this.gBPlacar.Controls.Add(this.pictureBox2);
            this.gBPlacar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBPlacar.Location = new System.Drawing.Point(632, 12);
            this.gBPlacar.Name = "gBPlacar";
            this.gBPlacar.Size = new System.Drawing.Size(180, 96);
            this.gBPlacar.TabIndex = 13;
            this.gBPlacar.TabStop = false;
            this.gBPlacar.Text = "Placar";
            // 
            // lbPlacarTimeVermelho
            // 
            this.lbPlacarTimeVermelho.AutoSize = true;
            this.lbPlacarTimeVermelho.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlacarTimeVermelho.Location = new System.Drawing.Point(141, 62);
            this.lbPlacarTimeVermelho.Name = "lbPlacarTimeVermelho";
            this.lbPlacarTimeVermelho.Size = new System.Drawing.Size(14, 13);
            this.lbPlacarTimeVermelho.TabIndex = 18;
            this.lbPlacarTimeVermelho.Text = "0";
            // 
            // lbPlacarTimeAzul
            // 
            this.lbPlacarTimeAzul.AutoSize = true;
            this.lbPlacarTimeAzul.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlacarTimeAzul.Location = new System.Drawing.Point(141, 26);
            this.lbPlacarTimeAzul.Name = "lbPlacarTimeAzul";
            this.lbPlacarTimeAzul.Size = new System.Drawing.Size(14, 13);
            this.lbPlacarTimeAzul.TabIndex = 17;
            this.lbPlacarTimeAzul.Text = "0";
            // 
            // lbTimeVermelho
            // 
            this.lbTimeVermelho.AutoSize = true;
            this.lbTimeVermelho.Location = new System.Drawing.Point(51, 62);
            this.lbTimeVermelho.Name = "lbTimeVermelho";
            this.lbTimeVermelho.Size = new System.Drawing.Size(80, 13);
            this.lbTimeVermelho.TabIndex = 16;
            this.lbTimeVermelho.Text = "Time Vermelho:";
            // 
            // lbTimeAzul
            // 
            this.lbTimeAzul.AutoSize = true;
            this.lbTimeAzul.Location = new System.Drawing.Point(51, 26);
            this.lbTimeAzul.Name = "lbTimeAzul";
            this.lbTimeAzul.Size = new System.Drawing.Size(56, 13);
            this.lbTimeAzul.TabIndex = 15;
            this.lbTimeAzul.Text = "Time Azul:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::AmbienteDeSimulacao.Properties.Resources.Jogador1_Time2;
            this.pictureBox3.Location = new System.Drawing.Point(15, 55);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(30, 30);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AmbienteDeSimulacao.Properties.Resources.Jogador1_Time1;
            this.pictureBox2.Location = new System.Drawing.Point(15, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 30);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReposicBola);
            this.groupBox2.Controls.Add(this.btnPausarPartida);
            this.groupBox2.Controls.Add(this.btnIniciaPartida);
            this.groupBox2.Controls.Add(this.btnPosicMeioCampo);
            this.groupBox2.Location = new System.Drawing.Point(632, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 142);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controles de Jogo";
            // 
            // btnReposicBola
            // 
            this.btnReposicBola.Enabled = false;
            this.btnReposicBola.Location = new System.Drawing.Point(7, 107);
            this.btnReposicBola.Name = "btnReposicBola";
            this.btnReposicBola.Size = new System.Drawing.Size(160, 23);
            this.btnReposicBola.TabIndex = 3;
            this.btnReposicBola.Text = "Reposic. Bola";
            this.btnReposicBola.UseVisualStyleBackColor = true;
            this.btnReposicBola.Click += new System.EventHandler(this.btnReposicBola_Click);
            // 
            // btnPausarPartida
            // 
            this.btnPausarPartida.Enabled = false;
            this.btnPausarPartida.Location = new System.Drawing.Point(7, 78);
            this.btnPausarPartida.Name = "btnPausarPartida";
            this.btnPausarPartida.Size = new System.Drawing.Size(160, 23);
            this.btnPausarPartida.TabIndex = 2;
            this.btnPausarPartida.Text = "Pausar Partida";
            this.btnPausarPartida.UseVisualStyleBackColor = true;
            this.btnPausarPartida.Click += new System.EventHandler(this.btnPausarPartida_Click);
            // 
            // btnIniciaPartida
            // 
            this.btnIniciaPartida.Enabled = false;
            this.btnIniciaPartida.Location = new System.Drawing.Point(7, 49);
            this.btnIniciaPartida.Name = "btnIniciaPartida";
            this.btnIniciaPartida.Size = new System.Drawing.Size(160, 23);
            this.btnIniciaPartida.TabIndex = 1;
            this.btnIniciaPartida.Text = "Iniciar Partida";
            this.btnIniciaPartida.UseVisualStyleBackColor = true;
            this.btnIniciaPartida.Click += new System.EventHandler(this.btIniciaPartida_Click);
            // 
            // btnPosicMeioCampo
            // 
            this.btnPosicMeioCampo.Enabled = false;
            this.btnPosicMeioCampo.Location = new System.Drawing.Point(7, 20);
            this.btnPosicMeioCampo.Name = "btnPosicMeioCampo";
            this.btnPosicMeioCampo.Size = new System.Drawing.Size(160, 23);
            this.btnPosicMeioCampo.TabIndex = 0;
            this.btnPosicMeioCampo.Text = "Posic. Meio Campo";
            this.btnPosicMeioCampo.UseVisualStyleBackColor = true;
            this.btnPosicMeioCampo.Click += new System.EventHandler(this.btPosicMeioCampo_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxTabelaQAtacante);
            this.groupBox3.Controls.Add(this.checkBoxTabelaQGoleiro);
            this.groupBox3.Location = new System.Drawing.Point(632, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(100, 66);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Salvar TabelaQ";
            // 
            // checkBoxTabelaQAtacante
            // 
            this.checkBoxTabelaQAtacante.AutoSize = true;
            this.checkBoxTabelaQAtacante.Checked = true;
            this.checkBoxTabelaQAtacante.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTabelaQAtacante.Enabled = false;
            this.checkBoxTabelaQAtacante.Location = new System.Drawing.Point(6, 42);
            this.checkBoxTabelaQAtacante.Name = "checkBoxTabelaQAtacante";
            this.checkBoxTabelaQAtacante.Size = new System.Drawing.Size(69, 17);
            this.checkBoxTabelaQAtacante.TabIndex = 1;
            this.checkBoxTabelaQAtacante.Text = "Atacante";
            this.checkBoxTabelaQAtacante.UseVisualStyleBackColor = true;
            // 
            // checkBoxTabelaQGoleiro
            // 
            this.checkBoxTabelaQGoleiro.AutoSize = true;
            this.checkBoxTabelaQGoleiro.Checked = true;
            this.checkBoxTabelaQGoleiro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTabelaQGoleiro.Enabled = false;
            this.checkBoxTabelaQGoleiro.Location = new System.Drawing.Point(6, 19);
            this.checkBoxTabelaQGoleiro.Name = "checkBoxTabelaQGoleiro";
            this.checkBoxTabelaQGoleiro.Size = new System.Drawing.Size(59, 17);
            this.checkBoxTabelaQGoleiro.TabIndex = 0;
            this.checkBoxTabelaQGoleiro.Text = "Goleiro";
            this.checkBoxTabelaQGoleiro.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numericUpDownEpisodios);
            this.groupBox4.Location = new System.Drawing.Point(738, 115);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(74, 65);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Episódios";
            // 
            // numericUpDownEpisodios
            // 
            this.numericUpDownEpisodios.Enabled = false;
            this.numericUpDownEpisodios.Location = new System.Drawing.Point(6, 25);
            this.numericUpDownEpisodios.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDownEpisodios.Name = "numericUpDownEpisodios";
            this.numericUpDownEpisodios.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownEpisodios.TabIndex = 0;
            this.numericUpDownEpisodios.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxLog
            // 
            this.checkBoxLog.AutoSize = true;
            this.checkBoxLog.Enabled = false;
            this.checkBoxLog.Location = new System.Drawing.Point(632, 233);
            this.checkBoxLog.Name = "checkBoxLog";
            this.checkBoxLog.Size = new System.Drawing.Size(111, 17);
            this.checkBoxLog.TabIndex = 17;
            this.checkBoxLog.Text = "Log da Simulação";
            this.checkBoxLog.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(824, 478);
            this.Controls.Add(this.checkBoxLog);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gBPlacar);
            this.Controls.Add(this.checkBoxMovTeclado);
            this.Controls.Add(this.checkBoxPontosOrientacao);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.campoBackground);
            this.Controls.Add(this.canvas1);
            this.Controls.Add(this.canvas2);
            this.Controls.Add(this.canvas3);
            this.Controls.Add(this.canvas4);
            this.Controls.Add(this.canvas5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(840, 516);
            this.MinimumSize = new System.Drawing.Size(840, 516);
            this.Name = "Form1";
            this.Text = "Futebol de Robôs";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campoBackground)).EndInit();
            this.gBPlacar.ResumeLayout(false);
            this.gBPlacar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEpisodios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerAmbiente;
        private System.Windows.Forms.Button btnIniciaSimulacao;
        private System.Windows.Forms.Button btnFinalizaSimulacao;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBoxPontosOrientacao;
        private System.Windows.Forms.PictureBox campoBackground;
        private System.Windows.Forms.CheckBox checkBoxMovTeclado;
        private System.Windows.Forms.GroupBox gBPlacar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbPlacarTimeVermelho;
        private System.Windows.Forms.Label lbPlacarTimeAzul;
        private System.Windows.Forms.Label lbTimeVermelho;
        private System.Windows.Forms.Label lbTimeAzul;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPosicMeioCampo;
        private System.Windows.Forms.Button btnIniciaPartida;
        private System.Windows.Forms.Button btnPausarPartida;
        private System.Windows.Forms.Button btnReposicBola;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxTabelaQAtacante;
        private System.Windows.Forms.CheckBox checkBoxTabelaQGoleiro;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown numericUpDownEpisodios;
        private System.Windows.Forms.CheckBox checkBoxLog;
    }
}

