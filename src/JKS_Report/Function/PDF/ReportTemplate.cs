using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Diagnostics;
using System.Web.Configuration;

namespace JKS_Report.Function.PDF
{
    class ReportTemplate
    {
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SW_SHOWMINIMIZED = 2;

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        public static NotifyIcon tray = new NotifyIcon();
        public static ContextMenu menu = new ContextMenu();


        static string filepath = WebConfigurationManager.AppSettings.Get("AMSNetID");
        static string pdfpath = WebConfigurationManager.AppSettings.Get("AMSNetID");
        static string csvpath = WebConfigurationManager.AppSettings.Get("AMSNetID");
        private static void ensurefolder()
        {
            try
            {
                // Check is the directory do exists or not
                if (!Directory.Exists(filepath))
                { Directory.CreateDirectory(filepath); }

                if (!Directory.Exists(pdfpath))
                { Directory.CreateDirectory(pdfpath); }

                if (!Directory.Exists(csvpath))
                { Directory.CreateDirectory(csvpath); }
            }
            catch (Exception)
            { }
        }

        string getfile; char[] clearcsv; int filecharnumb; bool stopdetct;     
        string comp_file, daysofweek, dateandtime;
        string[] lines;
        string[][] data;
        int num_rows, total_data, increment = 0; int[] eachrow;
        DateTime date;
        private void csvread(FileInfo csvfileread)
        {
            comp_file = System.IO.File.ReadAllText(filepath + "/" + csvfileread);
            
            lines = comp_file.Split(new char[] { '\r' }, StringSplitOptions.None);

            // Check length of rows and columns
            num_rows = lines.Length;
            data = new string[num_rows][];
            eachrow = new int[num_rows];
            // MessageBox.Show(num_rows.ToString());

            // Spit each column into cells 
            for (int row = 0; row < num_rows; row++)
            {
                
                data[row] = lines[row].Split(new char[] { ',' }, StringSplitOptions.None);              
                eachrow[row] = data[row].Length;
                total_data = row;
                if (string.IsNullOrEmpty(data[row][0]) == true && row > 1)
                { break; }
            }

            // Get the value into class as global use 
            DataValue.THEDATA = data;
            DataValue.rowcontains = eachrow;
            DataValue.totalrow = total_data;
            // Get file's name to get the date value to input for report generate use
            string csvfilename = csvfileread.ToString();
            string[] filenamesplit = csvfilename.Split('_');
            filenamesplit = filenamesplit[1].Split('.'); // filenamesplit[0] = date of csv
            char[] numbofdate = filenamesplit[0].ToCharArray();
            int year = Convert.ToInt32(numbofdate[0].ToString() + numbofdate[1].ToString() + numbofdate[2].ToString() + numbofdate[3].ToString());
            int month = Convert.ToInt32(numbofdate[4].ToString() + numbofdate[5].ToString());
            int day = Convert.ToInt32(numbofdate[6].ToString() + numbofdate[7].ToString());
            date = new DateTime(year, month, day);

            daysofweek = date.ToString("dddd");
            dateandtime = daysofweek + ", " + day.ToString() + ". " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + " " + year.ToString();
            //        MessageBox.Show(dateandtime);
            // Move the file from one folder to another
            string filetomove = filepath + "/" + csvfileread;
            string filemove = csvpath + "/" + csvfileread;
            try
            {
                File.Move(filetomove, filemove);
            }
            catch
            {
                int duplicate_check = 0;
                string desreplace = csvpath + "/" + duplicate_check + "_" + csvfileread;
                while (File.Exists(desreplace))
                {
                    duplicate_check++;
                    desreplace = csvpath + "/" + duplicate_check + "_" + csvfileread;
                }
                File.Replace(filetomove, filemove, desreplace);
            }
        }

        string[] variables, bar_buffer, item_buffer, item_display; string[][] bargrp, item_data_include; int[] numb_words, item_length, item_yesno_check;
        int numb_data_include, disp_numb = 0;
        int totaldata = new int(); int count = 0;
        private void txtread()
        {
            item_buffer = new string[21];
            item_length = new int[21];
            item_display = new string[21];
            item_data_include = new string[21][];
            bar_buffer = new string[5];
            bargrp = new string[5][];
            numb_words = new int[5];
            item_yesno_check = new int[30];
            variables = System.IO.File.ReadAllLines(@"C:\JKS\variables.txt");
            totaldata = variables.Length;
            // Differentiate true or false from the data
            for (int bar = 4; bar <= 8; bar++)
            {
                bargrp[bar - 4] = variables[bar].Split(new char[] { ' ' }, StringSplitOptions.None);
                numb_words[bar - 4] = bargrp[bar - 4].Length;
            }
            // Rearrange back the data inside textbox 
            bar_buffer[0] = bargrp[0][0];
            for (int numbbarzero = 1; numbbarzero < numb_words[0] - 1; numbbarzero++)
            {
                bar_buffer[0] = bar_buffer[0] + " " + bargrp[0][numbbarzero];
            }
            bar_buffer[1] = bargrp[1][0];
            for (int numbbarone = 1; numbbarone < numb_words[1] - 1; numbbarone++)
            {
                bar_buffer[1] = bar_buffer[1] + " " + bargrp[1][numbbarone];
            }
            bar_buffer[2] = bargrp[2][0];
            for (int numbbartwo = 1; numbbartwo < numb_words[2] - 1; numbbartwo++)
            {
                bar_buffer[2] = bar_buffer[2] + " " + bargrp[2][numbbartwo];
            }
            bar_buffer[3] = bargrp[3][0];
            for (int numbbarthree = 1; numbbarthree < numb_words[3] - 1; numbbarthree++)
            {
                bar_buffer[3] = bar_buffer[3] + " " + bargrp[3][numbbarthree];
            }
            bar_buffer[4] = bargrp[4][0];
            for (int numbbarfour = 1; numbbarfour < numb_words[4] - 1; numbbarfour++)
            {
                bar_buffer[4] = bar_buffer[4] + " " + bargrp[4][numbbarfour];
            }
            disp_numb = 0;
            for (int numb_var = 9; numb_var < 30; numb_var++)
            {
                item_data_include[numb_var - 9] = variables[numb_var].Split(new char[] { ' ' }, StringSplitOptions.None);
                item_length[numb_var - 9] = item_data_include[numb_var - 9].Length;
                item_buffer[numb_var - 9] = item_data_include[numb_var - 9][0];
                item_display[numb_var - 9] = item_data_include[numb_var - 9][item_length[numb_var - 9] - 2];
                if (item_display[numb_var - 9] == "yes" && !((numb_var - 9 + 2) * 2 == 42)) // && disp_numb<6  (Remove limits)
                {
                    item_yesno_check[disp_numb] = (numb_var - 9 + 2) * 2;
                    if (item_yesno_check[disp_numb] == 8 || item_yesno_check[disp_numb] == 10 || item_yesno_check[disp_numb] == 12)
                    {
                        item_yesno_check[disp_numb] = item_yesno_check[disp_numb] - 1;
                    }
                    disp_numb++;
                }
                for (int item_combine = 1; item_combine < item_length[numb_var - 9] - 1; item_combine++)
                {
                    item_buffer[numb_var - 9] = item_buffer[numb_var - 9] + " " + item_data_include[numb_var - 9][item_combine];
                }
            }
        }

        string filename; Document doc; PdfWriter write; // string variable;
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
        PdfPTable tablefirst, tablesecond, tablethird, tablefourth, tablestation;

