using iTextSharp.text;
using iTextSharp.text.pdf;
using JKS_Report.Function.DB;
using JKS_Report.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKS_Report.Function.API
{
    public class MultipleRecordReport
    {

        string filename; Document doc; 
        static PdfWriter writer; // string variable;
        static iTextSharp.text.Font rprttitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 15, BaseColor.BLACK);
        static iTextSharp.text.Font rprtcond = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.BLACK);
        static iTextSharp.text.Font seconds = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
        static iTextSharp.text.Font twice = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);
        static iTextSharp.text.Font para = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
        static iTextSharp.text.Font value = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
        static iTextSharp.text.Font firstbold = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        static iTextSharp.text.Font first = FontFactory.GetFont("Times New Roman", 10, BaseColor.BLACK);
        //    static iTextSharp.text.Font firstbold = FontFactory.GetFont("Times New Roman", 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //  static iTextSharp.text.Font first = FontFactory.GetFont("Times New Roman", 9, BaseColor.BLACK);
        static iTextSharp.text.Font second = FontFactory.GetFont("Times New Roman", 9, BaseColor.BLACK);
        static iTextSharp.text.Font secondtitle = FontFactory.GetFont("Times New Roman", 12, BaseColor.WHITE);
        static iTextSharp.text.Font secondtitledesc = FontFactory.GetFont("Times New Roman", 14, BaseColor.WHITE);
        static iTextSharp.text.Font third = FontFactory.GetFont("Times New Roman", 9, BaseColor.BLACK);
        static iTextSharp.text.Font fourthtitle = FontFactory.GetFont("Times New Roman", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        static iTextSharp.text.Font openbold = FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

        public static void ExportToPdf(string Info, List<clsPdfFullDataVariable> clsPdfFullDataVariableList, clsLang clslang, clsSystemSetting clsSystemSetting, string language)
        {
            //string filePath = LibDBHelper.CreatePdfFile("Amsonic", language,true);
            Document document = new Document();
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Append, FileAccess.Write));
            writer.PageEvent = new PDFTemplate();
            document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            Stream inputimg = new FileStream("CompanyLogo.PNG", FileMode.Open, FileAccess.Read, FileShare.Read);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputimg);
            image.Alignment = Image.ALIGN_MIDDLE;
            image.ScalePercent(55f);
            document.Add(image);
            //document.Add(new Paragraph(" "));

            #region Create first table for the pdf
            PdfPTable tablefirst = new PdfPTable(4);
            tablefirst.HorizontalAlignment = 2;
            tablefirst.TotalWidth = 500;    // actual width of table in points 
            tablefirst.LockedWidth = true;  // fix the width of the table to absolute
            tablefirst.SpacingBefore = 20f;
            // relative col widths in proportions 
            float[] width = new float[] { 1f, 2.5f, 1.5f, 2f };
            tablefirst.SetWidths(width);
            BaseColor Colour = new BaseColor(255, 255, 205);

            // First table title
            PdfPCell tb1cellt1 = new PdfPCell(new Phrase(clslang.Machine, firstbold))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = 20f,
                BackgroundColor = Colour
            };
            PdfPCell tb1cellt2 = new PdfPCell(new Phrase(clsSystemSetting.Machine, firstbold))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = 20f,
                BackgroundColor = Colour,
                Colspan = 3
            };

            // First table first row
            PdfPCell tb1cell11 = new PdfPCell(new Phrase(clslang.Name, first))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER,
                FixedHeight = 20f,
                BackgroundColor = Colour
            };
            PdfPCell tb1cell12 = new PdfPCell(new Phrase(clsSystemSetting.Name, first))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour
            };
            PdfPCell tb1cell13 = new PdfPCell(new Phrase(clslang.Page, first))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour,
                HorizontalAlignment = 2
            };
            PdfPCell tb1cell14 = new PdfPCell(new Phrase(writer.CurrentPageNumber.ToString() + " of " + writer.PageNumber.ToString(), first))
            {
                Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour,
                HorizontalAlignment = 2
            };

            // First table second row
            PdfPCell tb1cell21 = new PdfPCell(new Phrase(clslang.Software, first))
            {
                Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour
            };
            PdfPCell tb1cell22 = new PdfPCell(new Phrase(clsSystemSetting.Software, first))
            {
                Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour
            };
            PdfPCell tb1cell23 = new PdfPCell(new Phrase(clslang.Time, first))
            {
                Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour,
                HorizontalAlignment = 2
            };
            PdfPCell tb1cell24 = new PdfPCell(new Phrase(DateTime.Now.ToString(), first))
            {
                Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                FixedHeight = 15f,
                BackgroundColor = Colour,
                HorizontalAlignment = 2
            };

            tablefirst.AddCell(tb1cellt1);
            tablefirst.AddCell(tb1cellt2);
            tablefirst.AddCell(tb1cell11);
            tablefirst.AddCell(tb1cell12);
            tablefirst.AddCell(tb1cell13);
            tablefirst.AddCell(tb1cell14);
            tablefirst.AddCell(tb1cell21);
            tablefirst.AddCell(tb1cell22);
            tablefirst.AddCell(tb1cell23);
            tablefirst.AddCell(tb1cell24);
            document.Add(tablefirst);
            #endregion

                      
            foreach(var item in clsPdfFullDataVariableList)
            {
                document = MainTableGenerator(clslang, item, document);

                #region create third table for the pdf
                document.Add(new Paragraph(" "));
                PdfPTable tablethird = new PdfPTable(4);
                tablethird.HorizontalAlignment = 2;
                tablethird.TotalWidth = 500;    // actual width of table in points 
                tablethird.LockedWidth = true;  // fix the width of the table to absolute
                float[] sizefourth = new float[] { 2.5f, 1f, 2.5f, 1f };
                tablethird.SetWidths(sizefourth);

                // Second table title description
                PdfPCell tb3title = new PdfPCell(new Phrase(clslang.station.tProgramSequence, secondtitle))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE

                };
                PdfPCell tb3title1 = new PdfPCell(new Phrase(" ", secondtitle))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    VerticalAlignment = Element.ALIGN_MIDDLE

                };
                PdfPCell tb3title12 = new PdfPCell(new Phrase(clslang.station.NumberOfBasket, secondtitledesc))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                };
                PdfPCell tb3titledescrp = new PdfPCell(new Phrase(item.clsPdfMainVariable.BasketNumber, secondtitledesc))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    //HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Colspan = 4
                };


                tablethird.AddCell(tb3title);
                tablethird.AddCell(tb3title1);
                tablethird.AddCell(tb3title12);
                tablethird.AddCell(tb3titledescrp);
                document.Add(tablethird);
                #endregion

                document = DataTableColumnGenerator(clslang, document);
                document = DataTableGenerator(item, document);

                #region Barcode title
                document.Add(new Paragraph(" "));
                PdfPTable tablesixth = new PdfPTable(4);
                tablesixth.HorizontalAlignment = 2;
                tablesixth.TotalWidth = 500;    // actual width of table in points 
                tablesixth.LockedWidth = true;  // fix the width of the table to absolute
                float[] sizesixth = new float[] { 1.5f, 2.5f, 2.5f, 1f };
                tablesixth.SetWidths(sizesixth);

                // Second table title description
                PdfPCell tb6title = new PdfPCell(new Phrase(clslang.barcode.Orders, secondtitle))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 20f,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE

                };
                PdfPCell tb6title1 = new PdfPCell(new Phrase(" ", secondtitle))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 20f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Colspan = 3
                };

                tablesixth.AddCell(tb6title);
                tablesixth.AddCell(tb6title1);

                document.Add(tablesixth);
                #endregion

                document = BarcodeTableColumnGenerator(clslang, document);

                document = BarcodeTableGenerator(item, document);

                document.Add(new Paragraph(" "));
            }
                     
            document.Close();
            document.CloseDocument();
        }
        public static Document MainTableGenerator(clsLang clslang, clsPdfFullDataVariable clsPdfFullDataVariable, Document document)
        {
            try
            {
                #region Create second table for the pdf
                PdfPTable tablesecond = new PdfPTable(4);
                tablesecond.HorizontalAlignment = 2;
                tablesecond.SpacingBefore = 20f;
                tablesecond.TotalWidth = 500;    // actual width of table in points 
                tablesecond.LockedWidth = true;  // fix the width of the table to absolute
                float[] sizethird = new float[] { 20f, 15f, 20f, 15f };
                tablesecond.SetWidths(sizethird);

                // Second table title description
                PdfPCell tb2title = new PdfPCell(new Phrase(clslang.LoadNumber, secondtitle))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE

                };
                PdfPCell tb2titledescrp = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.LoadingNo, secondtitledesc))
                {
                    BackgroundColor = new BaseColor(100, 100, 100),
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER,
                    FixedHeight = 18f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Colspan = 3
                };

                #region second table 1st row
                PdfPCell tb2cell11 = new PdfPCell(new Phrase(clslang.TimeStart, second))
                {
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell12 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.TimeStart, second))
                {
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f

                };
                PdfPCell tb2cell13 = new PdfPCell(new Phrase(clslang.LoadingID, second))
                {
                    FixedHeight = 16f,
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER
                };
                PdfPCell tb2cell14 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.LoadingId, second))
                {
                    FixedHeight = 16f,
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER,
                    Colspan = 3
                };
                #endregion

                #region Second table second row
                PdfPCell tb2cell21 = new PdfPCell(new Phrase(clslang.TimeEnd, second))
                {
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell22 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.TimeEnd, second))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell23 = new PdfPCell(new Phrase(clslang.UnLoadingID, second))
                {
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell24 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.UnloadingId, second))
                {
                    FixedHeight = 16f,
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER,
                    Colspan = 3
                };
                #endregion

                #region Second table third row 
                PdfPCell tb2cell31 = new PdfPCell(new Phrase(clslang.RecipeNumber, second))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell32 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.RecipeNo, second))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell33 = new PdfPCell(new Phrase(clslang.Operator, second))
                {
                    FixedHeight = 16f,
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER
                };
                PdfPCell tb2cell34 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.Username, second))
                {
                    FixedHeight = 16f,
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    Colspan = 3
                };
                #endregion

                #region Second table fourth row 
                PdfPCell tb2cell41 = new PdfPCell(new Phrase(clslang.RecipeDesc, second))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell42 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.RecipeDescription, second))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell43 = new PdfPCell(new Phrase(clslang.BasketNumber, second))
                {
                    Border = iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    FixedHeight = 16f
                };
                PdfPCell tb2cell44 = new PdfPCell(new Phrase(clsPdfFullDataVariable.clsPdfMainVariable.LoadingNo, second))
                {
                    Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER,
                    Colspan = 3,
                    FixedHeight = 16f
                };
                #endregion

                // Second table append
                tablesecond.AddCell(tb2title);
                tablesecond.AddCell(tb2titledescrp);

                tablesecond.AddCell(tb2cell11);
                tablesecond.AddCell(tb2cell12);
                tablesecond.AddCell(tb2cell13);
                tablesecond.AddCell(tb2cell14);

                tablesecond.AddCell(tb2cell21);
                tablesecond.AddCell(tb2cell22);
                tablesecond.AddCell(tb2cell23);
                tablesecond.AddCell(tb2cell24);

                tablesecond.AddCell(tb2cell31);
                tablesecond.AddCell(tb2cell32);
                tablesecond.AddCell(tb2cell33);
                tablesecond.AddCell(tb2cell34);

                tablesecond.AddCell(tb2cell41);
                tablesecond.AddCell(tb2cell42);
                tablesecond.AddCell(tb2cell43);
                tablesecond.AddCell(tb2cell44);
                document.Add(tablesecond);

                #endregion
            }
            catch
            {
                throw;
            }

            return document;
        }
        public static Document DataTableColumnGenerator(clsLang clslang, Document document)
        {

            PdfPTable tablefourth = new PdfPTable(11);
            tablefourth.HorizontalAlignment = 2;
            //tablefourth.SpacingBefore = 20f;
            tablefourth.TotalWidth = 500;
            tablefourth.LockedWidth = true;
            float[] widths = new float[] { 10f, 10f, 54f, 10f, 11f, 10f, 13f, 11f, 11f, 12f, 11f };
            tablefourth.SetWidths(widths);

            iTextSharp.text.Font fourth = FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK);
            float data_height = 0;

            PdfPCell tb4cell11 = new PdfPCell(new Phrase(clslang.station.TimeIn, fourth));
            tb4cell11.HorizontalAlignment = 1;
            tb4cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell12 = new PdfPCell(new Phrase(clslang.station.TimeOut, fourth));
            tb4cell12.HorizontalAlignment = 1;
            tb4cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell13 = new PdfPCell(new Phrase(clslang.station.StationName, fourth));
            tb4cell13.HorizontalAlignment = 1;
            tb4cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell14 = new PdfPCell(new Phrase(clslang.station.Quality, fourth));
            tb4cell14.HorizontalAlignment = 1;
            tb4cell14.VerticalAlignment = Element.ALIGN_MIDDLE;

            PdfPCell tb4cell15 = new PdfPCell(new Phrase(clslang.station.ProgramSequence, fourth));
            tb4cell15.HorizontalAlignment = 1;
            tb4cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell16 = new PdfPCell(new Phrase(clslang.station.EffectiveTime, fourth));
            tb4cell16.HorizontalAlignment = 1;
            tb4cell16.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell17 = new PdfPCell(new Phrase(clslang.station.Temperature, fourth));
            tb4cell17.HorizontalAlignment = 1;
            tb4cell17.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell18 = new PdfPCell(new Phrase(clslang.station.USBottomA, fourth));
            tb4cell18.HorizontalAlignment = 1;
            tb4cell18.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell19 = new PdfPCell(new Phrase(clslang.station.USSideA, fourth));
            tb4cell19.HorizontalAlignment = 1;
            tb4cell19.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell110 = new PdfPCell(new Phrase(clslang.station.Conductivity, fourth));
            tb4cell110.HorizontalAlignment = 1;
            tb4cell110.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb4cell111 = new PdfPCell(new Phrase(clslang.station.Pressure, fourth));
            tb4cell111.HorizontalAlignment = 1;
            tb4cell111.VerticalAlignment = Element.ALIGN_MIDDLE;

            data_height = 35f;
            tb4cell11.FixedHeight = data_height;
            tb4cell12.FixedHeight = data_height;
            tb4cell13.FixedHeight = data_height;
            tb4cell14.FixedHeight = data_height;
            tb4cell15.FixedHeight = data_height;
            tb4cell16.FixedHeight = data_height;
            tb4cell17.FixedHeight = data_height;
            tb4cell18.FixedHeight = data_height;
            tb4cell19.FixedHeight = data_height;
            tb4cell110.FixedHeight = data_height;
            tb4cell111.FixedHeight = data_height;

            tablefourth.AddCell(tb4cell11);
            tablefourth.AddCell(tb4cell12);
            tablefourth.AddCell(tb4cell13);
            tablefourth.AddCell(tb4cell14);
            tablefourth.AddCell(tb4cell15);
            tablefourth.AddCell(tb4cell16);
            tablefourth.AddCell(tb4cell17);
            tablefourth.AddCell(tb4cell18);
            tablefourth.AddCell(tb4cell19);
            tablefourth.AddCell(tb4cell110);
            tablefourth.AddCell(tb4cell111);

            document.Add(tablefourth);

            return document;
        }
        public static Document DataTableGenerator(clsPdfFullDataVariable clsPdfFullDataVariable, Document document)
        {

            if (clsPdfFullDataVariable.clsPdfPlcVariables.Count > 0)
            {
                foreach (var item in clsPdfFullDataVariable.clsPdfPlcVariables)
                {
                    PdfPTable tablefifth = new PdfPTable(11);
                    tablefifth.HorizontalAlignment = 2;
                    tablefifth.TotalWidth = 500;
                    tablefifth.LockedWidth = true;
                    float[] widths = new float[] { 10f, 10f, 54f, 10f, 11f, 10f, 13f, 11f, 11f, 12f, 11f };
                    tablefifth.SetWidths(widths);

                    iTextSharp.text.Font fourth = FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK);
                    float data_height = 15f;


                    PdfPCell tb5cell1 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.TimeIn) ? "" : item.TimeIn, fourth));
                    tb5cell1.HorizontalAlignment = 0;
                    tb5cell1.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell1.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell1);

                    PdfPCell tb5cell2 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.TimeOut) ? "" : item.TimeOut, fourth));
                    tb5cell2.HorizontalAlignment = 0;
                    tb5cell2.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell2.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell2);

                    PdfPCell tb5cell3 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.StationNo) ? "" : item.StationNo, fourth));
                    tb5cell3.HorizontalAlignment = 0;
                    tb5cell3.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell3.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell3);

                    PdfPCell tb5cell4 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.Quality) ? "" : item.Quality, fourth));
                    tb5cell4.HorizontalAlignment = 0;
                    tb5cell4.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell4.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell4);

                    PdfPCell tb5cell5 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.SequenceRecipe) ? "" : item.SequenceRecipe, fourth));
                    tb5cell5.HorizontalAlignment = 0;
                    tb5cell5.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell5.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell5);

                    PdfPCell tb5cell6 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.EffectiveTime) ? "" : item.EffectiveTime, fourth));
                    tb5cell6.HorizontalAlignment = 0;
                    tb5cell6.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell6.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell6);

                    PdfPCell tb5cell7 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.TemperaturePV) ? "" : item.TemperaturePV, fourth));
                    tb5cell7.HorizontalAlignment = 0;
                    tb5cell7.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell7.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell7);

                    PdfPCell tb5cell8 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.UltrasonicBottomAPower) ? "" : item.UltrasonicBottomAPower, fourth));
                    tb5cell8.HorizontalAlignment = 0;
                    tb5cell8.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell8.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell8);

                    PdfPCell tb5cell9 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.UltrasonicSideAPower) ? "" : item.UltrasonicSideAPower, fourth));
                    tb5cell9.HorizontalAlignment = 0;
                    tb5cell9.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell9.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell9);

                    PdfPCell tb5cell10 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.ConductivityPV) ? "" : item.ConductivityPV, fourth));
                    tb5cell10.HorizontalAlignment = 0;
                    tb5cell10.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell10.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell10);

                    PdfPCell tb5cell11 = new PdfPCell(new Phrase(string.IsNullOrEmpty(item.PumpFlow) ? "" : item.PumpFlow, fourth));
                    tb5cell11.HorizontalAlignment = 0;
                    tb5cell11.VerticalAlignment = Element.ALIGN_LEFT;
                    tb5cell11.FixedHeight = data_height;
                    tablefifth.AddCell(tb5cell11);


                    document.Add(tablefifth);


                }
            }
            return document;
        }
        public static Document BarcodeTableColumnGenerator(clsLang clslang, Document document)
        {
            PdfPTable tablesixth = new PdfPTable(5);
            tablesixth.HorizontalAlignment = 2;
            //tablefourth.SpacingBefore = 20f;
            tablesixth.TotalWidth = 500;
            tablesixth.LockedWidth = true;
            iTextSharp.text.Font fourth = FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK);
            float data_height = 15f;
            float[] sizefourth = new float[] { 10f, 40f, 40f, 40f, 40f };
            tablesixth.SetWidths(sizefourth);

            PdfPCell tb6cell11 = new PdfPCell(new Phrase(clslang.barcode.No, fourth));
            tb6cell11.HorizontalAlignment = 1;
            tb6cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb6cell11.FixedHeight = data_height;
            PdfPCell tb6cell12 = new PdfPCell(new Phrase(clslang.barcode.PalletA, fourth));
            tb6cell12.HorizontalAlignment = 1;
            tb6cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb6cell12.FixedHeight = data_height;
            PdfPCell tb6cell13 = new PdfPCell(new Phrase(clslang.barcode.PalletB, fourth));
            tb6cell13.HorizontalAlignment = 1;
            tb6cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb6cell13.FixedHeight = data_height;
            PdfPCell tb6cell14 = new PdfPCell(new Phrase(clslang.barcode.PalletC, fourth));
            tb6cell14.HorizontalAlignment = 1;
            tb6cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb6cell14.FixedHeight = data_height;
            PdfPCell tb6cell15 = new PdfPCell(new Phrase(clslang.barcode.PalletD, fourth));
            tb6cell15.HorizontalAlignment = 1;
            tb6cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb6cell15.FixedHeight = data_height;

            tablesixth.AddCell(tb6cell11);
            tablesixth.AddCell(tb6cell12);
            tablesixth.AddCell(tb6cell13);
            tablesixth.AddCell(tb6cell14);
            tablesixth.AddCell(tb6cell15);

            document.Add(tablesixth);
            return document;
        }
        public static Document BarcodeTableGenerator(clsPdfFullDataVariable clsPdfFullDataVariable, Document document)
        {
            try
            {

                PdfPTable tableSeventh = new PdfPTable(5);
                tableSeventh.HorizontalAlignment = 2;
                //tablefourth.SpacingBefore = 20f;
                tableSeventh.TotalWidth = 500;
                tableSeventh.LockedWidth = true;
                iTextSharp.text.Font fourth = FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK);
                float data_height = 15f;
                float[] sizefourth = new float[] { 10f, 40f, 40f, 40f, 40f };
                tableSeventh.SetWidths(sizefourth);

                #region Numbering generator
                PdfPCell tb7num1 = new PdfPCell(new Phrase("1", fourth));
                tb7num1.HorizontalAlignment = 1;
                tb7num1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num1.FixedHeight = data_height;

                PdfPCell tb7num2 = new PdfPCell(new Phrase("2", fourth));
                tb7num2.HorizontalAlignment = 1;
                tb7num2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num2.FixedHeight = data_height;

                PdfPCell tb7num3 = new PdfPCell(new Phrase("3", fourth));
                tb7num3.HorizontalAlignment = 1;
                tb7num3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num3.FixedHeight = data_height;

                PdfPCell tb7num4 = new PdfPCell(new Phrase("4", fourth));
                tb7num4.HorizontalAlignment = 1;
                tb7num4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num4.FixedHeight = data_height;

                PdfPCell tb7num5 = new PdfPCell(new Phrase("5", fourth));
                tb7num5.HorizontalAlignment = 1;
                tb7num5.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num5.FixedHeight = data_height;

                PdfPCell tb7num6 = new PdfPCell(new Phrase("6", fourth));
                tb7num6.HorizontalAlignment = 1;
                tb7num6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num6.FixedHeight = data_height;

                PdfPCell tb7num7 = new PdfPCell(new Phrase("7", fourth));
                tb7num7.HorizontalAlignment = 1;
                tb7num7.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num7.FixedHeight = data_height;

                PdfPCell tb7num8 = new PdfPCell(new Phrase("8", fourth));
                tb7num8.HorizontalAlignment = 1;
                tb7num8.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7num8.FixedHeight = data_height;
                #endregion

                #region PalletA
                PdfPCell tb7CellA1 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO1) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO1, fourth));
                tb7CellA1.HorizontalAlignment = 0;
                tb7CellA1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA1.FixedHeight = data_height;

                PdfPCell tb7CellA2 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO2) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO2, fourth));
                tb7CellA2.HorizontalAlignment = 0;
                tb7CellA2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA2.FixedHeight = data_height;

                PdfPCell tb7CellA3 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO3) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO3, fourth));
                tb7CellA3.HorizontalAlignment = 0;
                tb7CellA3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA3.FixedHeight = data_height;

                PdfPCell tb7CellA4 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO4) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO4, fourth));
                tb7CellA4.HorizontalAlignment = 0;
                tb7CellA4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA4.FixedHeight = data_height;

                PdfPCell tb7CellA5 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO5) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO5, fourth));
                tb7CellA5.HorizontalAlignment = 0;
                tb7CellA5.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA5.FixedHeight = data_height;

                PdfPCell tb7CellA6 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO6) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO6, fourth));
                tb7CellA6.HorizontalAlignment = 0;
                tb7CellA6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA6.FixedHeight = data_height;

                PdfPCell tb7CellA7 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO7) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO7, fourth));
                tb7CellA7.HorizontalAlignment = 0;
                tb7CellA7.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA7.FixedHeight = data_height;

                PdfPCell tb7CellA8 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO8) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletA.PalletA_WO8, fourth));
                tb7CellA8.HorizontalAlignment = 0;
                tb7CellA8.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellA8.FixedHeight = data_height;
                #endregion

                #region Pallet B
                PdfPCell tb7CellB1 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO1) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO1, fourth));
                tb7CellB1.HorizontalAlignment = 0;
                tb7CellB1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB1.FixedHeight = data_height;

                PdfPCell tb7CellB2 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO2) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO2, fourth));
                tb7CellB2.HorizontalAlignment = 0;
                tb7CellB2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB2.FixedHeight = data_height;

                PdfPCell tb7CellB3 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO3) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO3, fourth));
                tb7CellB3.HorizontalAlignment = 0;
                tb7CellB3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB3.FixedHeight = data_height;

                PdfPCell tb7CellB4 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO4) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO4, fourth));
                tb7CellB4.HorizontalAlignment = 0;
                tb7CellB4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB4.FixedHeight = data_height;

                PdfPCell tb7CellB5 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO5) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO5, fourth));
                tb7CellB5.HorizontalAlignment = 0;
                tb7CellB5.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB5.FixedHeight = data_height;

                PdfPCell tb7CellB6 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO6) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO6, fourth));
                tb7CellB6.HorizontalAlignment = 0;
                tb7CellB6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB6.FixedHeight = data_height;

                PdfPCell tb7CellB7 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO7) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO7, fourth));
                tb7CellB7.HorizontalAlignment = 0;
                tb7CellB7.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB7.FixedHeight = data_height;

                PdfPCell tb7CellB8 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO8) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletB.PalletB_WO8, fourth));
                tb7CellB8.HorizontalAlignment = 0;
                tb7CellB8.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellB8.FixedHeight = data_height;
                #endregion

                #region Pallet C
                PdfPCell tb7CellC1 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO1) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO1, fourth));
                tb7CellC1.HorizontalAlignment = 0;
                tb7CellC1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC1.FixedHeight = data_height;

                PdfPCell tb7CellC2 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO2) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO2, fourth));
                tb7CellC2.HorizontalAlignment = 0;
                tb7CellC2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC2.FixedHeight = data_height;

                PdfPCell tb7CellC3 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO3) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO3, fourth));
                tb7CellC3.HorizontalAlignment = 0;
                tb7CellC3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC3.FixedHeight = data_height;

                PdfPCell tb7CellC4 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO4) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO4, fourth));
                tb7CellC4.HorizontalAlignment = 0;
                tb7CellC4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC4.FixedHeight = data_height;

                PdfPCell tb7CellC5 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO5) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO5, fourth));
                tb7CellC5.HorizontalAlignment = 0;
                tb7CellC5.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC5.FixedHeight = data_height;

                PdfPCell tb7CellC6 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO6) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO6, fourth));
                tb7CellC6.HorizontalAlignment = 0;
                tb7CellC6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC6.FixedHeight = data_height;

                PdfPCell tb7CellC7 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO7) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO7, fourth));
                tb7CellC7.HorizontalAlignment = 0;
                tb7CellC7.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC7.FixedHeight = data_height;

                PdfPCell tb7CellC8 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO8) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletC.PalletC_WO8, fourth));
                tb7CellC8.HorizontalAlignment = 0;
                tb7CellC8.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellC8.FixedHeight = data_height;
                #endregion

                #region Pallet C
                PdfPCell tb7CellD1 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO1) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO1, fourth));
                tb7CellD1.HorizontalAlignment = 0;
                tb7CellD1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD1.FixedHeight = data_height;

                PdfPCell tb7CellD2 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO2) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO2, fourth));
                tb7CellD2.HorizontalAlignment = 0;
                tb7CellD2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD2.FixedHeight = data_height;

                PdfPCell tb7CellD3 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO3) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO3, fourth));
                tb7CellD3.HorizontalAlignment = 0;
                tb7CellD3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD3.FixedHeight = data_height;

                PdfPCell tb7CellD4 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO4) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO4, fourth));
                tb7CellD4.HorizontalAlignment = 0;
                tb7CellD4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD4.FixedHeight = data_height;

                PdfPCell tb7CellD5 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO5) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO5, fourth));
                tb7CellD5.HorizontalAlignment = 0;
                tb7CellD5.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD5.FixedHeight = data_height;

                PdfPCell tb7CellD6 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO6) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO6, fourth));
                tb7CellD6.HorizontalAlignment = 0;
                tb7CellD6.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD6.FixedHeight = data_height;

                PdfPCell tb7CellD7 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO7) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO7, fourth));
                tb7CellD7.HorizontalAlignment = 0;
                tb7CellD7.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD7.FixedHeight = data_height;

                PdfPCell tb7CellD8 = new PdfPCell(new Phrase(string.IsNullOrEmpty(clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO8) ? "" : clsPdfFullDataVariable.clsPdfBarcodesPalletD.PalletD_WO8, fourth));
                tb7CellD8.HorizontalAlignment = 0;
                tb7CellD8.VerticalAlignment = Element.ALIGN_MIDDLE;
                tb7CellD8.FixedHeight = data_height;
                #endregion

                #region Assign all to table
                tableSeventh.AddCell(tb7num1);
                tableSeventh.AddCell(tb7CellA1);
                tableSeventh.AddCell(tb7CellB1);
                tableSeventh.AddCell(tb7CellC1);
                tableSeventh.AddCell(tb7CellD1);

                tableSeventh.AddCell(tb7num2);
                tableSeventh.AddCell(tb7CellA2);
                tableSeventh.AddCell(tb7CellB2);
                tableSeventh.AddCell(tb7CellC2);
                tableSeventh.AddCell(tb7CellD2);

                tableSeventh.AddCell(tb7num3);
                tableSeventh.AddCell(tb7CellA3);
                tableSeventh.AddCell(tb7CellB3);
                tableSeventh.AddCell(tb7CellC3);
                tableSeventh.AddCell(tb7CellD3);

                tableSeventh.AddCell(tb7num4);
                tableSeventh.AddCell(tb7CellA4);
                tableSeventh.AddCell(tb7CellB4);
                tableSeventh.AddCell(tb7CellC4);
                tableSeventh.AddCell(tb7CellD4);

                tableSeventh.AddCell(tb7num5);
                tableSeventh.AddCell(tb7CellA5);
                tableSeventh.AddCell(tb7CellB5);
                tableSeventh.AddCell(tb7CellC5);
                tableSeventh.AddCell(tb7CellD5);

                tableSeventh.AddCell(tb7num6);
                tableSeventh.AddCell(tb7CellA6);
                tableSeventh.AddCell(tb7CellB6);
                tableSeventh.AddCell(tb7CellC6);
                tableSeventh.AddCell(tb7CellD6);

                tableSeventh.AddCell(tb7num7);
                tableSeventh.AddCell(tb7CellA7);
                tableSeventh.AddCell(tb7CellB7);
                tableSeventh.AddCell(tb7CellC7);
                tableSeventh.AddCell(tb7CellD7);

                tableSeventh.AddCell(tb7num8);
                tableSeventh.AddCell(tb7CellA8);
                tableSeventh.AddCell(tb7CellB8);
                tableSeventh.AddCell(tb7CellC8);
                tableSeventh.AddCell(tb7CellD8);
                #endregion

                document.Add(tableSeventh);
                return document;
            }
            catch
            {
                throw;
            }
        }


        public class PDFTemplate : PdfPageEventHelper
        {
            PdfContentByte contentbyte;
            PdfTemplate pdffooter; BaseFont bf = null;
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    contentbyte = writer.DirectContent;
                    base.OnOpenDocument(writer, document);
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    pdffooter = contentbyte.CreateTemplate(30, 30);
                }
                catch
                {
                    throw;
                }
               
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                iTextSharp.text.Font headerfontbold = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15, BaseColor.BLACK);
                iTextSharp.text.Font headerfont = FontFactory.GetFont(BaseFont.HELVETICA, 14, BaseColor.BLACK);
                base.OnStartPage(writer, document);             
            }

            // Write the footer
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                string text = "Page " + writer.PageNumber + "/";
                base.OnEndPage(writer, document);
                contentbyte.BeginText();
                contentbyte.SetFontAndSize(bf, 10);
                contentbyte.SetTextMatrix(document.PageSize.GetRight(70), document.PageSize.GetBottom(20));
                contentbyte.ShowText(text);
                contentbyte.EndText();
                float len = bf.GetWidthPoint(text, 10);
                contentbyte.AddTemplate(pdffooter, document.PageSize.GetRight(70) + len, document.PageSize.GetBottom(20));
            }
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                pdffooter.BeginText();
                pdffooter.SetFontAndSize(bf, 10);
                pdffooter.SetTextMatrix(0, 0);
                pdffooter.ShowText((writer.PageNumber).ToString());
                pdffooter.EndText();
            }
        }
    }
}
