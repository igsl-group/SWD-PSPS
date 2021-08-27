using Psps.Core.Models;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Psps.Models.Dto.Security
{
    [DataContract]
    public class UserInfo : IUserInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string PostId { get; set; }

        [DataMember]
        public DateTime? PasswordChangedDate { get; set; }

        [DataMember]
        public string OriginalPostIdIfActed { get; set; }

        [DataMember]
        public bool IsSysAdmin { get; set; }

        public static UserInfo FromString(string userInfoData)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(userInfoData)))
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(UserInfo));
                return js.ReadObject(stream) as UserInfo;
            }
        }

        public override string ToString()
        {
            using (var stream = new MemoryStream())
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(UserInfo));
                js.WriteObject(stream, this);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}