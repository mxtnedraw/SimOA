//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimOA_Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WFInstance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WFInstance()
        {
            this.WFStep = new HashSet<WFStep>();
        }
    
        public long InstanceId { get; set; }
        public string InstanceTitle { get; set; }
        public string Details { get; set; }
        public long SubBy { get; set; }
        public string SubTime { get; set; }
        public string Remark { get; set; }
        public string InstanceGuid { get; set; }
        public int InstanceState { get; set; }
        public string RejectMsg { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFStep> WFStep { get; set; }
    }
}
