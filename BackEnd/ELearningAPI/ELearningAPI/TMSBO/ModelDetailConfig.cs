using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMSBO
{
    public class ModelDetailConfig
    {

        /// <summary>
        /// Tên bảng detail trong DB
        /// </summary>
        public string DetailTableName { get; set; }

        /// <summary>
        /// Tên cột ForeignKey
        /// </summary>
        public string ForeignKeyName { get; set; }

        /// <summary>
        /// Tên property kiểu List of detailObject trên master model
        /// </summary>
        public string PropertyOnMasterModel { get; set; }

        /// <summary>
        /// Có xóa trước khi xóa master model không
        /// </summary>
        public bool CascadeOnDeleteMasterModel { get; set; }

        public ModelDetailConfig(string detailTableName, string foreignKeyName, string propertyOnMasterModel, bool cascadeOnDeleteMasterModel)
        {
            this.DetailTableName = detailTableName;
            this.ForeignKeyName = foreignKeyName;
            this.PropertyOnMasterModel = propertyOnMasterModel;
            this.CascadeOnDeleteMasterModel = cascadeOnDeleteMasterModel;
        }

    }
}