        private void rprtgen()
        {
            int i = 0;
            // Check the filename do exists if yes next name if not use it
            do
            {
                i = i + 1;
                filename = getfile + ".pdf";
            } while (File.Exists("C:\\Reports\\pdf" + "/" + filename));
            // PDF 
            doc = new Document(PageSize.A4, 70f, 30f, 30f, 30f);
            //        doc = new Document(PageSize.A4, 70f, 30f, 100f, 50f);
            write = PdfWriter.GetInstance(doc, new FileStream("C:\\Reports\\pdf" + "/" + filename, FileMode.Create));
            write.PageEvent = new PDFTemplate();
            // Open pdf file to start processing it
            doc.Open();

            //       Random rndm = new Random();
            //     int time = rndm.Next(5, 8);

            // Amsonic logo include
            Stream inputimg = new FileStream("Amsonic_logo.png", FileMode.Open, FileAccess.Read, FileShare.Read);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputimg);
            image.ScalePercent(75f);
            doc.Add(image);

            // Create first table for the pdf
            tablefirst = new PdfPTable(4);
            tablefirst.HorizontalAlignment = 2;
            tablefirst.TotalWidth = 500;    // actual width of table in points 
            tablefirst.LockedWidth = true;  // fix the width of the table to absolute
            tablefirst.SpacingBefore = 20f;
            // relative col widths in proportions 
            float[] width = new float[] { 1f, 2.5f, 1.5f, 2f };
            tablefirst.SetWidths(width);
            BaseColor Colour = new BaseColor(255, 255, 205);

            // First table title
            PdfPCell tb1cellt1 = new PdfPCell(new Phrase("Title:", firstbold));
            tb1cellt1.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
            tb1cellt1.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb1cellt1.FixedHeight = 20f;
            tb1cellt1.BackgroundColor = Colour;
            PdfPCell tb1cellt2 = new PdfPCell(new Phrase(variables[1], firstbold));
            tb1cellt2.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            tb1cellt2.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb1cellt2.FixedHeight = 20f;
            tb1cellt2.BackgroundColor = Colour;
            tb1cellt2.Colspan = 3;

            // First table first row
            PdfPCell tb1cell11 = new PdfPCell(new Phrase("Machine:", first));
            tb1cell11.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
            tb1cell11.FixedHeight = 15f;
            tb1cell11.BackgroundColor = Colour;
            PdfPCell tb1cell12 = new PdfPCell(new Phrase(variables[2], first));
            tb1cell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            tb1cell12.FixedHeight = 15f;
            tb1cell12.BackgroundColor = Colour;
            PdfPCell tb1cell13 = new PdfPCell(new Phrase("Page:", first));
            tb1cell13.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            tb1cell13.FixedHeight = 15f;
            tb1cell13.BackgroundColor = Colour;
            tb1cell13.HorizontalAlignment = 2;
            PdfPCell tb1cell14 = new PdfPCell(new Phrase(write.CurrentPageNumber.ToString() + " of " + write.PageNumber.ToString(), first));
            tb1cell14.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            tb1cell14.FixedHeight = 15f;
            tb1cell14.BackgroundColor = Colour;
            tb1cell14.HorizontalAlignment = 2;

            // First table second row
            PdfPCell tb1cell21 = new PdfPCell(new Phrase("Software:", first));
            tb1cell21.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb1cell21.FixedHeight = 15f;
            tb1cell21.BackgroundColor = Colour;
            PdfPCell tb1cell22 = new PdfPCell(new Phrase(variables[3], first));
            tb1cell22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb1cell22.FixedHeight = 15f;
            tb1cell22.BackgroundColor = Colour;
            PdfPCell tb1cell23 = new PdfPCell(new Phrase("Date:", first));
            tb1cell23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb1cell23.FixedHeight = 15f;
            tb1cell23.BackgroundColor = Colour;
            tb1cell23.HorizontalAlignment = 2;
            PdfPCell tb1cell24 = new PdfPCell(new Phrase(dateandtime, first));
            tb1cell24.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb1cell24.FixedHeight = 15f;
            tb1cell24.BackgroundColor = Colour;
            tb1cell24.HorizontalAlignment = 2;

            // First table third row
            /*        PdfPCell tb1cell31 = new PdfPCell(new Phrase("Software:", first));
                    tb1cell31.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    tb1cell31.FixedHeight = 15f;
                    tb1cell31.BackgroundColor = Colour;
                    PdfPCell tb1cell32 = new PdfPCell(new Phrase(data[1][3], first));
                    tb1cell32.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    tb1cell32.FixedHeight = 15f;
                    tb1cell32.BackgroundColor = Colour;
                    PdfPCell tb1cell33 = new PdfPCell(new Phrase("Time of Printing:", first));
                    tb1cell33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    tb1cell33.FixedHeight = 15f;
                    tb1cell33.BackgroundColor = Colour;
                    tb1cell33.HorizontalAlignment = 2;
                    PdfPCell tb1cell34 = new PdfPCell(new Phrase(data[2][9], first));
                    tb1cell34.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    tb1cell34.FixedHeight = 15f;
                    tb1cell34.BackgroundColor = Colour;
                    tb1cell34.HorizontalAlignment = 2; */

            // First table cell append
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
            //        tablefirst.AddCell(tb1cell31);
            //      tablefirst.AddCell(tb1cell32);
            //    tablefirst.AddCell(tb1cell33);
            //  tablefirst.AddCell(tb1cell34);
            doc.Add(tablefirst);

            // Create second table for the pdf
            tablesecond = new PdfPTable(6);
            tablesecond.HorizontalAlignment = 2;
            tablesecond.SpacingBefore = 10f;
            tablesecond.SpacingAfter = 15f;
            tablesecond.TotalWidth = 500;    // actual width of table in points 
            tablesecond.LockedWidth = true;  // fix the width of the table to absolute
            float[] sizethird = new float[] { 1.5f, 2.3f, 0.8f, 0.5f, 0.6f, 0.3f };
            tablesecond.SetWidths(sizethird);

