namespace Thetis.Reports
{
    partial class AitiseisCount
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.���������GroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.������SumFunctionTextBox = new Telerik.Reporting.TextBox();
            this.���CountFunctionTextBox = new Telerik.Reporting.TextBox();
            this.���������GroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.���CaptionTextBox = new Telerik.Reporting.TextBox();
            this.������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.sqlDataSourceAitiseis = new Telerik.Reporting.SqlDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.���������DataTextBox = new Telerik.Reporting.TextBox();
            this.���������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.���DataTextBox = new Telerik.Reporting.TextBox();
            this.������DataTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.sqlDataSourceProkirixi = new Telerik.Reporting.SqlDataSource();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ���������GroupFooter
            // 
            this.���������GroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.97907495498657227D);
            this.���������GroupFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox1,
            this.������SumFunctionTextBox,
            this.���CountFunctionTextBox});
            this.���������GroupFooter.Name = "���������GroupFooter";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.2808256149291992D), Telerik.Reporting.Drawing.Unit.Cm(0.26499995589256287D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2195982933044434D), Telerik.Reporting.Drawing.Unit.Cm(0.71407490968704224D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "  ������ ��������:";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.26499995589256287D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.71407490968704224D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "  ������ ���:";
            // 
            // ������SumFunctionTextBox
            // 
            this.������SumFunctionTextBox.CanGrow = true;
            this.������SumFunctionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.500622749328613D), Telerik.Reporting.Drawing.Unit.Cm(0.26499995589256287D));
            this.������SumFunctionTextBox.Name = "������SumFunctionTextBox";
            this.������SumFunctionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.2562499046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.71407490968704224D));
            this.������SumFunctionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.������SumFunctionTextBox.Style.Color = System.Drawing.Color.White;
            this.������SumFunctionTextBox.Style.Font.Bold = true;
            this.������SumFunctionTextBox.StyleName = "Data";
            this.������SumFunctionTextBox.Value = "=\" \" +Sum(Fields.������)";
            // 
            // ���CountFunctionTextBox
            // 
            this.���CountFunctionTextBox.CanGrow = true;
            this.���CountFunctionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.1002001762390137D), Telerik.Reporting.Drawing.Unit.Cm(0.26499995589256287D));
            this.���CountFunctionTextBox.Name = "���CountFunctionTextBox";
            this.���CountFunctionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.1804251670837402D), Telerik.Reporting.Drawing.Unit.Cm(0.71407490968704224D));
            this.���CountFunctionTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            this.���CountFunctionTextBox.Style.Color = System.Drawing.Color.White;
            this.���CountFunctionTextBox.Style.Font.Bold = true;
            this.���CountFunctionTextBox.StyleName = "Data";
            this.���CountFunctionTextBox.Value = "=\" \" + Count(Fields.���)";
            // 
            // ���������GroupHeader
            // 
            this.���������GroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D);
            this.���������GroupHeader.Name = "���������GroupHeader";
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.50583314895629883D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���CaptionTextBox,
            this.������CaptionTextBox,
            this.textBox3});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // ���CaptionTextBox
            // 
            this.���CaptionTextBox.CanGrow = true;
            this.���CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2001996040344238D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.���CaptionTextBox.Name = "���CaptionTextBox";
            this.���CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.999600410461426D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.���CaptionTextBox.Style.Font.Bold = true;
            this.���CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.���CaptionTextBox.StyleName = "Caption";
            this.���CaptionTextBox.Value = " ���";
            // 
            // ������CaptionTextBox
            // 
            this.������CaptionTextBox.CanGrow = true;
            this.������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.200000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.������CaptionTextBox.Name = "������CaptionTextBox";
            this.������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5612497329711914D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.������CaptionTextBox.Style.Font.Bold = true;
            this.������CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.������CaptionTextBox.StyleName = "Caption";
            this.������CaptionTextBox.Value = " ������";
            // 
            // sqlDataSourceAitiseis
            // 
            this.sqlDataSourceAitiseis.ConnectionString = "Thetis.Properties.Settings.thetisDBConnectionString";
            this.sqlDataSourceAitiseis.Name = "sqlDataSourceAitiseis";
            this.sqlDataSourceAitiseis.SelectCommand = "SELECT        ���������, ���, ������\r\nFROM            AitisisCount\r\nGROUP BY ����" +
    "�����, ���, ������\r\nORDER BY ���������, ���";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.2999999523162842D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���������DataTextBox,
            this.���������CaptionTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // ���������DataTextBox
            // 
            this.���������DataTextBox.CanGrow = true;
            this.���������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.1485416889190674D), Telerik.Reporting.Drawing.Unit.Cm(0.29104167222976685D));
            this.���������DataTextBox.Name = "���������DataTextBox";
            this.���������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.608331680297852D), Telerik.Reporting.Drawing.Unit.Cm(0.71427518129348755D));
            this.���������DataTextBox.Style.Font.Bold = true;
            this.���������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.���������DataTextBox.StyleName = "Data";
            this.���������DataTextBox.Value = "=\"  \" + Fields.���������";
            // 
            // ���������CaptionTextBox
            // 
            this.���������CaptionTextBox.CanGrow = true;
            this.���������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.026458332315087318D), Telerik.Reporting.Drawing.Unit.Cm(0.29104167222976685D));
            this.���������CaptionTextBox.Name = "���������CaptionTextBox";
            this.���������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0468838214874268D), Telerik.Reporting.Drawing.Unit.Cm(0.71407490968704224D));
            this.���������CaptionTextBox.Style.Font.Bold = true;
            this.���������CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.���������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.���������CaptionTextBox.StyleName = "Caption";
            this.���������CaptionTextBox.Value = "���������:";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.79800873994827271D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.74509137868881226D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.74509137868881226D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���.\" + PageNumber + \" ��� \" + PageCount";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1000000238418579D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.814066886901856D), Telerik.Reporting.Drawing.Unit.Cm(1.085625171661377D));
            this.titleTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "������ �������� ��� ���������";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.994166910648346D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���DataTextBox,
            this.������DataTextBox,
            this.shape1,
            this.textBox4});
            this.detail.Name = "detail";
            // 
            // ���DataTextBox
            // 
            this.���DataTextBox.CanGrow = true;
            this.���DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2001998424530029D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.���DataTextBox.Name = "���DataTextBox";
            this.���DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.999600410461426D), Telerik.Reporting.Drawing.Unit.Cm(0.64125001430511475D));
            this.���DataTextBox.StyleName = "Data";
            this.���DataTextBox.Value = "=\" \" +Fields.���";
            // 
            // ������DataTextBox
            // 
            this.������DataTextBox.CanGrow = true;
            this.������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.200000762939453D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.������DataTextBox.Name = "������DataTextBox";
            this.������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5612497329711914D), Telerik.Reporting.Drawing.Unit.Cm(0.64125001430511475D));
            this.������DataTextBox.StyleName = "Data";
            this.������DataTextBox.Value = "=\" \" + Fields.������";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.6943669319152832D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.756773948669434D), Telerik.Reporting.Drawing.Unit.Cm(0.29980000853538513D));
            // 
            // sqlDataSourceProkirixi
            // 
            this.sqlDataSourceProkirixi.ConnectionString = "Thetis.Properties.Settings.thetisDBConnectionString";
            this.sqlDataSourceProkirixi.Name = "sqlDataSourceProkirixi";
            this.sqlDataSourceProkirixi.SelectCommand = "SELECT        ���������_��\r\nFROM            ���������";
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1470832824707031D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox3.StyleName = "Caption";
            this.textBox3.Value = " �/�";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1470832824707031D), Telerik.Reporting.Drawing.Unit.Cm(0.69406646490097046D));
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox4.StyleName = "Data";
            this.textBox4.Value = "= RowNumber() + \"]\"";
            // 
            // AitiseisCount
            // 
            this.DataSource = this.sqlDataSourceAitiseis;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.���������", Telerik.Reporting.FilterOperator.In, "=Parameters.ProkirixiSelect.Value"));
            group1.GroupFooter = this.���������GroupFooter;
            group1.GroupHeader = this.���������GroupHeader;
            group1.Name = "group1";
            group2.GroupFooter = this.labelsGroupFooterSection;
            group2.GroupHeader = this.labelsGroupHeaderSection;
            group2.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���������GroupHeader,
            this.���������GroupFooter,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.Name = "AitiseisCount";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AvailableValues.DataSource = this.sqlDataSourceProkirixi;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.���������_��";
            reportParameter1.AvailableValues.ValueMember = "= Fields.���������_��";
            reportParameter1.MultiValue = true;
            reportParameter1.Name = "ProkirixiSelect";
            reportParameter1.Text = "���������";
            reportParameter1.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule2.Style.Color = System.Drawing.Color.White;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.Black;
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSourceAitiseis;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox ���CaptionTextBox;
        private Telerik.Reporting.TextBox ������CaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox ���������DataTextBox;
        private Telerik.Reporting.TextBox ���������CaptionTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox ���DataTextBox;
        private Telerik.Reporting.TextBox ������DataTextBox;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.GroupHeaderSection ���������GroupHeader;
        private Telerik.Reporting.GroupFooterSection ���������GroupFooter;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox ������SumFunctionTextBox;
        private Telerik.Reporting.TextBox ���CountFunctionTextBox;
        private Telerik.Reporting.SqlDataSource sqlDataSourceProkirixi;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;

    }
}