using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Domain.Entities
{
    public class AwsSettings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string Region { get; set; }
    }
}
