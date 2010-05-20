using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tutorial2.Domain;

namespace Tutorial2.Application
{
    public interface IBlogView
    {
        Author Author { set; get;}

        Blog Blog { set; get;}

        BindingList<Post> Posts { set; get;}

    }
}