            // Second table title description
            PdfPCell tb2title = new PdfPCell(new Phrase(" Load Number:", secondtitle));
            tb2title.BackgroundColor = new BaseColor(100, 100, 100);
            tb2title.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
            tb2title.FixedHeight = 22f;
            tb2title.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2title.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell tb2titledescrp = new PdfPCell(new Phrase(" " + data[1][6], secondtitledesc));
            tb2titledescrp.BackgroundColor = new BaseColor(100, 100, 100);
            tb2titledescrp.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
            tb2titledescrp.FixedHeight = 22f;
            tb2titledescrp.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2title.VerticalAlignment = Element.ALIGN_MIDDLE;
            tb2titledescrp.Colspan = 5;
            // Second table first row 
            PdfPCell tb2cell11 = new PdfPCell(new Phrase(" Date Creation:", second));
            tb2cell11.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
            tb2cell11.FixedHeight = 16f;
            // tb2cell11.Colspan = 2;
            PdfPCell tb2cell12 = new PdfPCell(new Phrase(dateandtime + " at " + data[4][0].Trim(), second));
            tb2cell12.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            tb2cell12.FixedHeight = 16f;
            //       tb2cell12.Colspan = 2;
            PdfPCell tb2cell13 = new PdfPCell(new Phrase(" Loading ID: ", second));
            tb2cell13.FixedHeight = 16f;
            tb2cell13.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            PdfPCell tb2cell14 = new PdfPCell(new Phrase(data[1][2], second));
            tb2cell14.FixedHeight = 16f;
            tb2cell14.Colspan = 3;
            tb2cell14.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            // Second table second row
            PdfPCell tb2cell21 = new PdfPCell(new Phrase(" Date Exit:", second));
            tb2cell21.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell21.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell21.FixedHeight = 16f;
            // tb2cell21.Colspan = 2;
            PdfPCell tb2cell22 = new PdfPCell(new Phrase(dateandtime + " at " + data[total_data - 1][0].Trim(), second));
            tb2cell22.HorizontalAlignment = 2;
            tb2cell22.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell22.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell22.FixedHeight = 16f;
            //        tb2cell22.Colspan = 2;
            PdfPCell tb2cell23 = new PdfPCell(new Phrase(" Unloading ID: ", second));
            tb2cell23.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
            tb2cell23.FixedHeight = 16f;
            PdfPCell tb2cell24 = new PdfPCell(new Phrase(data[1][3], second));
            tb2cell24.FixedHeight = 16f;
            tb2cell24.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            tb2cell24.Colspan = 3;
            // Second table third row 
            PdfPCell tb2cell31 = new PdfPCell(new Phrase(" Product Recipe Number: ", second));
            tb2cell31.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell31.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
            //       tb2cell31.Colspan = 1;
            tb2cell31.FixedHeight = 16f;
            PdfPCell tb2cell32 = new PdfPCell(new Phrase(data[1][4], second));
            tb2cell32.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell32.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            tb2cell32.FixedHeight = 16f;
            PdfPCell tb2cell33 = new PdfPCell(new Phrase(" Operator: ", second));
            tb2cell33.FixedHeight = 16f;
            tb2cell33.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            PdfPCell tb2cell34 = new PdfPCell(new Phrase(data[1][1], second));
            tb2cell34.FixedHeight = 16f;
            tb2cell34.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell34.Colspan = 3;
            // Second table fourth row 
            PdfPCell tb2cell41 = new PdfPCell(new Phrase(" Product Recipe Description: ", second));
            tb2cell41.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell41.Colspan = 1;
            tb2cell41.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell41.FixedHeight = 16f;
            PdfPCell tb2cell42 = new PdfPCell(new Phrase(data[1][5], second));
            tb2cell42.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell42.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell42.Colspan = 5;
            tb2cell42.FixedHeight = 16f;
            // Second table fifth row
            PdfPCell tb2cell51 = new PdfPCell(new Phrase(" Bar Code Data 1: ", second));
            tb2cell51.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell51.Colspan = 1;
            tb2cell51.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell51.FixedHeight = 16f;
            PdfPCell tb2cell52 = new PdfPCell(new Phrase(bar_buffer[0], second));
            tb2cell52.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell52.Colspan = 5;
            tb2cell52.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell52.FixedHeight = 16f;
            // Second table sixth row
            PdfPCell tb2cell61 = new PdfPCell(new Phrase(" Bar Code Data 2: ", second));
            tb2cell61.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell61.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell61.Colspan = 1;
            tb2cell61.FixedHeight = 16f;
            PdfPCell tb2cell62 = new PdfPCell(new Phrase(bar_buffer[1], second));
            tb2cell62.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell62.Colspan = 5;
            tb2cell62.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell62.FixedHeight = 16f;
            // Second table seventh row
            PdfPCell tb2cell71 = new PdfPCell(new Phrase(" Bar Code Data 3: ", second));
            tb2cell71.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell71.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell71.Colspan = 1;
            tb2cell71.FixedHeight = 16f;
            PdfPCell tb2cell72 = new PdfPCell(new Phrase(bar_buffer[2], second));
            tb2cell72.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell72.Colspan = 5;
            tb2cell72.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell72.FixedHeight = 16f;
            // Second table eighth row
            PdfPCell tb2cell81 = new PdfPCell(new Phrase(" Bar Code Data 4: ", second));
            tb2cell81.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell81.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell81.Colspan = 1;
            tb2cell81.FixedHeight = 16f;
            PdfPCell tb2cell82 = new PdfPCell(new Phrase(bar_buffer[3], second));
            tb2cell82.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell82.Colspan = 5;
            tb2cell82.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell82.FixedHeight = 16f;
            // Second table ninth row
            PdfPCell tb2cell91 = new PdfPCell(new Phrase(" Bar Code Data 5: ", second));
            tb2cell91.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell91.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell91.Colspan = 1;
            tb2cell91.FixedHeight = 16f;
            PdfPCell tb2cell92 = new PdfPCell(new Phrase(bar_buffer[4], second));
            tb2cell92.HorizontalAlignment = Element.ALIGN_LEFT;
            tb2cell92.Colspan = 5;
            tb2cell92.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tb2cell92.FixedHeight = 16f;

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
            // Barcode group of choice selection to display or not
            try
            {
                if (bargrp[0][numb_words[0] - 1].Trim() == "true")
                {
                    tablesecond.AddCell(tb2cell51);
                    tablesecond.AddCell(tb2cell52);
                }
                if (bargrp[1][numb_words[1] - 1].Trim() == "true")
                {
                    tablesecond.AddCell(tb2cell61);
                    tablesecond.AddCell(tb2cell62);
                }
                if (bargrp[2][numb_words[2] - 1].Trim() == "true")
                {
                    tablesecond.AddCell(tb2cell71);
                    tablesecond.AddCell(tb2cell72);
                }
                if (bargrp[3][numb_words[3] - 1].Trim() == "true")
                {
                    tablesecond.AddCell(tb2cell81);
                    tablesecond.AddCell(tb2cell82);
                }
                if (bargrp[4][numb_words[4] - 1].Trim() == "true")
                {
                    tablesecond.AddCell(tb2cell91);
                    tablesecond.AddCell(tb2cell92);
                }
            }
            catch { }   // Fail silencly for null exception

            doc.Add(tablesecond);

            // Third table opening;
            //        tableopening(1);
            // Third table data;
            //        tableloadmonitor();

            // Fourth table opening;
            tableopening(2);
            // Fourth table data;
            tableloadprog();

            // Close pdf file deployed
            doc.Close();
        }

