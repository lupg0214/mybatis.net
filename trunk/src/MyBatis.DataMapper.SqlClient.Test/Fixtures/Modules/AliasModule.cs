using MyBatis.DataMapper.Configuration.Module;
using MyBatis.DataMapper.SqlClient.Test.Domain;

namespace MyBatis.DataMapper.SqlClient.Test.Fixtures.Modules
{
    class AliasModule: Module
    {
        /// <summary>
        /// Override to add code configuration mapping to the <see cref="Apache.Ibatis.DataMapper.Configuration.IConfigurationEngine"/>.
        /// </summary>
        public override void Load()
        {
            RegisterAlias<SimpleSqlSource>();
            RegisterAlias<SqlSourceWithParameter>();
            RegisterAlias<SqlSourceWithInlineParameter>();
            RegisterAlias<NVelocityDynamicEngine>();
            RegisterAlias<IAccount>();
        }
    }
}
