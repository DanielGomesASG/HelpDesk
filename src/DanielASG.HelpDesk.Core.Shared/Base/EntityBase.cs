using Abp.Domain.Entities.Auditing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk
{
    public class EntityBase : FullAuditedEntity
    {
        public virtual string Code { get; set; }

        [JsonIgnore]
        public string DynamicProperty
        {
            get
            {
                if (DynamicProperties != null)
                    return JsonConvert.SerializeObject(DynamicProperties);
                return null;
            }
            private set
            {
                if (value != null)
                    DynamicProperties = JsonConvert.DeserializeObject<List<EntityDynamicProperty>>(value);
            }
        }

        [NotMapped]
        public List<EntityDynamicProperty> DynamicProperties { get; set; } = new List<EntityDynamicProperty>();
    }
}
