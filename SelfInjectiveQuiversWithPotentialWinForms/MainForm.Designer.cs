namespace SelfInjectiveQuiversWithPotentialWinForms
{
    partial class MainForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cobwebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evenFlowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointedFlowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.generalizedCobwebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromMutationAppFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsMutationAppFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.relabelVerticesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectMove = new System.Windows.Forms.Button();
            this.btnAddVertex = new System.Windows.Forms.Button();
            this.nudVertexToAdd = new System.Windows.Forms.NumericUpDown();
            this.btnAddArrow = new System.Windows.Forms.Button();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.lblSelectPath = new System.Windows.Forms.Label();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.lblTool3 = new System.Windows.Forms.Label();
            this.lblTool2 = new System.Windows.Forms.Label();
            this.lblTool1 = new System.Windows.Forms.Label();
            this.grpToolSettings = new System.Windows.Forms.GroupBox();
            this.lblVertexToAdd = new System.Windows.Forms.Label();
            this.lstVertices = new System.Windows.Forms.ListView();
            this.lstArrows = new System.Windows.Forms.ListView();
            this.lblVertices = new System.Windows.Forms.Label();
            this.lblArrows = new System.Windows.Forms.Label();
            this.lblVertexCount = new System.Windows.Forms.Label();
            this.lblArrowCount = new System.Windows.Forms.Label();
            this.lblAnalysisMainResult = new System.Windows.Forms.Label();
            this.lstNakayamaPermutation = new System.Windows.Forms.ListView();
            this.lblNakayamaPermutation = new System.Windows.Forms.Label();
            this.lstMaximalPathRepresentatives = new System.Windows.Forms.ListView();
            this.representativeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblMaximalPathRepresentatives = new System.Windows.Forms.Label();
            this.grpAnalysisResults = new System.Windows.Forms.GroupBox();
            this.tabAnalysisResults = new System.Windows.Forms.TabControl();
            this.nakayamaTabPage = new System.Windows.Forms.TabPage();
            this.txtOrbit = new System.Windows.Forms.TextBox();
            this.lblOrbit = new System.Windows.Forms.Label();
            this.miscellaneousTabPage = new System.Windows.Forms.TabPage();
            this.txtLongestPathEncountered = new System.Windows.Forms.TextBox();
            this.lblLongestPathLength = new System.Windows.Forms.Label();
            this.lblLongestPathEncountered = new System.Windows.Forms.Label();
            this.exportAsMutationAppFileSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importFromMutationAppFileOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblCenterOfCanvas = new System.Windows.Forms.Label();
            this.rotateVerticesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canvas = new SelfInjectiveQuiversWithPotentialWinForms.Canvas();
            this.lblMousePointerOnCanvasLocation = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexToAdd)).BeginInit();
            this.grpTools.SuspendLayout();
            this.grpToolSettings.SuspendLayout();
            this.grpAnalysisResults.SuspendLayout();
            this.tabAnalysisResults.SuspendLayout();
            this.nakayamaTabPage.SuspendLayout();
            this.miscellaneousTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1137, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.importFromMutationAppFileToolStripMenuItem,
            this.exportAsMutationAppFileToolStripMenuItem,
            this.separatorToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cycleToolStripMenuItem,
            this.triangleToolStripMenuItem,
            this.squareToolStripMenuItem,
            this.cobwebToolStripMenuItem,
            this.flowerToolStripMenuItem,
            this.evenFlowerToolStripMenuItem,
            this.pointedFlowerToolStripMenuItem,
            this.separatorToolStripMenuItem2,
            this.generalizedCobwebToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // cycleToolStripMenuItem
            // 
            this.cycleToolStripMenuItem.Name = "cycleToolStripMenuItem";
            this.cycleToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.cycleToolStripMenuItem.Text = "Cycle...";
            // 
            // triangleToolStripMenuItem
            // 
            this.triangleToolStripMenuItem.Name = "triangleToolStripMenuItem";
            this.triangleToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.triangleToolStripMenuItem.Text = "Triangle...";
            // 
            // squareToolStripMenuItem
            // 
            this.squareToolStripMenuItem.Name = "squareToolStripMenuItem";
            this.squareToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.squareToolStripMenuItem.Text = "Square...";
            // 
            // cobwebToolStripMenuItem
            // 
            this.cobwebToolStripMenuItem.Name = "cobwebToolStripMenuItem";
            this.cobwebToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.cobwebToolStripMenuItem.Text = "Cobweb...";
            // 
            // flowerToolStripMenuItem
            // 
            this.flowerToolStripMenuItem.Name = "flowerToolStripMenuItem";
            this.flowerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.flowerToolStripMenuItem.Text = "Flower...";
            // 
            // evenFlowerToolStripMenuItem
            // 
            this.evenFlowerToolStripMenuItem.Name = "evenFlowerToolStripMenuItem";
            this.evenFlowerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.evenFlowerToolStripMenuItem.Text = "Even flower...";
            // 
            // pointedFlowerToolStripMenuItem
            // 
            this.pointedFlowerToolStripMenuItem.Name = "pointedFlowerToolStripMenuItem";
            this.pointedFlowerToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.pointedFlowerToolStripMenuItem.Text = "Pointed flower...";
            // 
            // separatorToolStripMenuItem2
            // 
            this.separatorToolStripMenuItem2.Name = "separatorToolStripMenuItem2";
            this.separatorToolStripMenuItem2.Size = new System.Drawing.Size(186, 6);
            // 
            // generalizedCobwebToolStripMenuItem
            // 
            this.generalizedCobwebToolStripMenuItem.Name = "generalizedCobwebToolStripMenuItem";
            this.generalizedCobwebToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.generalizedCobwebToolStripMenuItem.Text = "Generalized cobweb...";
            // 
            // importFromMutationAppFileToolStripMenuItem
            // 
            this.importFromMutationAppFileToolStripMenuItem.Name = "importFromMutationAppFileToolStripMenuItem";
            this.importFromMutationAppFileToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.importFromMutationAppFileToolStripMenuItem.Text = "Import from Mutation App file...";
            // 
            // exportAsMutationAppFileToolStripMenuItem
            // 
            this.exportAsMutationAppFileToolStripMenuItem.Name = "exportAsMutationAppFileToolStripMenuItem";
            this.exportAsMutationAppFileToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.exportAsMutationAppFileToolStripMenuItem.Text = "Export as Mutation App file...";
            // 
            // separatorToolStripMenuItem
            // 
            this.separatorToolStripMenuItem.Name = "separatorToolStripMenuItem";
            this.separatorToolStripMenuItem.Size = new System.Drawing.Size(241, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.separatorToolStripMenuItem3,
            this.relabelVerticesToolStripMenuItem,
            this.rotateVerticesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // separatorToolStripMenuItem3
            // 
            this.separatorToolStripMenuItem3.Name = "separatorToolStripMenuItem3";
            this.separatorToolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // relabelVerticesToolStripMenuItem
            // 
            this.relabelVerticesToolStripMenuItem.Name = "relabelVerticesToolStripMenuItem";
            this.relabelVerticesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.relabelVerticesToolStripMenuItem.Text = "Relabel vertices...";
            // 
            // btnSelectMove
            // 
            this.btnSelectMove.Location = new System.Drawing.Point(25, 19);
            this.btnSelectMove.Name = "btnSelectMove";
            this.btnSelectMove.Size = new System.Drawing.Size(91, 23);
            this.btnSelectMove.TabIndex = 1;
            this.btnSelectMove.Text = "Select/move";
            this.btnSelectMove.UseVisualStyleBackColor = true;
            // 
            // btnAddVertex
            // 
            this.btnAddVertex.Location = new System.Drawing.Point(25, 48);
            this.btnAddVertex.Name = "btnAddVertex";
            this.btnAddVertex.Size = new System.Drawing.Size(91, 23);
            this.btnAddVertex.TabIndex = 3;
            this.btnAddVertex.Text = "Add vertex";
            this.btnAddVertex.UseVisualStyleBackColor = true;
            // 
            // nudVertexToAdd
            // 
            this.nudVertexToAdd.Location = new System.Drawing.Point(6, 46);
            this.nudVertexToAdd.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudVertexToAdd.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.nudVertexToAdd.Name = "nudVertexToAdd";
            this.nudVertexToAdd.Size = new System.Drawing.Size(70, 20);
            this.nudVertexToAdd.TabIndex = 1;
            this.nudVertexToAdd.Visible = false;
            // 
            // btnAddArrow
            // 
            this.btnAddArrow.Location = new System.Drawing.Point(25, 77);
            this.btnAddArrow.Name = "btnAddArrow";
            this.btnAddArrow.Size = new System.Drawing.Size(91, 23);
            this.btnAddArrow.TabIndex = 5;
            this.btnAddArrow.Text = "Add arrow";
            this.btnAddArrow.UseVisualStyleBackColor = true;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(788, 461);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 10;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.lblSelectPath);
            this.grpTools.Controls.Add(this.btnSelectPath);
            this.grpTools.Controls.Add(this.lblTool3);
            this.grpTools.Controls.Add(this.lblTool2);
            this.grpTools.Controls.Add(this.lblTool1);
            this.grpTools.Controls.Add(this.btnSelectMove);
            this.grpTools.Controls.Add(this.btnAddVertex);
            this.grpTools.Controls.Add(this.btnAddArrow);
            this.grpTools.Location = new System.Drawing.Point(789, 29);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(122, 136);
            this.grpTools.TabIndex = 2;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Tools";
            // 
            // lblSelectPath
            // 
            this.lblSelectPath.AutoSize = true;
            this.lblSelectPath.Location = new System.Drawing.Point(6, 111);
            this.lblSelectPath.Name = "lblSelectPath";
            this.lblSelectPath.Size = new System.Drawing.Size(13, 13);
            this.lblSelectPath.TabIndex = 6;
            this.lblSelectPath.Text = "4";
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(25, 106);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(90, 23);
            this.btnSelectPath.TabIndex = 7;
            this.btnSelectPath.Text = "Select path";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            // 
            // lblTool3
            // 
            this.lblTool3.AutoSize = true;
            this.lblTool3.Location = new System.Drawing.Point(6, 82);
            this.lblTool3.Name = "lblTool3";
            this.lblTool3.Size = new System.Drawing.Size(13, 13);
            this.lblTool3.TabIndex = 4;
            this.lblTool3.Text = "3";
            // 
            // lblTool2
            // 
            this.lblTool2.AutoSize = true;
            this.lblTool2.Location = new System.Drawing.Point(6, 53);
            this.lblTool2.Name = "lblTool2";
            this.lblTool2.Size = new System.Drawing.Size(13, 13);
            this.lblTool2.TabIndex = 2;
            this.lblTool2.Text = "2";
            // 
            // lblTool1
            // 
            this.lblTool1.AutoSize = true;
            this.lblTool1.Location = new System.Drawing.Point(6, 24);
            this.lblTool1.Name = "lblTool1";
            this.lblTool1.Size = new System.Drawing.Size(13, 13);
            this.lblTool1.TabIndex = 0;
            this.lblTool1.Text = "1";
            // 
            // grpToolSettings
            // 
            this.grpToolSettings.Controls.Add(this.lblVertexToAdd);
            this.grpToolSettings.Controls.Add(this.nudVertexToAdd);
            this.grpToolSettings.Location = new System.Drawing.Point(917, 29);
            this.grpToolSettings.Name = "grpToolSettings";
            this.grpToolSettings.Size = new System.Drawing.Size(189, 136);
            this.grpToolSettings.TabIndex = 3;
            this.grpToolSettings.TabStop = false;
            this.grpToolSettings.Text = "Tool settings";
            // 
            // lblVertexToAdd
            // 
            this.lblVertexToAdd.AutoSize = true;
            this.lblVertexToAdd.Location = new System.Drawing.Point(6, 24);
            this.lblVertexToAdd.Name = "lblVertexToAdd";
            this.lblVertexToAdd.Size = new System.Drawing.Size(70, 13);
            this.lblVertexToAdd.TabIndex = 0;
            this.lblVertexToAdd.Text = "Vertex to add";
            this.lblVertexToAdd.Visible = false;
            // 
            // lstVertices
            // 
            this.lstVertices.HideSelection = false;
            this.lstVertices.Location = new System.Drawing.Point(789, 205);
            this.lstVertices.MultiSelect = false;
            this.lstVertices.Name = "lstVertices";
            this.lstVertices.Size = new System.Drawing.Size(121, 217);
            this.lstVertices.TabIndex = 5;
            this.lstVertices.UseCompatibleStateImageBehavior = false;
            this.lstVertices.View = System.Windows.Forms.View.List;
            // 
            // lstArrows
            // 
            this.lstArrows.HideSelection = false;
            this.lstArrows.Location = new System.Drawing.Point(916, 205);
            this.lstArrows.MultiSelect = false;
            this.lstArrows.Name = "lstArrows";
            this.lstArrows.Size = new System.Drawing.Size(189, 217);
            this.lstArrows.TabIndex = 8;
            this.lstArrows.UseCompatibleStateImageBehavior = false;
            this.lstArrows.View = System.Windows.Forms.View.List;
            // 
            // lblVertices
            // 
            this.lblVertices.AutoSize = true;
            this.lblVertices.Location = new System.Drawing.Point(788, 185);
            this.lblVertices.Name = "lblVertices";
            this.lblVertices.Size = new System.Drawing.Size(45, 13);
            this.lblVertices.TabIndex = 4;
            this.lblVertices.Text = "Vertices";
            // 
            // lblArrows
            // 
            this.lblArrows.AutoSize = true;
            this.lblArrows.Location = new System.Drawing.Point(913, 185);
            this.lblArrows.Name = "lblArrows";
            this.lblArrows.Size = new System.Drawing.Size(39, 13);
            this.lblArrows.TabIndex = 7;
            this.lblArrows.Text = "Arrows";
            // 
            // lblVertexCount
            // 
            this.lblVertexCount.AutoSize = true;
            this.lblVertexCount.Location = new System.Drawing.Point(788, 425);
            this.lblVertexCount.Name = "lblVertexCount";
            this.lblVertexCount.Size = new System.Drawing.Size(79, 13);
            this.lblVertexCount.TabIndex = 6;
            this.lblVertexCount.Text = "Vertex count: 0";
            // 
            // lblArrowCount
            // 
            this.lblArrowCount.AutoSize = true;
            this.lblArrowCount.Location = new System.Drawing.Point(913, 425);
            this.lblArrowCount.Name = "lblArrowCount";
            this.lblArrowCount.Size = new System.Drawing.Size(76, 13);
            this.lblArrowCount.TabIndex = 9;
            this.lblArrowCount.Text = "Arrow count: 0";
            // 
            // lblAnalysisMainResult
            // 
            this.lblAnalysisMainResult.AutoSize = true;
            this.lblAnalysisMainResult.Location = new System.Drawing.Point(6, 23);
            this.lblAnalysisMainResult.Name = "lblAnalysisMainResult";
            this.lblAnalysisMainResult.Size = new System.Drawing.Size(90, 13);
            this.lblAnalysisMainResult.TabIndex = 0;
            this.lblAnalysisMainResult.Text = "Main result: None";
            // 
            // lstNakayamaPermutation
            // 
            this.lstNakayamaPermutation.Location = new System.Drawing.Point(8, 156);
            this.lstNakayamaPermutation.MultiSelect = false;
            this.lstNakayamaPermutation.Name = "lstNakayamaPermutation";
            this.lstNakayamaPermutation.Size = new System.Drawing.Size(302, 98);
            this.lstNakayamaPermutation.TabIndex = 4;
            this.lstNakayamaPermutation.UseCompatibleStateImageBehavior = false;
            this.lstNakayamaPermutation.View = System.Windows.Forms.View.List;
            // 
            // lblNakayamaPermutation
            // 
            this.lblNakayamaPermutation.AutoSize = true;
            this.lblNakayamaPermutation.Location = new System.Drawing.Point(5, 140);
            this.lblNakayamaPermutation.Name = "lblNakayamaPermutation";
            this.lblNakayamaPermutation.Size = new System.Drawing.Size(116, 13);
            this.lblNakayamaPermutation.TabIndex = 3;
            this.lblNakayamaPermutation.Text = "Nakayama permutation";
            // 
            // lstMaximalPathRepresentatives
            // 
            this.lstMaximalPathRepresentatives.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.representativeColumnHeader});
            this.lstMaximalPathRepresentatives.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstMaximalPathRepresentatives.Location = new System.Drawing.Point(5, 19);
            this.lstMaximalPathRepresentatives.Name = "lstMaximalPathRepresentatives";
            this.lstMaximalPathRepresentatives.Size = new System.Drawing.Size(305, 106);
            this.lstMaximalPathRepresentatives.TabIndex = 2;
            this.lstMaximalPathRepresentatives.UseCompatibleStateImageBehavior = false;
            this.lstMaximalPathRepresentatives.View = System.Windows.Forms.View.Details;
            // 
            // representativeColumnHeader
            // 
            this.representativeColumnHeader.Text = "";
            this.representativeColumnHeader.Width = 301;
            // 
            // lblMaximalPathRepresentatives
            // 
            this.lblMaximalPathRepresentatives.AutoSize = true;
            this.lblMaximalPathRepresentatives.Location = new System.Drawing.Point(6, 3);
            this.lblMaximalPathRepresentatives.Name = "lblMaximalPathRepresentatives";
            this.lblMaximalPathRepresentatives.Size = new System.Drawing.Size(161, 13);
            this.lblMaximalPathRepresentatives.TabIndex = 1;
            this.lblMaximalPathRepresentatives.Text = "Maximal nonzero representatives";
            // 
            // grpAnalysisResults
            // 
            this.grpAnalysisResults.Controls.Add(this.tabAnalysisResults);
            this.grpAnalysisResults.Controls.Add(this.lblAnalysisMainResult);
            this.grpAnalysisResults.Enabled = false;
            this.grpAnalysisResults.Location = new System.Drawing.Point(789, 490);
            this.grpAnalysisResults.Name = "grpAnalysisResults";
            this.grpAnalysisResults.Size = new System.Drawing.Size(336, 369);
            this.grpAnalysisResults.TabIndex = 11;
            this.grpAnalysisResults.TabStop = false;
            this.grpAnalysisResults.Text = "Analysis results";
            // 
            // tabAnalysisResults
            // 
            this.tabAnalysisResults.Controls.Add(this.nakayamaTabPage);
            this.tabAnalysisResults.Controls.Add(this.miscellaneousTabPage);
            this.tabAnalysisResults.Location = new System.Drawing.Point(9, 51);
            this.tabAnalysisResults.Name = "tabAnalysisResults";
            this.tabAnalysisResults.SelectedIndex = 0;
            this.tabAnalysisResults.Size = new System.Drawing.Size(324, 312);
            this.tabAnalysisResults.TabIndex = 7;
            // 
            // nakayamaTabPage
            // 
            this.nakayamaTabPage.Controls.Add(this.txtOrbit);
            this.nakayamaTabPage.Controls.Add(this.lblMaximalPathRepresentatives);
            this.nakayamaTabPage.Controls.Add(this.lblOrbit);
            this.nakayamaTabPage.Controls.Add(this.lstMaximalPathRepresentatives);
            this.nakayamaTabPage.Controls.Add(this.lstNakayamaPermutation);
            this.nakayamaTabPage.Controls.Add(this.lblNakayamaPermutation);
            this.nakayamaTabPage.Location = new System.Drawing.Point(4, 22);
            this.nakayamaTabPage.Name = "nakayamaTabPage";
            this.nakayamaTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.nakayamaTabPage.Size = new System.Drawing.Size(316, 286);
            this.nakayamaTabPage.TabIndex = 0;
            this.nakayamaTabPage.Text = "Things Nakayama";
            this.nakayamaTabPage.UseVisualStyleBackColor = true;
            // 
            // txtOrbit
            // 
            this.txtOrbit.Location = new System.Drawing.Point(43, 260);
            this.txtOrbit.Name = "txtOrbit";
            this.txtOrbit.ReadOnly = true;
            this.txtOrbit.Size = new System.Drawing.Size(267, 20);
            this.txtOrbit.TabIndex = 6;
            // 
            // lblOrbit
            // 
            this.lblOrbit.AutoSize = true;
            this.lblOrbit.Location = new System.Drawing.Point(5, 263);
            this.lblOrbit.Name = "lblOrbit";
            this.lblOrbit.Size = new System.Drawing.Size(29, 13);
            this.lblOrbit.TabIndex = 5;
            this.lblOrbit.Text = "Orbit";
            // 
            // miscellaneousTabPage
            // 
            this.miscellaneousTabPage.Controls.Add(this.txtLongestPathEncountered);
            this.miscellaneousTabPage.Controls.Add(this.lblLongestPathLength);
            this.miscellaneousTabPage.Controls.Add(this.lblLongestPathEncountered);
            this.miscellaneousTabPage.Location = new System.Drawing.Point(4, 22);
            this.miscellaneousTabPage.Name = "miscellaneousTabPage";
            this.miscellaneousTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.miscellaneousTabPage.Size = new System.Drawing.Size(316, 286);
            this.miscellaneousTabPage.TabIndex = 1;
            this.miscellaneousTabPage.Text = "Misc.";
            this.miscellaneousTabPage.UseVisualStyleBackColor = true;
            // 
            // txtLongestPathEncountered
            // 
            this.txtLongestPathEncountered.Location = new System.Drawing.Point(6, 19);
            this.txtLongestPathEncountered.Name = "txtLongestPathEncountered";
            this.txtLongestPathEncountered.ReadOnly = true;
            this.txtLongestPathEncountered.Size = new System.Drawing.Size(304, 20);
            this.txtLongestPathEncountered.TabIndex = 2;
            // 
            // lblLongestPathLength
            // 
            this.lblLongestPathLength.AutoSize = true;
            this.lblLongestPathLength.Location = new System.Drawing.Point(9, 52);
            this.lblLongestPathLength.Name = "lblLongestPathLength";
            this.lblLongestPathLength.Size = new System.Drawing.Size(107, 13);
            this.lblLongestPathLength.TabIndex = 1;
            this.lblLongestPathLength.Text = "Longest path length: ";
            // 
            // lblLongestPathEncountered
            // 
            this.lblLongestPathEncountered.AutoSize = true;
            this.lblLongestPathEncountered.Location = new System.Drawing.Point(6, 3);
            this.lblLongestPathEncountered.Name = "lblLongestPathEncountered";
            this.lblLongestPathEncountered.Size = new System.Drawing.Size(135, 13);
            this.lblLongestPathEncountered.TabIndex = 0;
            this.lblLongestPathEncountered.Text = "Longest path encountered:";
            // 
            // lblCenterOfCanvas
            // 
            this.lblCenterOfCanvas.AutoSize = true;
            this.lblCenterOfCanvas.Location = new System.Drawing.Point(12, 806);
            this.lblCenterOfCanvas.Name = "lblCenterOfCanvas";
            this.lblCenterOfCanvas.Size = new System.Drawing.Size(68, 13);
            this.lblCenterOfCanvas.TabIndex = 12;
            this.lblCenterOfCanvas.Text = "Center: (0, 0)";
            // 
            // rotateVerticesToolStripMenuItem
            // 
            this.rotateVerticesToolStripMenuItem.Name = "rotateVerticesToolStripMenuItem";
            this.rotateVerticesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rotateVerticesToolStripMenuItem.Text = "Rotate vertices...";
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.Location = new System.Drawing.Point(12, 27);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(770, 776);
            this.canvas.TabIndex = 1;
            // 
            // lblMousePointerOnCanvasLocation
            // 
            this.lblMousePointerOnCanvasLocation.AutoSize = true;
            this.lblMousePointerOnCanvasLocation.Location = new System.Drawing.Point(189, 806);
            this.lblMousePointerOnCanvasLocation.Name = "lblMousePointerOnCanvasLocation";
            this.lblMousePointerOnCanvasLocation.Size = new System.Drawing.Size(70, 13);
            this.lblMousePointerOnCanvasLocation.TabIndex = 13;
            this.lblMousePointerOnCanvasLocation.Text = "Pointer: (0, 0)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1137, 871);
            this.Controls.Add(this.lblMousePointerOnCanvasLocation);
            this.Controls.Add(this.lblCenterOfCanvas);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.grpAnalysisResults);
            this.Controls.Add(this.lblArrowCount);
            this.Controls.Add(this.lblVertexCount);
            this.Controls.Add(this.lblArrows);
            this.Controls.Add(this.lblVertices);
            this.Controls.Add(this.lstArrows);
            this.Controls.Add(this.lstVertices);
            this.Controls.Add(this.grpToolSettings);
            this.Controls.Add(this.grpTools);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "QP analyzer";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexToAdd)).EndInit();
            this.grpTools.ResumeLayout(false);
            this.grpTools.PerformLayout();
            this.grpToolSettings.ResumeLayout(false);
            this.grpToolSettings.PerformLayout();
            this.grpAnalysisResults.ResumeLayout(false);
            this.grpAnalysisResults.PerformLayout();
            this.tabAnalysisResults.ResumeLayout(false);
            this.nakayamaTabPage.ResumeLayout(false);
            this.nakayamaTabPage.PerformLayout();
            this.miscellaneousTabPage.ResumeLayout(false);
            this.miscellaneousTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.Button btnSelectMove;
        private System.Windows.Forms.Button btnAddVertex;
        private System.Windows.Forms.NumericUpDown nudVertexToAdd;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnAddArrow;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Label lblTool3;
        private System.Windows.Forms.Label lblTool2;
        private System.Windows.Forms.Label lblTool1;
        private System.Windows.Forms.GroupBox grpToolSettings;
        private System.Windows.Forms.Label lblVertexToAdd;
        private System.Windows.Forms.ListView lstVertices;
        private System.Windows.Forms.ListView lstArrows;
        private System.Windows.Forms.Label lblVertices;
        private System.Windows.Forms.Label lblArrows;
        private System.Windows.Forms.Label lblVertexCount;
        private System.Windows.Forms.Label lblArrowCount;
        private System.Windows.Forms.Label lblAnalysisMainResult;
        private System.Windows.Forms.ListView lstNakayamaPermutation;
        private System.Windows.Forms.Label lblNakayamaPermutation;
        private System.Windows.Forms.Label lblSelectPath;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.ListView lstMaximalPathRepresentatives;
        private System.Windows.Forms.Label lblMaximalPathRepresentatives;
        private System.Windows.Forms.GroupBox grpAnalysisResults;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triangleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cobwebToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator separatorToolStripMenuItem;
        private Canvas canvas;
        private System.Windows.Forms.ColumnHeader representativeColumnHeader;
        private System.Windows.Forms.ToolStripMenuItem evenFlowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAsMutationAppFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportAsMutationAppFileSaveFileDialog;
        private System.Windows.Forms.OpenFileDialog importFromMutationAppFileOpenFileDialog;
        private System.Windows.Forms.ToolStripMenuItem importFromMutationAppFileToolStripMenuItem;
        private System.Windows.Forms.Label lblCenterOfCanvas;
        private System.Windows.Forms.ToolStripSeparator separatorToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem generalizedCobwebToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointedFlowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator separatorToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem relabelVerticesToolStripMenuItem;
        private System.Windows.Forms.TextBox txtOrbit;
        private System.Windows.Forms.Label lblOrbit;
        private System.Windows.Forms.TabControl tabAnalysisResults;
        private System.Windows.Forms.TabPage nakayamaTabPage;
        private System.Windows.Forms.TabPage miscellaneousTabPage;
        private System.Windows.Forms.TextBox txtLongestPathEncountered;
        private System.Windows.Forms.Label lblLongestPathLength;
        private System.Windows.Forms.Label lblLongestPathEncountered;
        private System.Windows.Forms.ToolStripMenuItem rotateVerticesToolStripMenuItem;
        private System.Windows.Forms.Label lblMousePointerOnCanvasLocation;
    }
}

