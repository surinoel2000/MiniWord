using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MiniWord_NhatMinhBui
{
    public partial class frm_NhatMinh : Form
    {
        //-----------------------//

        #region định nghĩa biến

        public string duongdan = "";
        public int saveinfo = 0; //kiểm tra tình trạng lưu -> 0 chưa lưu , 1 lưu r
        public SaveFileDialog save; //tạo dialog -> hiển thị cửa sổ lưu
        public OpenFileDialog open; //tạo dialog -> hiển thị cửa sổ open file
        public ColorDialog color; //dialog -> hiển thị cửa sổ đổi màu chữ có sẵn
        public FontDialog dialogfont; //dialog -> hiển thị font chữ có sẵn

        public int
            kiemtra = 0; // biến sự kiện này sẽ quy định các event: in đậm , nghiêng, gạch chân , căn lề: trái, phải , giữa

        #endregion

        //-----------------------//

        frmtimkiem Fsearch;

        //-----------------------//

        #region Method làm việc

        public void fontdefine()
        {
            float fsize = 10;

            if (fontsizebox.SelectedIndex != -1)
            {
                fsize = (float) float.Parse(fontsizebox.SelectedItem.ToString());
            }

            string fname = "Times New Roman";
            if (fontnamebox.SelectedIndex != -1)
            {
                fname = fontnamebox.SelectedItem.ToString();
            }

            try
            {
                Font font = new Font(new FontFamily(fname), fsize);
                fieldtextbox.SelectionFont = font;

            }
            catch
            {
                MessageBox.Show("Font này không hỗ trợ kiểu hiển thị hiện tại", "Lỗi", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            //tao danh sach Fonts lấy từ hệ thống chính máy tính bạn.
            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (FontFamily f in fonts.Families)
            {
                fontnamebox.Items.Add(f.Name.ToString());
            }

            fontnamebox.SelectedItem = "";
            for (int i = 10; i <= 72; i++)
            {
                fontsizebox.Items.Add(i.ToString());
                fontsizebox.SelectedIndex.Equals(i);
            }
            //set mặc định cho Fonts.
            //rtbinfo.Dispose();
        }

        public void dinhdangsave() //định nghĩa định dạng file khi save
        {
            save = new SaveFileDialog();
            save.DefaultExt = "rtf";
            // save.DefaultExt = "txt";
            save.Filter = "Text Document | *.txt | RTF File Document (*.rtf)| *.rtf |All Files (*.*)|*.*";
            //Các loại định dạng khác sẽ cố định khi Save
        }

        //-----------------------------------//
        public void checkExit() //kiểm tra thoát
        {
            //kiểm tra file được lưu chưa -> =0 chưa lưu or = 1 lưu r
            if (this.saveinfo == 0) //nếu chưa lưu thì
            {
                if (!fieldtextbox.Text.Equals("")) // kiểm tra content bên trong
                {
                    //nếu khác rỗng thì
                    if (this.duongdan.Equals(""))
                    {
                        //nếu đường dẫn rỗng thì
                        save = new SaveFileDialog();
                        save.DefaultExt = "rtf";
                        save.Filter =
                            "RTF Document (*.rtf) |*.rtf| Doc File Document(*.doc) | *.doc | All files(*.*) | *.* ";
                        DialogResult result = save.ShowDialog();

                        //Mặc định sẽ hiên ra cửa sổ Open fILE Và Save file với 3 lựa chọn OK/NO/CANCEL
                        if (result == DialogResult.Cancel)
                        {
                            return; //Nếu chọn Cancel thì trở về như cũ không làm gì
                        }

                        //gắn đường dẫn
                        this.duongdan = save.FileName;
                        try
                        {
                            fieldtextbox.SaveFile(duongdan); //lưu file theo đường dẫn đã chọn
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else // ngược lại trường hơp jđường dẫn rỗng
                    {
                        try
                        {
                            //Có đường dẫn r -> lưu đè file theo đường dẫn
                            fieldtextbox.SaveFile(duongdan);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                        }
                    }
                }
            }
        }
        //-----------------------------------//

        public void checkNew()
        {
            //xuất hiện với 1 thông báo + icon
            if (!fieldtextbox.Text.Equals(""))
            {
                //biến select để đưa ra lựa chọn
                DialogResult select = MessageBox.Show("Bro muốn Lưu File trước khi tạo File MỚI!", "Thông báo",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                //Nếu OK
                if (select == DialogResult.Yes)
                {
                    //Thì
                    //Nếu đường dẫn rỗng
                    if (this.duongdan.Equals(""))
                    {
                        //Tạo sự kiện lưu file
                        save = new SaveFileDialog();
                        save.DefaultExt = "rtf";
                        save.Filter =
                            "RTF Document |*.rtf | Doc File Document(*.doc) | *.doc | All files(*.*) | *.* "; //gán cho là *.rtf

                        //Gọi cửa sổ lưu

                        DialogResult result = save.ShowDialog();

                        //Nếu Cancel
                        if (result == DialogResult.Cancel)
                        {
                            return; //quay về sự kiện thoát
                        }

                        //Nếu không chọn CANCEL
                        //Gán duongdan = với save.FIleName
                        this.duongdan = save.FileName;


                        //Lưu đường dẫn vào biến
                        try
                        {
                            fieldtextbox
                                .SaveFile(
                                    duongdan); // lưu tập tin từ Rich Text Box(Field nhập text định dạng mặc định *.txt
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            return;
                            //thông báo lỗi xong , rời sự kiện
                        }
                    }
                    else
                    {
                        //Có đường dẫn không trống hay đã có đường dẫn -> lưu tập tin mà không gọi cửa sổ lưu
                        try
                        {
                            fieldtextbox.SaveFile(duongdan); //lưu
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông Báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                        }
                    }

                    fieldtextbox.Text = ""; //Khởi tạo mới field nội dung
                }

                //Ngược lại nếu YES
                else if (select == DialogResult.No)
                {
                    fieldtextbox.Text = ""; //thoát không lưu
                }
                else //Ngược lại nếu chọn CANCEL
                {
                    return; // trả về không lưu gì
                }

            }
            else
            {
                return;
            }
        }
        //-----------------------------------//


        //-----------------------------------//
        public void checkedOpen() //kiểm tra open
        {

            open = new OpenFileDialog();
            save = new SaveFileDialog();
            if (!this.fieldtextbox.Text.Equals(""))
            {
                DialogResult result2 = MessageBox.Show("Bro muốn lưu lại file trước khi mở file mới ?", "Thông báo",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //tạo 1 biến trả lời câu hỏi

                if (result2 == DialogResult.Yes) // nếu chọn YES
                {
                    //giống menu item thoát
                    if (this.duongdan.Equals(""))
                    {
                        DialogResult resultValue = save.ShowDialog();
                        DialogResult resval = open.ShowDialog();
                        if (resultValue == DialogResult.Cancel)
                        {
                            return;
                        }

                        this.duongdan = save.FileName;

                        try
                        {
                            /*  using (OpenFileDialog ofd = new OpenFileDialog()
                                  {Filter = "RTF Document |*.rtf", ValidateNames = true, Multiselect = false})
                              {
                                  if (ofd.ShowDialog() == DialogResult.OK)
                                  {
                                       fieldtextbox.LoadFile(ofd.FileName);
                                  }
                              }*/

                            fieldtextbox.SaveFile(duongdan);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else
                    {
                        try
                        {

                            fieldtextbox.SaveFile(duongdan);
                            save.DefaultExt = "rtf";
                            save.Filter = "RTF Document |*.rtf |Doc Ducument |*.doc| All files(*.*) | *.* ";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                        }
                    }

                    DialogResult Result = open.ShowDialog();
                    //tạo 1 biến kết quả check việc mở file
                    //Cửa sổ sẽ có 2 button Open, Cancel

                    if (Result == DialogResult.Cancel) //nếu chọn cancel
                    {
                        return;
                    }
                    else //hoặc Open
                    {
                        try
                        {
                            this.duongdan = open.FileName; //lưu đường dẫn tệp tin

                            Stream myStream;
                            OpenFileDialog openFileDialog1 = new OpenFileDialog();

                            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if ((myStream = openFileDialog1.OpenFile()) != null) ;
                                {
                                    string strfilename = openFileDialog1.FileName;
                                    string filetext = File.ReadAllText(strfilename);
                                    fieldtextbox.Text = filetext;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            //THÔNG BÁO LỖI
                        }
                    }
                }
                else if (result2 == DialogResult.No)
                {
                    DialogResult Result = open.ShowDialog();
                    //tạo tệp tin từ việc mở file
                    //hộp thoại mở tập tin có 2 nút quyết định open - cancel
                    if (Result == DialogResult.Cancel) //nếu chọn cancel
                    {
                        return; //rời khỏi sự kiện
                    }
                    else //hoặc chọn mở
                    {
                        try
                        {
                            this.duongdan = open.FileName; //lưu đường dẫn tập tin

                        }
                        catch (Exception ex) //nếu có ngoại lệ
                        {
                            MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            //thông báo lỗi
                        }
                    }
                }
                else
                {
                    return; //rời khỏi sự kiện
                }
            }
            else
            {
                DialogResult Result = open.ShowDialog();
                //tạo một biến kết quả từ việc mở tập tin
                //lúc đó hộp thoại mở tập tin sẽ có 2 nút là Open và Cancel
                if (Result == DialogResult.Cancel) //nếu chọn Cancel
                {
                    return; //rời khỏi sự kiện
                }
                else //hoặc chọn mở
                {
                    try //thử
                    {
                        using (OpenFileDialog ofd = new OpenFileDialog()
                            {Filter = "RTF Document |*.rtf", ValidateNames = true, Multiselect = false})
                        {
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                fieldtextbox.LoadFile(ofd.FileName);
                            }
                        }

                        this.duongdan = open.FileName; //lưu đường dẫn tập tin

                    }
                    catch (Exception Ex) //nếu có ngoại lệ
                    {
                        MessageBox.Show(Ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //thông báo lỗi
                    }
                }
            }
        }

        public void saveData()
        {
            //giống trên nhưng không cẦN biết tập tin
            if (this.duongdan.Equals(""))
            {
                save = new SaveFileDialog();
                save.DefaultExt = "rtf";
                save.Filter = "RTF Document |*.rtf|Doc File Document (*.doc)|*.doc|All files (*.*)|*.*";
                DialogResult Result = save.ShowDialog();

                if (Result == DialogResult.Cancel)
                {
                    return;
                }

                duongdan = save.FileName;

                try
                {
                    fieldtextbox.SaveFile(duongdan);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            this.saveinfo = 1; //đặt lại trạng thái cho biến kiểm tra việc lưu
        }

        #endregion

        //----------------------




        public frm_NhatMinh()
        {
            InitializeComponent();

        }

        private void FormSoanThao_Load(object sender, EventArgs e)
        {

        }
        private void exitprogram(object sender, FormClosingEventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn muốn thoát khỏi chương trinh", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (thoat == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                checkExit();
            }

        }

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {

        }



        #region MenuStrip

        //Menu strip -> Save AS

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dinhdangsave();
            DialogResult select = save.ShowDialog();
            this.duongdan = save.FileName;
            try
            {
                if (select == DialogResult.OK)
                {
                    fieldtextbox.SaveFile(duongdan);
                }
                else
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Menu Strip -> Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kiem tra
            if (this.saveinfo == 0) //Tức là đoạn văn bản chưa được lưu.
            {
                Application.Exit();
                /************************************************************************/
                /** Khi thoát chương trình.
                 *  Mặc định hệ thống sẽ gọi sự kiện FormClosing hoặc FormClosed.
                 *  Để tránh trường hợp này, ta nên lập trình cho sự kiện thoát của FORM trước.
                 *  Sau đó vào phần code cho Menu chỉ cần khọi hàm thoát, khi đó chương trình sẽ tự động gọi hàm.
                 *  FormClosing hoặc FormClosed.
                 */
                /************************************************************************/
            }
            // Ngược lại trường hợp kiemTraSave = 0
            // tức là đã lưu rồi.
            else
            {
                Application.Exit();
            }
        }

        #endregion

        //----------------------------------//

        #region Event trong Toolstrip

        //toolstrip -> new file
        private void newfilebtn_Click(object sender, EventArgs e)
        {

            if (saveinfo == 0)
            {
                checkNew();
            }
            else
            {
                fieldtextbox.Text = ""; //tạo mới(clear hết text lại field nhập nội dung)
            }
        }

        //toolstrip open file button
        private void openfilebtn_Click(object sender, EventArgs e)
        {
            if (saveinfo == 0)
            {
                checkedOpen();
            }
        }

        //toolstrip save file button
        private void savefilebtn_Click(object sender, EventArgs e)
        {

            if (saveinfo == 0)
            {
                saveData();
            }
            else
            {
                fieldtextbox.SaveFile(duongdan);
            }
        }

        //toolstrip cut text button
        private void cutfilebtn_Click(object sender, EventArgs e)
        {
            fieldtextbox.Cut();
        }

        //toolstrip copy text button
        private void copyfilebtn_Click(object sender, EventArgs e)
        {
            fieldtextbox.Copy();
        }

        //toolstrip paste text button
        private void pastefilebtn_Click(object sender, EventArgs e)
        {
            fieldtextbox.Paste();
        }

        // ToolStrip undo button
        private void undobtn_Click(object sender, EventArgs e)
        {
            fieldtextbox.Undo();
        }

        //toolstrip redo button
        private void redobtn_Click(object sender, EventArgs e)
        {
            fieldtextbox.Redo();
        }

        //toolstrip Font button
        private void fontbtn_Click(object sender, EventArgs e)
        {
            dialogfont = new FontDialog();
            DialogResult font = dialogfont.ShowDialog();
            if (this.fieldtextbox.SelectedText.Equals(""))
            {
                if (font == DialogResult.OK)
                {
                    fieldtextbox.Font = dialogfont.Font;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (font == DialogResult.OK)
                {
                    fieldtextbox.SelectionFont = dialogfont.Font;
                }
                else
                {
                    return;
                }
            }
        }

        //toolstrip chon font combo box
        private void fontselect_Click(object sender, EventArgs e)
        {
            fontdefine();
        }

        //toolstrip combobox chọn cỡ texxt

        //Bold button
        private void nutindam_Click(object sender, EventArgs e)
        {
            indam();
        }

        //nút gạch chân
        private void nutgachchan_Click(object sender, EventArgs e)
        {
            gachchan();
        }

        //in nghiêng
        private void nutinghieng_Click(object sender, EventArgs e)
        {
            innghieng();
        }

        //button tìm kiếm
        private void timkiembtn_Click(object sender, EventArgs e)
        {
            if (Fsearch == null || Fsearch.IsDisposed)
                Fsearch = new frmtimkiem(fieldtextbox);
            Fsearch.ShowFind();
        }

        //chèn ảnh
        private void chenanhbtn_Click(object sender, EventArgs e)
        {
            open = new OpenFileDialog();

            open.Filter = "Images| *.bmp;*.jpg;*.png;*.gif;*.ico"; //dinh dang cac type img co the insert
            open.Multiselect = false;
            open.FileName = "";
            DialogResult result = open.ShowDialog();
            if (result == DialogResult.OK)
            {
                Image img = Image.FromFile(open.FileName);
                Clipboard.SetImage(img);
                fieldtextbox.Paste();
                fieldtextbox.Focus();
            }
            else
            {
                fieldtextbox.Focus();
            }
        }

        public void mauchu()
        {
             color = new ColorDialog();
            DialogResult mauchu = color.ShowDialog(); // hiển thị hộp thoại màu chữ
            if (mauchu == DialogResult.OK)
            {
                fieldtextbox.SelectionColor = color.Color; //đặt lại màu
                toolStrip_mau.BackColor = color.Color;
            }
            else
            {
                return;
            }
        }
        //set màu chữ
        private void mauchubtn_Click(object sender, EventArgs e)
        {
            mauchu();
        }

        //-------------------------------------------//

        #endregion

        #region Căn trái - phải - giữa - in đậm - nghiêng - gạch chân

        public void cantrai()
        {
            fieldtextbox.SelectionAlignment = HorizontalAlignment.Left;
        }

        public void cangiua()
        {
            fieldtextbox.SelectionAlignment = HorizontalAlignment.Center;
        }

        public void cantphai()
        {
            fieldtextbox.SelectionAlignment = HorizontalAlignment.Right;
        }

        public void indam()
        {
            if (kiemtra == 0)
            {
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Bold);
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Bold);
                kiemtra++;
            }
            else
            {
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Regular);
                kiemtra--;
            }
        }

        public void innghieng()
        {
            if (kiemtra == 0)
            {
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Italic);
                kiemtra++;
            }
            else
            {
                    fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Regular);
                kiemtra--;
            }
        }

        public void gachchan()
        {
            if (kiemtra == 0)
            {
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Underline);
                kiemtra++;
            }
            else
            {
                fieldtextbox.SelectionFont = new Font(fieldtextbox.SelectionFont, FontStyle.Regular);
                kiemtra--;
            }
        }

        //căn trái
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            cantrai();
        }

        //căn giữa
        private void cangiuabtn_Click(object sender, EventArgs e)
        {
            cangiua();
        }

        //căn phải
        private void canphaibtn_Click(object sender, EventArgs e)
        {
            cantphai();
        }

        #endregion

        private void colortextc_Click(object sender, EventArgs e)
        {

        }

        private void tsbcbfonts_Click(object sender, EventArgs e)
        {
            //loadFonts();
            fontdefine();
        }

        private void rtbinfo_TextChanged(object sender, EventArgs e)
        {
            //loadFonts();
            // indam();
            // innghieng();
            // gachchan();

        }

        private void tsbcbfontsize_Click(object sender, EventArgs e)
        {
            fontdefine();
        }


        private void rightclick_fonts_Click(object sender, EventArgs e)
        {
            dialogfont = new FontDialog();
            DialogResult font = dialogfont.ShowDialog();
            if (this.fieldtextbox.SelectedText.Equals(""))
            {
                if (font == DialogResult.OK)
                {
                    fieldtextbox.Font = dialogfont.Font;
                }
                else
                {
                    return;
                }

            }
            else
            {
                if (font == DialogResult.OK)
                {
                    fieldtextbox.SelectionFont = dialogfont.Font;
                }
                else
                {
                    return;
                }
            }
        }

        private void bgcolorbtn_Click(object sender, EventArgs e)
        {
            color = new ColorDialog();
            DialogResult bgcolor = color.ShowDialog(); // hiển thị hộp thoại màu chữ
            if (bgcolor == DialogResult.OK)
            {
                fieldtextbox.SelectionColor = color.Color; //đặt lại màu
                fieldtextbox.BackColor = color.Color;
            }
            else
            {
                return;
            }
        }

        private void zoominbtn_Click(object sender, EventArgs e)
        {
           // fontdefine();
            // fieldtextbox.Height = fieldtextbox.Size.Height + 1;
            // fieldtextbox.Width = fieldtextbox.Size.Width + 1;
            float currentSize;
            currentSize = fieldtextbox.Font.Size;
            currentSize += 2.0F;
            fieldtextbox.Font = new Font(fieldtextbox.Font.Name, currentSize, fieldtextbox.Font.Style,
                fieldtextbox.Font.Unit);

        }

        private void zoomoutbtn_Click(object sender, EventArgs e)
        {
          //  fontdefine();
            // fieldtextbox.Height = fieldtextbox.Size.Height -1;
            // fieldtextbox.Width = fieldtextbox.Size.Width - 1;
            float currentSize;
            currentSize = fieldtextbox.Font.Size;
            currentSize -= 2.0F;
            fieldtextbox.Font = new Font(fieldtextbox.Font.Name, currentSize, fieldtextbox.Font.Style,
                fieldtextbox.Font.Unit);


        }

        private void emojiibtn_Click(object sender, EventArgs e)
        {
            /*   Hashtable emotions;

                void CreateEmotions()
               {
                   emotions = new Hashtable(12);
                   emotions.Add(@":)", Properties.Resources.ThumbsUp1);
                   emotions.Add(@"0)", Properties.Resources.AngelSmile1);
                   emotions.Add(@":(", Properties.Resources.CrySmile1);
                   emotions.Add(@":X", Properties.Resources.AngrySmile1);
                   emotions.Add(@"XX", Properties.Resources.DevilSmile);
               }

                void AddEmotions(Hashtable)
               {
                   foreach (string emote in emotions.Keys)
                       while (fieldtextbox.Text.Contains(emote))
                       {
                           int ind = fieldtextbox.Text.IndexOf(emote);
                           fieldtextbox.Select(ind, emote.Length);
                           Clipboard.SetImage((Image)emotions[emote]);
                           fieldtextbox.Paste();
                       }
               }*/
        }

        #region Hot key
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                //cantrai Ctrl+ Q
                case (Keys.Control | Keys.Q):
                {
                    cantrai(); break;

                }
                //canphai Ctrl+ R
                case (Keys.Control | Keys.R):
                {
                    cantphai(); break;

                }
                //canhgiua Ctrl+ E
                case (Keys.Control | Keys.E):
                {
                    cangiua(); break;

                }
                //indam Ctrl+ B
                case (Keys.Control | Keys.B):
                {
                    indam();break;

                }
                //in nghieng Ctrl+ I
                case (Keys.Control | Keys.I):
                {
                    innghieng(); break;

                }
                //gach chan Ctrl+ U
                case (Keys.Control | Keys.U):
                {
                    gachchan(); break;

                }
                //Undo Ctrl+ Z
                case (Keys.Control | Keys.Z):
                {
                    fieldtextbox.Undo(); break;

                }
                //Redo Ctrl+ Y
                case (Keys.Control | Keys.Y):
                {
                    fieldtextbox.Redo(); break;

                }
                //Copy Ctrl+ C
                case (Keys.Control | Keys.C):
                {
                    fieldtextbox.Copy(); break;

                }
                //Paste Ctrl+ V
                case (Keys.Control | Keys.V):
                {
                    fieldtextbox.Paste(); break;

                }
                //Cut Ctrl+ X
                case (Keys.Control | Keys.X):
                {
                    fieldtextbox.Cut(); break;

                }
                //Find Ctrl+ F
                case (Keys.Control | Keys.F):
                {
                    //fieldtextbox.Redo(); break;
                    if (Fsearch == null || Fsearch.IsDisposed)
                        Fsearch = new frmtimkiem(fieldtextbox);
                    Fsearch.ShowFind();
                    break;
                }

                //New File Ctrl+ N
                case (Keys.Control | Keys.N):
                {
                    if (saveinfo == 0)
                    {
                        checkNew();
                    }
                    else
                    {
                        fieldtextbox.Text = ""; //tạo mới(clear hết text lại field nhập nội dung)
                    }
                    break;
                }

                //Open File Ctrl+ O
                case (Keys.Control | Keys.O):
                {
                    if (saveinfo == 0)
                    {
                        checkedOpen();
                    }
                    break;

                }

                //save file Ctrl+ S
                case (Keys.Control | Keys.S):
                {
                    if (saveinfo == 0)
                    {
                        saveData();
                    }
                    else
                    {
                        fieldtextbox.SaveFile(duongdan);
                    }
                        break;
                }

                //Chen anh Ctrl+ G
                case (Keys.Control | Keys.G):
                {
                    open = new OpenFileDialog();

                    open.Filter = "Images| *.bmp;*.jpg;*.png;*.gif;*.ico"; //dinh dang cac type img co the insert
                    open.Multiselect = false;
                    open.FileName = "";
                    DialogResult result = open.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Image img = Image.FromFile(open.FileName);
                        Clipboard.SetImage(img);
                        fieldtextbox.Paste();
                        fieldtextbox.Focus();
                    }
                    else
                    {
                        fieldtextbox.Focus();
                    }
                    break;
                }

                //Zoom In Ctrl+ chuot trai
                case (Keys.Control | Keys.Left):
                {
                    float currentSize;
                    currentSize = fieldtextbox.Font.Size;
                    currentSize += 2.0F;
                    fieldtextbox.Font = new Font(fieldtextbox.Font.Name, currentSize, fieldtextbox.Font.Style,
                        fieldtextbox.Font.Unit);
                        break;

                }

                //Zoom out Ctrl+ Chuot phai
                case (Keys.Control | Keys.Right):
                {
                    if (saveinfo == 0)
                    {
                        checkedOpen();
                    }
                    break;

                }

                //Exit ESC ( ra luon do ton cong save file :))
                case (Keys.Escape):
                {
                    Application.Exit();
                    break;
                        break;
                }




            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
#endregion

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}

