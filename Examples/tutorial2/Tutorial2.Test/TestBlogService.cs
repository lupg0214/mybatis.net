using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using NUnit.Framework;
using Tutorial2.Domain;
using Tutorial2.Service;

namespace Tutorial2.Test
{
    [TestFixture]
    public class TestBlogService
    {
        private IBlogService blogService = null;
        [SetUp]
        public void SetUp()
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            ISqlMapper mapper = builder.Configure(@"../../../Files/sqlMap.config");

            blogService = new BlogService(mapper);
        }

        [Test]
        public void SelectVerify()
        {
            Author gilles = blogService.Verify("gilles","test");

            Assert.IsNotNull(gilles,"gilles is null !");
            Assert.IsTrue(gilles.Login == "gilles");
            Assert.IsNotNull(gilles.Blog, "Gilles blog is null");
            Assert.IsTrue(gilles.Blog.Name == "Gilles Weblog", "Gilles blog is not 'Gilles Weblog' ");

            Assert.IsTrue(gilles.Blog.Posts.Count > 0);

        }
    }
}
