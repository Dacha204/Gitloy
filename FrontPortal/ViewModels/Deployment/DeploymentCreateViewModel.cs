using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gitloy.Services.FrontPortal.ViewModels.Deployment
{
    public class DeploymentCreateViewModel
    {      
        [Required]
        [DisplayName("Git URL")]
        [DataType(DataType.Url)]
        public string GitUrl { get; set; }
        
        [Required]
        [DisplayName("Git Branch")]
        public string GitBranch { get; set; } = "master";
        
        [Required]
        [DisplayName("FTP Username")]
        public string FtpUsername { get; set; }
        
        [Required]
        [DisplayName("FTP Password")]
        [DataType(DataType.Password)]
        public string FtpPassword { get; set; }
        
        [Required]
        [DisplayName("FTP Hostname")]
        public string FtpHostname { get; set; }
        
        [Required]
        [DisplayName("FTP Port")]
        public int FtpPort { get; set; } = 21;
        
        [Required]
        [DisplayName("FTP Root Directory")]
        public string FtpRootDirectory { get; set; } = "/";        
    }
}