using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Prototype.DataAccess
{
    public class SqlParser
    {
        private string[] keywords = { "action", "add", "all", "alter", "and", "any", "as", "asc", "authorization", "avg", "backup", "begin", "between", "break", "browse", "bulk", "by", "cascade", 
                                  "case", "check", "checkpoint", "close", "clustered", "coalesce", "collate", "column", "commit", "compute", "constraint", "contains", "containstable",
                                  "continue", "convert", "count", "create", "cross", "current", "current_date", "current_time", "current_timestamp", "current_user", "cursor",
                                  "database", "databasepassword", "dateadd", "datediff", "datename", "datepart", "dbcc", "deallocate", "declare", "default", "delete", "deny", "desc",
                                  "disk", "distinct", "distributed", "double", "drop", "dump", "else", "encryption", "end", "errlvl", "escape", "except", "exec", "execute", "exists",
                                  "exit", "expression", "fetch", "file", "fillfactor", "for", "foreign", "freetext", "freetexttable", "from", "full", "function", "goto", "grant",
                                  "group", "having", "holdlock", "identity", "identity_insert", "identitycol", "if", "in", "index", "inner", "insert", "intersect", "into", "is",
                                  "joint", "key", "kill", "left", "like", "lineno", "load", "max", "min", "national", "no", "nocheck", "nonclustered", "not", "null", "nullif", "of", "off",
                                  "offsets", "on", "open", "opendatasource", "openquery", "openrowset", "openxml", "option", "or", "order", "outer", "over", "percent", "plan",
                                  "precision", "primary", "print", "proc", "procedure", "public", "raiserror", "read", "readtext", "reconfigure", "references", "replication",
                                  "restore", "restrict", "return", "revoke", "right", "rollback", "rowcount", "rowguidcol", "rule", "save", "schema", "select", "session_user", "set",
                                  "setuser", "shutdown", "some", "statistics", "sum", "system_user", "table", "textsize", "then", "to", "top", "tran", "transaction", "trigger",
                                  "truncate", "tsequal", "union", "unique", "update", "updatetext", "use", "user", "values", "varying", "view", "waitfor", "when", "where", "while",
                                  "with", "writetext" };
        private string[] types = { "bigint", "int", "integer", "smallint", "tinyint", "bit", "numeric", "decimal", "dec", "money", "float", "real", "datetime", "national character",
                                  "nchar", "national character varying", "nvarchar", "ntext", "binary", "varbinary", "image", "uniqueidentifier", "timestamp", "rowversion" };
        private Regex regexNumbers;
        private Regex regexKeywords;
        private Regex regexStrings;
        private Regex regexNoKeysQuoted;
        private Regex regexNoNamesBrackets;
        private Regex regexPar;
        private Regex regexInsertColortbl;
        private Regex regexTypes;
        private RichTextBox richTextBoxAux = new RichTextBox();
        private RichTextBox _RichTextBox;
        private int selectionStart;
        private int selectionLength;
        private int numLines;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static IntPtr SendMessage(IntPtr HWnd, UInt32 Msg, IntPtr WParam, IntPtr LParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static IntPtr SendMessage(IntPtr HWnd, UInt32 Msg, Int32 WParam, ref Point LParam);

        private void SetScrollPosition(Control control, Point position)
        {
            const uint WM_USER = 0x400;
            const uint EM_SETSCROLLPOS = WM_USER + 222;
            SendMessage(control.AddHandler, EM_SETSCROLLPOS, 0, ref position);
        }

        private Point GetScrollPosition(Control control)
        {
            const uint WM_USER = 0x400;
            const uint EM_GETSCROLLPOS = WM_USER + 221;
            Point pt = new Point();
            SendMessage(control.Handle, EM_GETSCROLLPOS, 0, ref pt);
            return pt;
        }

        private void HideSelection(Control control, bool hide)
        {
            const uint WM_USER = 0x400;
            const uint EM_HIDESELECTION = WM_USER + 63;
            SendMessage(control.Handle, EM_HIDESELECTION, (IntPtr)(hide ? 1 : 0), IntPtr.Zero);
        }

        public SqlParser()
        {
            StringBuilder pattern = new StringBuilder();
            foreach (string key in keywords) pattern.Append(@"(?<1>\b" + key + @"\b)|");
            pattern.Length--;
            regexKeywords = new Regex(pattern.ToString(), RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            pattern.Clear();
            foreach (string typ in types) pattern.Append(@"(?<1>\b" + typ + @"\b)|");
            pattern.Length--;
            regexTypes = new Regex(pattern.ToString(), RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            regexInsertColortbl = new Regex(@";}}");
            regexNoKeysQuoted = new Regex(@"(?<1>')(?<2>[^\\]*)(?<3>\\cf\d+ )(?<4>[^\\]*)(?<5>\\cf\d+ )(?<6>.*')", RegexOptions.Compiled | RegexOptions.Multiline);
            regexNoNamesBrackets = new Regex(@"(?<1>\[)(?<2>[^\\]*)(?<3>\\cf\d+ )(?<4>[^\\]*)(?<5>\\cf\d+ )(?<6>[^\[]*\])", RegexOptions.Compiled | RegexOptions.Multiline);
            regexNumbers = new Regex(@"(?<1>\b\d+\b)", RegexOptions.Compiled | RegexOptions.Multiline);
            regexStrings = new Regex(@"(?<1>'.*[^\\]')", RegexOptions.Compiled | RegexOptions.Multiline);
            regexPar = new Regex(@"\\par\b", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.RightToLeft);
            string s = Parse(""); // Initialize Regex;
        }

        public string Parse(string text)
        {
            return Parse(text, null);
        }

        public string Parse(string text, Font font)
        {
            FlowDocument mcFlowDoc = new FlowDocument();
            //richTextBoxAux.Font=font;
            if (font != null)
            {
                richTextBoxAux.FontFamily = new System.Windows.Media.FontFamily(font.FontFamily.Name);
                richTextBoxAux.FontSize = font.Size;
                //richTextBoxAux.FontStyle = new System.Windows.FontStyle(font.Style.ToString());
            }

            //richTextBoxAux.Text=text;
            Paragraph para = new Paragraph();
            para.Inlines.Add(text);
            mcFlowDoc.Blocks.Add(para);
            richTextBoxAux.Document = mcFlowDoc;

            string rtf = richTextBoxAux.Rtf;

            // Insert color table
            rtf = regexInsertColortbl.Replace(rtf, @";}}{\colortbl ;\red255\green0\blue0;\red0\green128\blue0;\red0\green0\blue255;\red0\green128\blue128;}");

            rtf = regexNumbers.Replace(rtf, @"\cf1 ${1}\cf0 ");
            rtf = regexKeywords.Replace(rtf, @"\cf3 ${1}\cf0 ");
            rtf = regexTypes.Replace(rtf, @"\cf4 ${1}\cf0 ");
            rtf = regexStrings.Replace(rtf, @"\cf2 ${1}\cf0 ");
            while (regexNoKeysQuoted.IsMatch(rtf)) rtf = regexNoKeysQuoted.Replace(rtf, "${1}${2}${4}${6}");
            while (regexNoNamesBrackets.IsMatch(rtf)) rtf = regexNoNamesBrackets.Replace(rtf, "${1}${2}${4}${6}");

            return rtf;
        }

        private void ParseChanges()
        {
            if (_RichTextBox.Lines.Length > numLines)
            {
                _RichTextBox.Rtf = Parse(_RichTextBox.Text, _RichTextBox.Font);
            }
            else
            {
                int numLine = _RichTextBox.GetLineFromCharIndex(_RichTextBox.SelectionStart);
                int posIni = _RichTextBox.GetFirstCharIndexFromLine(numLine);
                HideSelection(_RichTextBox, true);

                _RichTextBox.Select(posIni, _RichTextBox.Lines[numLine].Length);
                _RichTextBox.SelectedRtf = regexPar.Replace(Parse(_RichTextBox.SelectedText, _RichTextBox.Font), "");

                HideSelection(_RichTextBox, false);
            }
        }

        public RichTextBox RichTextBox
        {
            get
            {
                return _RichTextBox;
            }

            set
            {
                if (value == _RichTextBox) return;
                if (_RichTextBox != null)
                {
                    _RichTextBox.TextChanged -= RichTextBox_TextChanged;
                    _RichTextBox.KeyDown -= RichTextBox_KeyDown;
                }
                _RichTextBox = value;
                _RichTextBox.TextChanged += RichTextBox_TextChanged;
                _RichTextBox.KeyDown += RichTextBox_KeyDown;
            }
        }

        private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V && _RichTextBox.SelectionLength != 0) numLines = -1;
            if (e.KeyCode == Keys.Return) numLines++;
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_RichTextBox.Text.Length > 0)
            {
                _RichTextBox.TextChanged -= RichTextBox_TextChanged;
                Point pos = GetScrollPosition(_RichTextBox);
                selectionLength = _RichTextBox.SelectionLength;
                selectionStart = _RichTextBox.SelectionStart;

                ParseChanges();

                _RichTextBox.SelectionStart = selectionStart;
                _RichTextBox.SelectionLength = selectionLength;
                SetScrollPosition(_RichTextBox, pos);
                _RichTextBox.TextChanged += RichTextBox_TextChanged;
            }
            numLines = _RichTextBox.Lines.Length;
        }
    }
}
