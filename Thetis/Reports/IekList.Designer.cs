namespace Thetis.Reports
{
    partial class IekList
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
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.������������GroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.������������GroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.������������DataTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.���_EMAILCaptionTextBox = new Telerik.Reporting.TextBox();
            this.���_����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.���_���CaptionTextBox = new Telerik.Reporting.TextBox();
            this.���_FAXCaptionTextBox = new Telerik.Reporting.TextBox();
            this.���_��������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.sql_iekList = new Telerik.Reporting.SqlDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.���_EMAILDataTextBox = new Telerik.Reporting.TextBox();
            this.���_FAXDataTextBox = new Telerik.Reporting.TextBox();
            this.���_���DataTextBox = new Telerik.Reporting.TextBox();
            this.���_����DataTextBox = new Telerik.Reporting.TextBox();
            this.���_��������DataTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.textBox1 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ������������GroupFooter
            // 
            this.������������GroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89989984035491943D);
            this.������������GroupFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2});
            this.������������GroupFooter.Name = "������������GroupFooter";
            this.������������GroupFooter.PrintOnEveryPage = false;
            this.������������GroupFooter.Style.BackgroundColor = System.Drawing.Color.Silver;
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.2230181694030762D), Telerik.Reporting.Drawing.Unit.Cm(0.19989977777004242D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.19999885559082D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "= \"������ �.�.�.\" + \" - \" + Fields.������������ + \": \" + Count(Fields.���_�������" +
    "�)";
            // 
            // ������������GroupHeader
            // 
            this.������������GroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D);
            this.������������GroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.������������DataTextBox});
            this.������������GroupHeader.Name = "������������GroupHeader";
            this.������������GroupHeader.PrintOnEveryPage = true;
            // 
            // ������������DataTextBox
            // 
            this.������������DataTextBox.CanGrow = true;
            this.������������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.������������DataTextBox.Name = "������������DataTextBox";
            this.������������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.254585266113281D), Telerik.Reporting.Drawing.Unit.Cm(0.66541671752929688D));
            this.������������DataTextBox.Style.Font.Bold = true;
            this.������������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.������������DataTextBox.StyleName = "Data";
            this.������������DataTextBox.Value = "=Fields.������������";
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���_EMAILCaptionTextBox,
            this.���_����CaptionTextBox,
            this.���_���CaptionTextBox,
            this.���_FAXCaptionTextBox,
            this.���_��������CaptionTextBox});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // ���_EMAILCaptionTextBox
            // 
            this.���_EMAILCaptionTextBox.CanGrow = true;
            this.���_EMAILCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.26176643371582D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.���_EMAILCaptionTextBox.Name = "���_EMAILCaptionTextBox";
            this.���_EMAILCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1612515449523926D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_EMAILCaptionTextBox.Style.Font.Bold = true;
            this.���_EMAILCaptionTextBox.StyleName = "Caption";
            this.���_EMAILCaptionTextBox.Value = " E-mail";
            // 
            // ���_����CaptionTextBox
            // 
            this.���_����CaptionTextBox.CanGrow = true;
            this.���_����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.8473916053771973D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.���_����CaptionTextBox.Name = "���_����CaptionTextBox";
            this.���_����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_����CaptionTextBox.Style.Font.Bold = true;
            this.���_����CaptionTextBox.StyleName = "Caption";
            this.���_����CaptionTextBox.Value = " ����";
            // 
            // ���_���CaptionTextBox
            // 
            this.���_���CaptionTextBox.CanGrow = true;
            this.���_���CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9484329223632812D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.���_���CaptionTextBox.Name = "���_���CaptionTextBox";
            this.���_���CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1075010299682617D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_���CaptionTextBox.Style.Font.Bold = true;
            this.���_���CaptionTextBox.StyleName = "Caption";
            this.���_���CaptionTextBox.Value = " ��������";
            // 
            // ���_FAXCaptionTextBox
            // 
            this.���_FAXCaptionTextBox.CanGrow = true;
            this.���_FAXCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.160724639892578D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.���_FAXCaptionTextBox.Name = "���_FAXCaptionTextBox";
            this.���_FAXCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.023958683013916D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_FAXCaptionTextBox.Style.Font.Bold = true;
            this.���_FAXCaptionTextBox.StyleName = "Caption";
            this.���_FAXCaptionTextBox.Value = " Fax";
            // 
            // ���_��������CaptionTextBox
            // 
            this.���_��������CaptionTextBox.CanGrow = true;
            this.���_��������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.55572509765625D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.���_��������CaptionTextBox.Name = "���_��������CaptionTextBox";
            this.���_��������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_��������CaptionTextBox.Style.Font.Bold = true;
            this.���_��������CaptionTextBox.StyleName = "Caption";
            this.���_��������CaptionTextBox.Value = " �������� �.�.�.";
            // 
            // sql_iekList
            // 
            this.sql_iekList.ConnectionString = "Thetis.Properties.Settings.thetisDBConnectionString";
            this.sql_iekList.Name = "sql_iekList";
            this.sql_iekList.SelectCommand = "SELECT        qry���_������������.*\r\nFROM            qry���_������������";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(24.408334732055664D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.reportNameTextBox.Style.Font.Bold = true;
            this.reportNameTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "����� ��� ��� ������������ �/���";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.77125006914138794D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.177709579467773D), Telerik.Reporting.Drawing.Unit.Cm(0.71833264827728271D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.283542633056641D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.177709579467773D), Telerik.Reporting.Drawing.Unit.Cm(0.71833264827728271D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���.\" + PageNumber + \" ��� \" + PageCount";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0941669940948486D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(24.514167785644531D), Telerik.Reporting.Drawing.Unit.Cm(1.0939664840698242D));
            this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "����� �.�.�. - ��� ������������ �/���";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1854250431060791D);
            this.reportFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1});
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.90030032396316528D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.���_EMAILDataTextBox,
            this.���_FAXDataTextBox,
            this.���_���DataTextBox,
            this.���_����DataTextBox,
            this.���_��������DataTextBox,
            this.shape1});
            this.detail.Name = "detail";
            this.detail.Style.BackgroundColor = System.Drawing.Color.White;
            // 
            // ���_EMAILDataTextBox
            // 
            this.���_EMAILDataTextBox.CanGrow = true;
            this.���_EMAILDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.26176643371582D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.���_EMAILDataTextBox.Name = "���_EMAILDataTextBox";
            this.���_EMAILDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1612515449523926D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_EMAILDataTextBox.StyleName = "Data";
            this.���_EMAILDataTextBox.Value = "=Fields.���_EMAIL";
            // 
            // ���_FAXDataTextBox
            // 
            this.���_FAXDataTextBox.CanGrow = true;
            this.���_FAXDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.160724639892578D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.���_FAXDataTextBox.Name = "���_FAXDataTextBox";
            this.���_FAXDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.023958683013916D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_FAXDataTextBox.StyleName = "Data";
            this.���_FAXDataTextBox.Value = "=Fields.���_FAX";
            // 
            // ���_���DataTextBox
            // 
            this.���_���DataTextBox.CanGrow = true;
            this.���_���DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9484329223632812D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.���_���DataTextBox.Name = "���_���DataTextBox";
            this.���_���DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1075019836425781D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_���DataTextBox.StyleName = "Data";
            this.���_���DataTextBox.Value = "=Fields.���_���";
            // 
            // ���_����DataTextBox
            // 
            this.���_����DataTextBox.CanGrow = true;
            this.���_����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.8473916053771973D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.���_����DataTextBox.Name = "���_����DataTextBox";
            this.���_����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_����DataTextBox.StyleName = "Data";
            this.���_����DataTextBox.Value = "=Fields.���_����";
            // 
            // ���_��������DataTextBox
            // 
            this.���_��������DataTextBox.CanGrow = true;
            this.���_��������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.55572509765625D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.���_��������DataTextBox.Name = "���_��������DataTextBox";
            this.���_��������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1999993324279785D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.���_��������DataTextBox.StyleName = "Data";
            this.���_��������DataTextBox.Value = "=Fields.���_��������";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.55572509765625D), Telerik.Reporting.Drawing.Unit.Cm(0.60030049085617065D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(23.958442687988281D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9484329223632812D), Telerik.Reporting.Drawing.Unit.Cm(0.285425066947937D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.671466827392578D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Name = "Tahoma";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "=\"������ �.�.�. : \" + Count(Fields.���_��������)";
            // 
            // IekList
            // 
            this.DataSource = this.sql_iekList;
            group1.GroupFooter = this.������������GroupFooter;
            group1.GroupHeader = this.������������GroupHeader;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.������������"));
            group1.Name = "group1";
            group2.GroupFooter = this.labelsGroupFooterSection;
            group2.GroupHeader = this.labelsGroupHeaderSection;
            group2.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.������������GroupHeader,
            this.������������GroupFooter,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.Name = "IekList";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(24.6200008392334D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sql_iekList;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.GroupHeaderSection ������������GroupHeader;
        private Telerik.Reporting.TextBox ������������DataTextBox;
        private Telerik.Reporting.GroupFooterSection ������������GroupFooter;
        private Telerik.Reporting.TextBox ���_EMAILDataTextBox;
        private Telerik.Reporting.TextBox ���_FAXDataTextBox;
        private Telerik.Reporting.TextBox ���_���DataTextBox;
        private Telerik.Reporting.TextBox ���_����DataTextBox;
        private Telerik.Reporting.TextBox ���_��������DataTextBox;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox ���_EMAILCaptionTextBox;
        private Telerik.Reporting.TextBox ���_����CaptionTextBox;
        private Telerik.Reporting.TextBox ���_���CaptionTextBox;
        private Telerik.Reporting.TextBox ���_FAXCaptionTextBox;
        private Telerik.Reporting.TextBox ���_��������CaptionTextBox;
        private Telerik.Reporting.TextBox textBox1;

    }
}