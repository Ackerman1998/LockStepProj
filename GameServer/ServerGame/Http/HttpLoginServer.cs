using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class HttpLoginServer:HttpServer
{
    public override void Get(HttpRequest request, HttpResponse response)
    {
        base.Get(request, response);
        //当文件不存在时应返回404状态码
        string requestURL = request.URL;
        requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
        string requestFile = Path.Combine(ServerRoot, requestURL);

        //判断地址中是否存在扩展名
        string extension = Path.GetExtension(requestFile);

        //根据有无扩展名按照两种不同链接进行处
        if (extension != "")
        {
            //从文件中返回HTTP响应
            response = response.FromFile(requestFile);
        }
        else
        {
            #region Load Index
            ////目录存在且不存在index页面时时列举目录
            //if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
            //{
            //    requestFile = Path.Combine(ServerRoot, requestFile);
            //    var content = ListDirectory(requestFile, requestURL);
            //    response = response.SetContent(content, Encoding.UTF8);
            //    response.Content_Type = "text/html; charset=UTF-8";
            //}
            //else
            //{
            //    //加载静态HTML页面
            //    requestFile = Path.Combine(requestFile, "index.html");
            //    response = response.FromFile(requestFile);
            //    response.Content_Type = "text/html; charset=UTF-8";
            //}
            #endregion
            if (requestURL.Equals("geturl")) {
                response = response.FromText("www.baidu.com");
                response.Content_Type = "text/plain";
            }
        }

        //发送HTTP响应
        response.Send();
    }
    public override void Post(HttpRequest request, HttpResponse response)
    {
        base.Post(request, response);
        //获取客户端传递的参数
        string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());
        Debug.Log(request.URL + " request.URL\n");
        if (request.URL.Contains("requestlogin")) {
            //设置返回信息
            //string content = string.Format("这是通过Post方式返回的数据:{0}", data);
            data = request.json;
            Debug.Log(data);
            LoginData loginData = LitJson.JsonMapper.ToObject<LoginData>(data);
            Debug.Log(loginData.userId+"\t"+loginData.password+ " request login\n");
            string content = "loginsuccess";
            //构造响应报文
            response.SetContent(content);
            response.Content_Encoding = "utf-8";
            response.StatusCode = "200";
            response.Content_Type = "text/html; charset=UTF-8";
            response.Headers["Server"] = "ExampleServer";
            //发送响应
            response.Send();
        }
       
    }

    private string ConvertPath(string[] urls)
    {
        string html = string.Empty;
        int length = ServerRoot.Length;
        foreach (var url in urls)
        {
            var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
            html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
        }

        return html;
    }

    private string ListDirectory(string requestDirectory, string requestURL)
    {
        //列举子目录
        var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
        folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
        var foldersList = ConvertPath(folders);

        //列举文件
        var files = Directory.GetFiles(requestDirectory);
        var filesList = ConvertPath(files);

        //构造HTML
        StringBuilder builder = new StringBuilder();
        builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
        builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
             requestURL, filesList, foldersList));

        return builder.ToString();
    }

}


class LoginData
{
    public string userId;
    public string password;
}