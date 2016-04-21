using ASP.NET_MVC_6.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_6.Models
{
    public class BlogModel
    {
        [Display(Name = "博客名称")]
        [Required(ErrorMessage = "请输入你的博客名称！")]
        public string BlogName { get; set; }

        [Display(Name = "博客地址")]
        [Required(ErrorMessage = "请输入你的博客地址！")]
        public string BlogAddress { get; set; }

        [Display(Name = "博客图片")]
        [Required(ErrorMessage = "请上传你的博客图片！")]
        //[ValidateFile]
        public HttpPostedFileBase BlogPhoto { get; set; }
    }
}