        /*   private void tableloadmonitor()
           {
               tablethird = new PdfPTable(8);
               tablethird.HorizontalAlignment = Element.ALIGN_RIGHT;
               tablethird.SpacingAfter = 15f;
               tablethird.TotalWidth = 500;    // actual width of table in points 
               tablethird.LockedWidth = true;  // fix the width of the table to absolute
               float[] sizethird = new float[] { 1f, 1.5f, 0.8f, 0.8f, 0.9f, 1f, 1f, 1f};
               tablethird.SetWidths(sizethird);

               PdfPCell tb3cell11 = new PdfPCell(new Phrase("Time of\nExit", third));
               tb3cell11.HorizontalAlignment = 1;
               tb3cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell11.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell11.FixedHeight = 26f;
               PdfPCell tb3cell12 = new PdfPCell(new Phrase("Position", third));
               tb3cell12.HorizontalAlignment = 0;
               tb3cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell12.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell12.FixedHeight = 26f;
               PdfPCell tb3cell13 = new PdfPCell(new Phrase("Tmin\n(sec)", third));
               tb3cell13.HorizontalAlignment = 1;
               tb3cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell13.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell13.FixedHeight = 26f;
               PdfPCell tb3cell14 = new PdfPCell(new Phrase("Tmin\n(eff)", third));
               tb3cell14.HorizontalAlignment = 1;
               tb3cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell14.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell14.FixedHeight = 26f;
               PdfPCell tb3cell15 = new PdfPCell(new Phrase("Tem. Bath\n(°C)", third));
               tb3cell15.HorizontalAlignment = 1;
               tb3cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell15.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell15.FixedHeight = 26f;
               PdfPCell tb3cell16 = new PdfPCell(new Phrase("Tem. Bath\nBuffer (°C)", third));
               tb3cell16.HorizontalAlignment = 1;
               tb3cell16.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell16.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell16.FixedHeight = 26f;
               PdfPCell tb3cell17 = new PdfPCell(new Phrase("Conductivity\n(µS/mS)", third));
               tb3cell17.HorizontalAlignment = 1;
               tb3cell17.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell17.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell17.FixedHeight = 26f;
               PdfPCell tb3cell18 = new PdfPCell(new Phrase("Goods\nQuality", third));
               tb3cell18.HorizontalAlignment = 1;
               tb3cell18.VerticalAlignment = Element.ALIGN_MIDDLE;
               tb3cell18.BackgroundColor = BaseColor.LIGHT_GRAY;
               tb3cell18.FixedHeight = 26f;

               tablethird.AddCell(tb3cell11);
               tablethird.AddCell(tb3cell12);
               tablethird.AddCell(tb3cell13);
               tablethird.AddCell(tb3cell14);
               tablethird.AddCell(tb3cell15);
               tablethird.AddCell(tb3cell16);
               tablethird.AddCell(tb3cell17);
               tablethird.AddCell(tb3cell18);

               for (int numb_data = 4; numb_data < total_data - 1; numb_data++)
               {
                   decimal tempvalue = decimal.Parse(data[numb_data][4], CultureInfo.InvariantCulture);
                   decimal tempvaluerounded = decimal.Round(tempvalue, 2, MidpointRounding.AwayFromZero);
                   PdfPCell tb3cellu1 = new PdfPCell(new Phrase(data[numb_data][1], value));
                   tb3cellu1.HorizontalAlignment = 1;
                   PdfPCell tb3cellu2 = new PdfPCell(new Phrase(" " + data[numb_data][2], value));
                   tb3cellu2.HorizontalAlignment = 0;
                   PdfPCell tb3cellu3 = new PdfPCell(new Phrase(tempvaluerounded.ToString("0.000"), value));
                   tb3cellu3.HorizontalAlignment = 1;
                   PdfPCell tb3cellu4 = new PdfPCell(new Phrase(data[numb_data][5], value));
                   tb3cellu4.HorizontalAlignment = 1;
                   PdfPCell tb3cellu5 = new PdfPCell(new Phrase(data[numb_data][6], value));
                   tb3cellu5.HorizontalAlignment = 2;
                   PdfPCell tb3cellu6 = new PdfPCell(new Phrase(data[numb_data][7], value));
                   tb3cellu6.HorizontalAlignment = 2;
                   PdfPCell tb3cellu7 = new PdfPCell(new Phrase(data[numb_data][8], value));
                   tb3cellu7.HorizontalAlignment = 2;
                   PdfPCell tb3cellu8 = new PdfPCell(new Phrase(data[numb_data][3], value));
                   tb3cellu8.HorizontalAlignment = 1;

                   tablethird.AddCell(tb3cellu1);
                   tablethird.AddCell(tb3cellu2);
                   tablethird.AddCell(tb3cellu3);
                   tablethird.AddCell(tb3cellu4);
                   tablethird.AddCell(tb3cellu5);
                   tablethird.AddCell(tb3cellu6);
                   tablethird.AddCell(tb3cellu7);
                   tablethird.AddCell(tb3cellu8);
               }

               doc.Add(tablethird);
           } */

