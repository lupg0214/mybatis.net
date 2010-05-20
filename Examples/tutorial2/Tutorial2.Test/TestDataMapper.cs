using IBatisNet.DataMapper.Configuration;
using NUnit.Framework;
using IBatisNet.DataMapper;

using System;
using System.Collections.Generic;
using System.Text;
using Tutorial2.Domain;

namespace Tutorial2.Test
{
    [TestFixture]
    public class TestDataMapper
    {
        private ISqlMapper mapper = null;
        private Author gilles = null;
        
        [SetUp]
        public void SetUp()
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            mapper = builder.Configure(@"../../../Files/sqlMap.config");
        }

        [Test]
        public void SelectBasicAuthor()
        {
            gilles = mapper.QueryForObject<Author>("Author-Select-Basic", 2);
            Assert.IsTrue(gilles.Login=="gilles");
        }

        /// <summary>
        /// Shows lazy loading + resultMap usage on a result property 
        /// </summary>
        [Test]
        public void SelectAuthorAndBlogAndLazyPost()
        {
            gilles = mapper.QueryForObject<Author>("Author-Select", 2);
            
            Assert.IsTrue(gilles.Login == "gilles");
            Assert.IsNotNull(gilles.Blog, "Gilles blog is null");
            Assert.IsTrue(gilles.Blog.Name == "Gilles Weblog", "Gilles blog is not 'Gilles Weblog' ");

            Assert.IsTrue(gilles.Blog.Posts.Count > 0);
        }

        [Test]
        public void InsertAuthor()
        {
            Author author = new Author();
            author.Name = "test" + DateTime.Now.ToShortDateString();
            author.Login = "Login";
            author.Password = "Password";

            Assert.IsNull(author.Id);
            mapper.Insert("Author-Insert", author);
            Assert.IsNotNull(author.Id);

            mapper.Delete("Author-Delete", author.Id);
        }

        [Test]
        public void UpdateAuthor()
        {
            gilles = mapper.QueryForObject<Author>("Author-Select-Basic", 2);
            string newPassword= DateTime.Now.ToShortDateString();
            gilles.Password = newPassword;

            mapper.Update("Author-Update", gilles);
            gilles = mapper.QueryForObject<Author>("Author-Select-Basic", 2);

            Assert.IsTrue(gilles.Password == newPassword);

            gilles.Password = "test";

            mapper.Update("Author-Update", gilles);

        }
        
    }
}
