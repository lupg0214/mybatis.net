using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tutorial2.Application;
using Tutorial2.Domain;
using Tutorial2.Test;

namespace Tutorial2.WinUI
{
    public partial class Login : Form, IBlogView 
    {
        public BlogPresenter presenter = null;
        
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.presenter = new BlogPresenter(this, new BlogServiceMock());
        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            presenter.Verify(this.textBoxLogin.Text, this.textBoxPassword.Text);
        }

        #region IBlogView Members

        public User User
        {
            get
            {
                return this.bindingSourceUser.DataSource as User;
            }
            set
            {
                this.bindingSourceUser.SuspendBinding();
                this.bindingSourceUser.DataSource = value;
                this.bindingSourceUser.ResumeBinding();
            }
        }

        public Blog Blog
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public BindingList<Post> Posts
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}