        int eachstn = 0;
        private void tableloadprog()
        {
            tablefourth = new PdfPTable(3 + disp_numb);
            tablefourth.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablefourth.TotalWidth = 500;    // actual width of table in points 
            tablefourth.LockedWidth = true;  // fix the width of the table to absolute
            tablefourth.SpacingAfter = 15f;
            // Update to auto arrange the percentage rather than fix column width size
            float[] sizefourth = new float[3 + disp_numb];
            /*        if (disp_numb == 0) {
                        sizefourth = new float[] { 1f, 3f, 1f };    
                    }
                    else { */
            /// When disp_numb = 0, the calculation will stil 3f
            /// Equation set => Second Column Width Value = 1.7(2^-0.1x)+1.3; 
            /// There the data is sets along the exponential graph between 3 and 1.3 with a steep decrease based on the -0.25*disp_numb
            float sec_width = ((float)1.7 * (float)Math.Pow(2.3, -0.05 * disp_numb)) + (float)1.3; // 3 - ((float)0.1 * disp_numb);
            for (int col_disp_width = 0; col_disp_width < (3 + disp_numb); col_disp_width++)
            {
                if (col_disp_width == 1)
                {
                    sizefourth[col_disp_width] = sec_width;
                }
                else
                {
                    sizefourth[col_disp_width] = 1f;
                }
            }
            tablefourth.SetWidths(sizefourth);
            // Set font size after setting column width
            float fontsize;
            fontsize = 7 - (float)(disp_numb * 0.15);
            iTextSharp.text.Font fourth = FontFactory.GetFont("Times New Roman", fontsize, BaseColor.BLACK);

            // Looping the data for display after appending it 
            decimal bufer_integer_after_detect = 0; float data_height = 0;
            for (eachstn = 3; eachstn < DataValue.totalrow; ++eachstn)
            {
                PdfPCell tb4cell11 = new PdfPCell(new Phrase(data[eachstn][0].Trim(), fourth));
                tb4cell11.HorizontalAlignment = 1;
                tb4cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb4cell12 = new PdfPCell(new Phrase(data[eachstn][2].Trim(), fourth));
                if (eachstn == 3)
                { tb4cell12.HorizontalAlignment = 1; }
                else { tb4cell12.HorizontalAlignment = 0; }
                tb4cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb4cell13 = new PdfPCell(new Phrase(data[eachstn][42].Trim(), fourth));
                tb4cell13.HorizontalAlignment = 1;
                tb4cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                if (eachstn == 3) { data_height = 35f; }
                else { data_height = 15f; }
                tb4cell11.FixedHeight = data_height;
                tb4cell12.FixedHeight = data_height;
                tb4cell13.FixedHeight = data_height;
                tablefourth.AddCell(tb4cell11);
                tablefourth.AddCell(tb4cell12);
                tablefourth.AddCell(tb4cell13);
                if (eachstn == 3)
                {
                    // Update from fixed number variable declaration to flexible number variable declaration
                    PdfPCell[] tb4cell_Cont = new PdfPCell[disp_numb];
                    for (int dataloop = 0; dataloop < disp_numb; dataloop++)
                    {
                        if (data[eachstn - 1].Length > item_yesno_check[dataloop])
                        {
                            tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[dataloop]] + "\n(" + data[eachstn - 1][item_yesno_check[dataloop]].Trim() + ")", fourth));
                            tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                            tb4cell_Cont[dataloop].VerticalAlignment = Element.ALIGN_MIDDLE;
                            tb4cell_Cont[dataloop].FixedHeight = 35f;
                            tablefourth.AddCell(tb4cell_Cont[dataloop]);
                        }
                        else
                        {
                            tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[dataloop]] + "\n(N/A)", fourth));
                            tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                            tb4cell_Cont[dataloop].VerticalAlignment = Element.ALIGN_MIDDLE;
                            tb4cell_Cont[dataloop].FixedHeight = 35f;
                            tablefourth.AddCell(tb4cell_Cont[dataloop]);
                        }
                    }
                }
                else
                {
                    PdfPCell[] tb4cell_Cont = new PdfPCell[disp_numb];
                    for (int dataloop = 0; dataloop < disp_numb; dataloop++)
                    {
                        bool isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[dataloop]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                        tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[dataloop]].Trim(), fourth));
                        tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                        tb4cell_Cont[dataloop].FixedHeight = 15f;
                        if (isNumeric)
                        {
                            if (bufer_integer_after_detect == 0)
                            {
                                tb4cell_Cont[dataloop] = new PdfPCell(new Phrase("-", fourth));
                                tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                                tb4cell_Cont[dataloop].FixedHeight = 15f;
                            }
                            else
                            {
                                tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                                tb4cell_Cont[dataloop].FixedHeight = 15f;
                            }
                        }
                        if (item_yesno_check[dataloop] == 7 || item_yesno_check[dataloop] == 9 || item_yesno_check[dataloop] == 11)
                        {
                            if (data[eachstn][item_yesno_check[dataloop]].Contains("N/A") && data[eachstn][(item_yesno_check[dataloop]) + 1].Contains("N/A"))
                            {
                                tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(" ", fourth));
                                tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                                tb4cell_Cont[dataloop].FixedHeight = 15f;
                            }
                        }
                        else
                        {
                            if (data[eachstn][(item_yesno_check[dataloop]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[dataloop]].Contains("N/A"))
                            {
                                tb4cell_Cont[dataloop] = new PdfPCell(new Phrase(" ", fourth));
                                tb4cell_Cont[dataloop].HorizontalAlignment = 1;
                                tb4cell_Cont[dataloop].FixedHeight = 15f;
                            }
                        }
                        tablefourth.AddCell(tb4cell_Cont[dataloop]);
                    }
                    /*                if (disp_numb >= 1)
                                    {
                                        bool isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[0]],NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                        PdfPCell tb4cellu4 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[0]].Trim(), fourth));
                                        tb4cellu4.HorizontalAlignment = 1;
                                        tb4cellu4.FixedHeight = 15f;
                                        if (isNumeric) {
                                            if (bufer_integer_after_detect == 0)
                                            {
                                                tb4cellu4 = new PdfPCell(new Phrase("-", fourth));
                                                tb4cellu4.HorizontalAlignment = 1;
                                                tb4cellu4.FixedHeight = 15f;
                                            }
                                            else {
                                                tb4cellu4 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                tb4cellu4.HorizontalAlignment = 1;
                                                tb4cellu4.FixedHeight = 15f;
                                            }
                                        }
                                        if (item_yesno_check[0] == 7 || item_yesno_check[0] == 9 || item_yesno_check[0] == 11)
                                        {
                                            if (data[eachstn][item_yesno_check[0]].Contains("N/A") && data[eachstn][(item_yesno_check[0]) + 1].Contains("N/A"))
                                            {
                                                tb4cellu4 = new PdfPCell(new Phrase(" ", fourth));
                                                tb4cellu4.HorizontalAlignment = 1;
                                                tb4cellu4.FixedHeight = 15f;
                                            }
                                        }
                                        else
                                        {
                                            if (data[eachstn][(item_yesno_check[0]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[0]].Contains("N/A"))
                                            {
                                                tb4cellu4 = new PdfPCell(new Phrase(" ", fourth));
                                                tb4cellu4.HorizontalAlignment = 1;
                                                tb4cellu4.FixedHeight = 15f;
                                            }
                                        }
                                        tablefourth.AddCell(tb4cellu4);
                                        if (disp_numb >= 2)
                                        {
                                            isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[1]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                            PdfPCell tb4cellu5 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[1]].Trim(), fourth));
                                            tb4cellu5.HorizontalAlignment = 1;
                                            tb4cellu5.FixedHeight = 15f;
                                            if (isNumeric)
                                            {
                                                if (bufer_integer_after_detect == 0)
                                                {
                                                    tb4cellu5 = new PdfPCell(new Phrase("-", fourth));
                                                    tb4cellu5.HorizontalAlignment = 1;
                                                    tb4cellu5.FixedHeight = 15f;
                                                }
                                                else
                                                {
                                                    tb4cellu5 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                    tb4cellu5.HorizontalAlignment = 1;
                                                    tb4cellu5.FixedHeight = 15f;
                                                }
                                            }
                                            if (item_yesno_check[1] == 7 || item_yesno_check[1] == 9 || item_yesno_check[1] == 11)
                                            {
                                                if (data[eachstn][item_yesno_check[1]].Contains("N/A") && data[eachstn][(item_yesno_check[1]) + 1].Contains("N/A"))
                                                {
                                                    tb4cellu5 = new PdfPCell(new Phrase(" ", fourth));
                                                    tb4cellu5.HorizontalAlignment = 1;
                                                    tb4cellu5.FixedHeight = 15f;
                                                }
                                            }
                                            else
                                            {
                                                if (data[eachstn][(item_yesno_check[1]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[1]].Contains("N/A"))
                                                {
                                                    tb4cellu5 = new PdfPCell(new Phrase(" ", fourth));
                                                    tb4cellu5.HorizontalAlignment = 1;
                                                    tb4cellu5.FixedHeight = 15f;
                                                }
                                            }
                                            tablefourth.AddCell(tb4cellu5);
                                            if (disp_numb >= 3)
                                            {
                                                isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[2]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                                PdfPCell tb4cellu6 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[2]].Trim(), fourth));
                                                tb4cellu6.HorizontalAlignment = 1;
                                                tb4cellu6.FixedHeight = 15f;
                                                if (isNumeric)
                                                {
                                                    if (bufer_integer_after_detect == 0)
                                                    {
                                                        tb4cellu6 = new PdfPCell(new Phrase("-", fourth));
                                                        tb4cellu6.HorizontalAlignment = 1;
                                                        tb4cellu6.FixedHeight = 15f;
                                                    }
                                                    else
                                                    {
                                                        tb4cellu6 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                        tb4cellu6.HorizontalAlignment = 1;
                                                        tb4cellu6.FixedHeight = 15f;
                                                    }
                                                }
                                                if (item_yesno_check[2] == 7 || item_yesno_check[2] == 9 || item_yesno_check[2] == 11)
                                                {
                                                    if (data[eachstn][item_yesno_check[2]].Contains("N/A") && data[eachstn][(item_yesno_check[2]) + 1].Contains("N/A"))
                                                    {
                                                        tb4cellu6 = new PdfPCell(new Phrase(" ", fourth));
                                                        tb4cellu6.HorizontalAlignment = 1;
                                                        tb4cellu6.FixedHeight = 15f;
                                                    }
                                                }
                                                else
                                                {
                                                    if (data[eachstn][(item_yesno_check[2]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[2]].Contains("N/A"))
                                                    {
                                                        tb4cellu6 = new PdfPCell(new Phrase(" ", fourth));
                                                        tb4cellu6.HorizontalAlignment = 1;
                                                        tb4cellu6.FixedHeight = 15f;
                                                    }
                                                }
                                                tablefourth.AddCell(tb4cellu6);
                                                if (disp_numb >= 4)
                                                {
                                                    isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[3]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                                    PdfPCell tb4cellu7 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[3]].Trim(), fourth));
                                                    tb4cellu7.HorizontalAlignment = 1;
                                                    tb4cellu7.FixedHeight = 15f;
                                                    if (isNumeric)
                                                    {
                                                        if (bufer_integer_after_detect == 0)
                                                        {
                                                            tb4cellu7 = new PdfPCell(new Phrase("-", fourth));
                                                            tb4cellu7.HorizontalAlignment = 1;
                                                            tb4cellu7.FixedHeight = 15f;
                                                        }
                                                        else
                                                        {
                                                            tb4cellu7 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                            tb4cellu7.HorizontalAlignment = 1;
                                                            tb4cellu7.FixedHeight = 15f;
                                                        }
                                                    }
                                                    if (item_yesno_check[3] == 7 || item_yesno_check[3] == 9 || item_yesno_check[3] == 11)
                                                    {
                                                        if (data[eachstn][item_yesno_check[3]].Contains("N/A") && data[eachstn][(item_yesno_check[3]) + 1].Contains("N/A"))
                                                        {
                                                            tb4cellu7 = new PdfPCell(new Phrase(" ", fourth));
                                                            tb4cellu7.HorizontalAlignment = 1;
                                                            tb4cellu7.FixedHeight = 15f;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (data[eachstn][(item_yesno_check[3]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[3]].Contains("N/A"))
                                                        {
                                                            tb4cellu7 = new PdfPCell(new Phrase(" ", fourth));
                                                            tb4cellu7.HorizontalAlignment = 1;
                                                            tb4cellu7.FixedHeight = 15f;
                                                        }
                                                    }
                                                    tablefourth.AddCell(tb4cellu7);
                                                    if (disp_numb >= 5)
                                                    {
                                                        isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[4]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                                        PdfPCell tb4cellu8 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[4]], fourth));
                                                        tb4cellu8.HorizontalAlignment = 1;
                                                        tb4cellu8.FixedHeight = 15f;
                                                        if (isNumeric)
                                                        {
                                                            if (bufer_integer_after_detect == 0)
                                                            {
                                                                tb4cellu8 = new PdfPCell(new Phrase("-", fourth));
                                                                tb4cellu8.HorizontalAlignment = 1;
                                                                tb4cellu8.FixedHeight = 15f;
                                                            }
                                                            else
                                                            {
                                                                tb4cellu8 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                                tb4cellu8.HorizontalAlignment = 1;
                                                                tb4cellu8.FixedHeight = 15f;
                                                            }
                                                        }
                                                        if (item_yesno_check[4] == 7 || item_yesno_check[4] == 9 || item_yesno_check[4] == 11)
                                                        {
                                                            if (data[eachstn][item_yesno_check[4]].Contains("N/A") && data[eachstn][(item_yesno_check[4]) + 1].Contains("N/A"))
                                                            {
                                                                tb4cellu8 = new PdfPCell(new Phrase(" ", fourth));
                                                                tb4cellu8.HorizontalAlignment = 1;
                                                                tb4cellu8.FixedHeight = 15f;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (data[eachstn][(item_yesno_check[4]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[4]].Contains("N/A"))
                                                            {
                                                                tb4cellu8 = new PdfPCell(new Phrase(" ", fourth));
                                                                tb4cellu8.HorizontalAlignment = 1;
                                                                tb4cellu8.FixedHeight = 15f;
                                                            }
                                                        }
                                                        tablefourth.AddCell(tb4cellu8);
                                                        if (disp_numb == 6)
                                                        {
                                                            isNumeric = decimal.TryParse(data[eachstn][item_yesno_check[5]], NumberStyles.Float, CultureInfo.InvariantCulture, out bufer_integer_after_detect);
                                                            PdfPCell tb4cellu9 = new PdfPCell(new Phrase(data[eachstn][item_yesno_check[5]], fourth));
                                                            tb4cellu9.HorizontalAlignment = 1;
                                                            tb4cellu9.FixedHeight = 15f;
                                                            if (isNumeric)
                                                            {
                                                                if (bufer_integer_after_detect == 0)
                                                                {
                                                                    tb4cellu9 = new PdfPCell(new Phrase("-", fourth));
                                                                    tb4cellu9.HorizontalAlignment = 1;
                                                                    tb4cellu9.FixedHeight = 15f;
                                                                }
                                                                else
                                                                {
                                                                    tb4cellu9 = new PdfPCell(new Phrase(bufer_integer_after_detect.ToString("0.00"), fourth));
                                                                    tb4cellu9.HorizontalAlignment = 1;
                                                                    tb4cellu9.FixedHeight = 15f;
                                                                }
                                                            }
                                                            if (item_yesno_check[5] == 7 || item_yesno_check[5] == 9 || item_yesno_check[5] == 11)
                                                            {
                                                                if (data[eachstn][item_yesno_check[5]].Contains("N/A") && data[eachstn][(item_yesno_check[5]) + 1].Contains("N/A"))
                                                                {
                                                                    tb4cellu9 = new PdfPCell(new Phrase(" ", fourth));
                                                                    tb4cellu9.HorizontalAlignment = 1;
                                                                    tb4cellu9.FixedHeight = 15f;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (data[eachstn][(item_yesno_check[5]) - 1].Contains("N/A") && data[eachstn][item_yesno_check[5]].Contains("N/A"))
                                                                {
                                                                    tb4cellu9 = new PdfPCell(new Phrase(" ", fourth));
                                                                    tb4cellu9.HorizontalAlignment = 1;
                                                                    tb4cellu9.FixedHeight = 15f;
                                                                }
                                                            }
                                                            tablefourth.AddCell(tb4cellu9);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    { } */
                }
            }
            doc.Add(tablefourth);
        }

        /*    private void tabletitle(int which)
            {
                if (which == 1)
                { variable = "List of Parameters"; }
                else if (which == 2)
                { variable = "List of Ultrasonic Power"; }
                else if (which == 3)
                { variable = "List of Ultrasonic Frequency"; }
                else
                { variable = " "; }

                // Create title table for the pdf
                PdfPTable titletable = new PdfPTable(3);
                titletable.HorizontalAlignment = Element.ALIGN_MIDDLE;
                titletable.TotalWidth = 500;    // actual width of table in points 
                titletable.LockedWidth = true;  // fix the width of the table to absolute
                float[] size = new float[] { 0.3f, 1.4f, 0.8f };
                titletable.SetWidths(size);

                // Title table first row
                PdfPCell tbtcell1 = new PdfPCell(new Phrase("Barcode:", second));
                tbtcell1.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                tbtcell1.BackgroundColor = BaseColor.DARK_GRAY;
                PdfPCell tbtcell2 = new PdfPCell(new Phrase(data[0][0], twice));
                tbtcell2.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                tbtcell2.BackgroundColor = BaseColor.DARK_GRAY;
                PdfPCell tbtcell3 = new PdfPCell(new Phrase(variable, second));
                tbtcell3.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                tbtcell3.HorizontalAlignment = 2;
                tbtcell3.BackgroundColor = BaseColor.DARK_GRAY;

                // Title table append
                titletable.AddCell(tbtcell1);
                titletable.AddCell(tbtcell2);
                titletable.AddCell(tbtcell3);
                doc.Add(titletable);
            }

            private void tableone()
            {
                PdfPCell tb1cell11 = new PdfPCell(new Phrase(data[1][1], para));
                tb1cell11.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell11.HorizontalAlignment = Element.ALIGN_CENTER;
                tb1cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb1cell12 = new PdfPCell(new Phrase(data[1][2], para));
                tb1cell12.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell12.HorizontalAlignment = Element.ALIGN_CENTER;
                tb1cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb1cell13 = new PdfPCell(new Phrase(data[1][3], para));
                tb1cell13.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell13.HorizontalAlignment = Element.ALIGN_LEFT;
                tb1cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb1cell14 = new PdfPCell(new Phrase(data[1][4], para));
                tb1cell14.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell14.HorizontalAlignment = Element.ALIGN_LEFT;
                tb1cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb1cell15 = new PdfPCell(new Phrase(data[1][5], para));
                tb1cell15.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell15.HorizontalAlignment = Element.ALIGN_LEFT;
                tb1cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb1cell16 = new PdfPCell(new Phrase(data[1][6], para));
                tb1cell16.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb1cell16.HorizontalAlignment = Element.ALIGN_LEFT;
                tb1cell16.VerticalAlignment = Element.ALIGN_CENTER;

                tablefirst.AddCell(tb1cell11);
                tablefirst.AddCell(tb1cell12);
                tablefirst.AddCell(tb1cell13);
                tablefirst.AddCell(tb1cell14);
                tablefirst.AddCell(tb1cell15);
                tablefirst.AddCell(tb1cell16);

                for (int valueone = 2; valueone < num_rows - 2; valueone++)
                {
         //           string  roundvalue = String.Format("{ 0:0.00}", data[valueone][4]);
                    decimal tempvalue = decimal.Parse(data[valueone][4], CultureInfo.InvariantCulture);
                    decimal tempvaluerounded = decimal.Round(tempvalue, 2, MidpointRounding.AwayFromZero);
                    PdfPCell tb1cellu1 = new PdfPCell(new Phrase(data[valueone][1], value));
                    tb1cellu1.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell tb1cellu2 = new PdfPCell(new Phrase(data[valueone][2], value));
                    tb1cellu2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb1cellu3 = new PdfPCell(new Phrase(data[valueone][3], value));
                    tb1cellu3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb1cellu4 = new PdfPCell(new Phrase(tempvaluerounded.ToString("0.000"), value));
                    tb1cellu4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb1cellu5 = new PdfPCell(new Phrase(data[valueone][5], value));
                    tb1cellu5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb1cellu6 = new PdfPCell(new Phrase(data[valueone][6], value));
                    tb1cellu6.HorizontalAlignment = Element.ALIGN_RIGHT;

                    tablefirst.AddCell(tb1cellu1);
                    tablefirst.AddCell(tb1cellu2);
                    tablefirst.AddCell(tb1cellu3);
                    tablefirst.AddCell(tb1cellu4);
                    tablefirst.AddCell(tb1cellu5);
                    tablefirst.AddCell(tb1cellu6);
                }
            }

            private void tabletwo()
            {
                PdfPCell tb2cell11 = new PdfPCell(new Phrase(data[1][1], para));
                tb2cell11.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell11.HorizontalAlignment = Element.ALIGN_CENTER;
                tb2cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb2cell12 = new PdfPCell(new Phrase(data[1][2], para));
                tb2cell12.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell12.VerticalAlignment = Element.ALIGN_CENTER;
                tb2cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb2cell13 = new PdfPCell(new Phrase(data[1][7], para));
                tb2cell13.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell13.HorizontalAlignment = Element.ALIGN_LEFT;
                tb2cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb2cell14 = new PdfPCell(new Phrase(data[1][8], para));
                tb2cell14.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell14.HorizontalAlignment = Element.ALIGN_LEFT;
                tb2cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb2cell15 = new PdfPCell(new Phrase(data[1][9], para));
                tb2cell15.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell15.HorizontalAlignment = Element.ALIGN_LEFT;
                tb2cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb2cell16 = new PdfPCell(new Phrase(data[1][10], para));
                tb2cell16.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb2cell16.HorizontalAlignment = Element.ALIGN_LEFT;
                tb2cell16.VerticalAlignment = Element.ALIGN_MIDDLE;

                tablesecond.AddCell(tb2cell11);
                tablesecond.AddCell(tb2cell12);
                tablesecond.AddCell(tb2cell13);
                tablesecond.AddCell(tb2cell14);
                tablesecond.AddCell(tb2cell15);
                tablesecond.AddCell(tb2cell16);

                for (int valuetwo = 2; valuetwo < num_rows - 2; valuetwo++)
                {
                    PdfPCell tb2cellu1 = new PdfPCell(new Phrase(data[valuetwo][1], value));
                    tb2cellu1.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell tb2cellu2 = new PdfPCell(new Phrase(data[valuetwo][2], value));
                    tb2cellu2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb2cellu3 = new PdfPCell(new Phrase(data[valuetwo][7], value));
                    tb2cellu3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb2cellu4 = new PdfPCell(new Phrase(data[valuetwo][8], value));
                    tb2cellu4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb2cellu5 = new PdfPCell(new Phrase(data[valuetwo][9], value));
                    tb2cellu5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb2cellu6 = new PdfPCell(new Phrase(data[valuetwo][10], value));
                    tb2cellu6.HorizontalAlignment = Element.ALIGN_RIGHT;

                    tablesecond.AddCell(tb2cellu1);
                    tablesecond.AddCell(tb2cellu2);
                    tablesecond.AddCell(tb2cellu3);
                    tablesecond.AddCell(tb2cellu4);
                    tablesecond.AddCell(tb2cellu5);
                    tablesecond.AddCell(tb2cellu6);
                }
            }

            private void tablethree()
            {
                PdfPCell tb3cell11 = new PdfPCell(new Phrase(data[1][1], para));
                tb3cell11.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell11.HorizontalAlignment = Element.ALIGN_CENTER;
                tb3cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb3cell12 = new PdfPCell(new Phrase(data[1][2], para));
                tb3cell12.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell12.HorizontalAlignment = Element.ALIGN_CENTER;
                tb3cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb3cell13 = new PdfPCell(new Phrase(data[1][11], para));
                tb3cell13.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell13.HorizontalAlignment = Element.ALIGN_LEFT;
                tb3cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb3cell14 = new PdfPCell(new Phrase(data[1][12], para));
                tb3cell14.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell14.HorizontalAlignment = Element.ALIGN_LEFT;
                tb3cell14.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb3cell15 = new PdfPCell(new Phrase(data[1][13], para));
                tb3cell15.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell15.HorizontalAlignment = Element.ALIGN_LEFT;
                tb3cell15.VerticalAlignment = Element.ALIGN_MIDDLE;
                PdfPCell tb3cell16 = new PdfPCell(new Phrase(data[1][14], para));
                tb3cell16.BackgroundColor = BaseColor.LIGHT_GRAY;
                tb3cell16.HorizontalAlignment = Element.ALIGN_LEFT;
                tb3cell16.VerticalAlignment = Element.ALIGN_MIDDLE;

                tablethird.AddCell(tb3cell11);
                tablethird.AddCell(tb3cell12);
                tablethird.AddCell(tb3cell13);
                tablethird.AddCell(tb3cell14);
                tablethird.AddCell(tb3cell15);
                tablethird.AddCell(tb3cell16);

                for (int valuethree = 2; valuethree < num_rows - 2; valuethree++)
                {
                    PdfPCell tb3cellu1 = new PdfPCell(new Phrase(data[valuethree][1], value));
                    tb3cellu1.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell tb3cellu2 = new PdfPCell(new Phrase(data[valuethree][2], value));
                    tb3cellu2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb3cellu3 = new PdfPCell(new Phrase(data[valuethree][11], value));
                    tb3cellu3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb3cellu4 = new PdfPCell(new Phrase(data[valuethree][12], value));
                    tb3cellu4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb3cellu5 = new PdfPCell(new Phrase(data[valuethree][13], value));
                    tb3cellu5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPCell tb3cellu6 = new PdfPCell(new Phrase(data[valuethree][14], value));
                    tb3cellu6.HorizontalAlignment = Element.ALIGN_RIGHT;

                    tablethird.AddCell(tb3cellu1);
                    tablethird.AddCell(tb3cellu2);
                    tablethird.AddCell(tb3cellu3);
                    tablethird.AddCell(tb3cellu4);
                    tablethird.AddCell(tb3cellu5);
                    tablethird.AddCell(tb3cellu6);
                }
            }
        */

        private void tableopening(int whch)
        {
            string description;

            switch (whch)
            {
                case 1:
                    description = " Monitoring the load";
                    break;
                case 2:
                    description = " Load the program description";
                    break;
                default:
                    description = " ";
                    break;
            }

            // Create description for the table
            PdfPTable open = new PdfPTable(2);
            open.HorizontalAlignment = 2;
            open.TotalWidth = 500;    // actual width of table in points 
            open.LockedWidth = true;  // fix the width of the table to absolute

            // Opening cell
            PdfPCell tbopencell = new PdfPCell(new Phrase(description, openbold));
            tbopencell.FixedHeight = 17f;
            //       tbopencell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            tbopencell.Colspan = 2;
            tbopencell.BackgroundColor = new BaseColor(100, 100, 100);

            // Cell then document append
            open.AddCell(tbopencell);
            doc.Add(open);
        }

        public static class DataValue
        {
            public static string[][] THEDATA { get; set; }
            public static int[] rowcontains { get; set; }
            public static int totalrow { get; set; }
        }

        // The page event as template use
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
                catch (DocumentException de)
                {
                    MessageBox.Show("This is the document error cause: {0}", de.ToString());
                }
                catch (System.IO.IOException ioe)
                {
                    MessageBox.Show("This is the IO error cause: {0}", ioe.ToString());
                }
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                iTextSharp.text.Font headerfontbold = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15, BaseColor.BLACK);
                iTextSharp.text.Font headerfont = FontFactory.GetFont(BaseFont.HELVETICA, 14, BaseColor.BLACK);
                base.OnStartPage(writer, document);
                /*            contentbyte.MoveTo(50, document.PageSize.GetTop(80));
                            contentbyte.LineTo(document.PageSize.Width - 50, document.PageSize.GetTop(80));
                            contentbyte.Stroke(); */
                /*            Stream amslogo = new FileStream("Amsonic_logo.png", FileMode.Open, FileAccess.Read, FileShare.Read);
                            iTextSharp.text.Image amsnc = iTextSharp.text.Image.GetInstance(amslogo);
                            amsnc.SetAbsolutePosition(30f, document.PageSize.GetTop(75));
                            amsnc.ScaleAbsoluteHeight(45f);
                            amsnc.ScaleAbsoluteWidth(190);
                            amsnc.Alignment = Element.ALIGN_LEFT;
                            // Create table for header 
                            PdfPTable headertable = new PdfPTable(4);
                            headertable.HorizontalAlignment = 2;
                            headertable.TotalWidth = 350;    // actual width of table in points 
                            headertable.LockedWidth = true;  // fix the width of the table to absolute
                            headertable.SpacingBefore = 20f;
                            // relative col widths in proportions 
                            float[] width = new float[] { 1f, 2.5f, 1.5f, 2f };
                            headertable.SetWidths(width);
                            BaseColor Colour = new BaseColor(255, 255, 205);

                            // First table title
                            PdfPCell tb1cellt1 = new PdfPCell(new Phrase("Title:", firstbold));
                            tb1cellt1.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
                            tb1cellt1.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tb1cellt1.FixedHeight = 20f;
                            tb1cellt1.BackgroundColor = Colour;
                            PdfPCell tb1cellt2 = new PdfPCell(new Phrase(DataValue.THEDATA[0][3], firstbold));
                            tb1cellt2.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                            tb1cellt2.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tb1cellt2.FixedHeight = 20f;
                            tb1cellt2.BackgroundColor = Colour;
                            tb1cellt2.Colspan = 3;

                            // First table first row
                            PdfPCell tb1cell11 = new PdfPCell(new Phrase("Machine:", first));
                            tb1cell11.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
                            tb1cell11.FixedHeight = 15f;
                            tb1cell11.BackgroundColor = Colour;
                            PdfPCell tb1cell12 = new PdfPCell(new Phrase(DataValue.THEDATA[1][0], first));
                            tb1cell12.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            tb1cell12.FixedHeight = 15f;
                            tb1cell12.BackgroundColor = Colour;
                            PdfPCell tb1cell13 = new PdfPCell(new Phrase("Page:", first));
                            tb1cell13.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                            tb1cell13.FixedHeight = 15f;
                            tb1cell13.BackgroundColor = Colour;
                            tb1cell13.HorizontalAlignment = 2;
                            PdfPCell tb1cell14 = new PdfPCell(new Phrase(writer.CurrentPageNumber.ToString() + " of " + writer.PageNumber.ToString(), first));
                            tb1cell14.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                            tb1cell14.FixedHeight = 15f;
                            tb1cell14.BackgroundColor = Colour;
                            tb1cell14.HorizontalAlignment = 2;

                            // First table second row
                            PdfPCell tb1cell21 = new PdfPCell(new Phrase("Operator:", first));
                            tb1cell21.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                            tb1cell21.FixedHeight = 15f;
                            tb1cell21.BackgroundColor = Colour;
                            PdfPCell tb1cell22 = new PdfPCell(new Phrase(DataValue.THEDATA[2][0], first));
                            tb1cell22.Border = 0;
                            tb1cell22.FixedHeight = 15f;
                            tb1cell22.BackgroundColor = Colour;
                            PdfPCell tb1cell23 = new PdfPCell(new Phrase("Date:", first));
                            tb1cell23.Border = 0;
                            tb1cell23.FixedHeight = 15f;
                            tb1cell23.BackgroundColor = Colour;
                            tb1cell23.HorizontalAlignment = 2;
                            PdfPCell tb1cell24 = new PdfPCell(new Phrase("date", first));
                            tb1cell24.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            tb1cell24.FixedHeight = 15f;
                            tb1cell24.BackgroundColor = Colour;
                            tb1cell24.HorizontalAlignment = 2;

                            // First table third row
                            PdfPCell tb1cell31 = new PdfPCell(new Phrase("Software:", first));
                            tb1cell31.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            tb1cell31.FixedHeight = 15f;
                            tb1cell31.BackgroundColor = Colour;
                            PdfPCell tb1cell32 = new PdfPCell(new Phrase(DataValue.THEDATA[1][3], first));
                            tb1cell32.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            tb1cell32.FixedHeight = 15f;
                            tb1cell32.BackgroundColor = Colour;
                            PdfPCell tb1cell33 = new PdfPCell(new Phrase("Time of Printing:", first));
                            tb1cell33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            tb1cell33.FixedHeight = 15f;
                            tb1cell33.BackgroundColor = Colour;
                            tb1cell33.HorizontalAlignment = 2;
                            PdfPCell tb1cell34 = new PdfPCell(new Phrase(DataValue.THEDATA[2][9], first));
                            tb1cell34.Border = iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            tb1cell34.FixedHeight = 15f;
                            tb1cell34.BackgroundColor = Colour;
                            tb1cell34.HorizontalAlignment = 2;

                            // First table cell append
                            headertable.AddCell(tb1cellt1);
                            headertable.AddCell(tb1cellt2);
                            headertable.AddCell(tb1cell11);
                            headertable.AddCell(tb1cell12);
                            headertable.AddCell(tb1cell13);
                            headertable.AddCell(tb1cell14);
                            headertable.AddCell(tb1cell21);
                            headertable.AddCell(tb1cell22);
                            headertable.AddCell(tb1cell23);
                            headertable.AddCell(tb1cell24);
                            headertable.AddCell(tb1cell31);
                            headertable.AddCell(tb1cell32);
                            headertable.AddCell(tb1cell33);
                            headertable.AddCell(tb1cell34);

                            contentbyte.AddImage(amsnc);
                            headertable.WriteSelectedRows(0, -1, document.GetLeft(160), document.PageSize.GetTop(20), contentbyte); */
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

        public class filecheck
        {
            static string filepath = @"C:\Users\TC\Desktop\jks\Full_Set_Ams_Gen\Source Code\Amsonic third format\Frm_C_Drive_Reports\format\";
            public FileInfo[] isthereanyfile()
            {
                DirectoryInfo dir = new DirectoryInfo(filepath);
                FileInfo[] file;
                try
                {
                    file = dir.GetFiles("*.csv");
                }
                catch
                {
                    file = null;
                    //            MessageBox.Show("Please create a JKS folder and create a report_csv.");
                }
                return file;
            }
        }
    }
}

