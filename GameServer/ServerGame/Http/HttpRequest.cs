using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


class HttpRequest : BaseHeader
{
    public Dictionary<string, string> Params { get; private set; }

    public string Method { get; private set; }

    public string URL { get; set; }

    private const int MAX_SIZE = 1024 * 1024 * 2;
    private byte[] bytes = new byte[MAX_SIZE];

    public string json { get; set; }

    public string ProtocolVersion { get; set; }

    private Stream handler;
    public HttpRequest(Stream stream)
    {
        this.handler = stream;
        //byte[] buffer = new byte[1024 * 10];
        //stream.Read(buffer,0,buffer.Length);
        //Debug.Log(Encoding.UTF8.GetString(buffer)+"\n");
        var data = GetRequestData(handler);
        var rows = Regex.Split(data, Environment.NewLine);

        //Request URL & Method & Version
        var first = Regex.Split(rows[0], @"(\s+)")
            .Where(e => e.Trim() != string.Empty)
            .ToArray();
        if (first.Length > 0) this.Method = first[0];
        if (first.Length > 1) this.URL = Uri.UnescapeDataString(first[1]);
        if (first.Length > 2) this.ProtocolVersion = first[2];

        //Request Headers
        this.Headers = GetRequestHeaders(rows);

        //Request Body
        Body = GetRequestBody(rows);
        
        var contentLength = GetHeader(RequestHeaders.ContentLength);
        if (int.TryParse(contentLength, out var length) && Body.Length != length)
        {
            do
            {
                length = stream.Read(bytes, 0, MAX_SIZE - 1);
                Body += Encoding.UTF8.GetString(bytes, 0, length);
            } while (Body.Length != length);
        }
        //Request "GET"
        if (this.Method == "GET")
        {
            var isUrlencoded = this.URL.Contains('?');
            if (isUrlencoded) this.Params = GetRequestParameters(URL.Split('?')[1]);
        }

        //Request "POST"
        if (this.Method == "POST")
        {
            var contentType = GetHeader(RequestHeaders.ContentType);
            var isUrlencoded = contentType == @"application/x-www-form-urlencoded";
            if (isUrlencoded) this.Params = GetRequestParameters(this.Body);
       
            var isJson = contentType == @"application/json";
            if (isJson) this.Params = GetRequestParameters(this.Body); this.json = this.Body;
            //Debug.Log("Body:\n" + this.Body);
        }
       
    }

    public Stream GetRequestStream()
    {
        return this.handler;
    }

    public string GetHeader(RequestHeaders header)
    {
        return GetHeaderByKey(header);
    }

    public string GetHeader(string fieldName)
    {
        return GetHeaderByKey(fieldName);
    }

    public void SetHeader(RequestHeaders header, string value)
    {
        SetHeaderByKey(header, value);
    }

    public void SetHeader(string fieldName, string value)
    {
        SetHeaderByKey(fieldName, value);
    }

    private string GetRequestData(Stream stream)
    {
        var length = 0;
        var data = string.Empty;

        do
        {
            length = stream.Read(bytes, 0, MAX_SIZE - 1);
            data += Encoding.UTF8.GetString(bytes, 0, length);
        } while (length > 0 && !data.Contains("\r\n\r\n"));
        Debug.Log("Receive Request,Content : \n"+data);
        return data;
    }

    private string GetRequestBody(IEnumerable<string> rows)
    {
        var target = rows.Select((v, i) => new { Value = v, Index = i }).FirstOrDefault(e => e.Value.Trim() == string.Empty);
        if (target == null) return null;
        var range = Enumerable.Range(target.Index + 1, rows.Count() - target.Index - 1);
        return string.Join(Environment.NewLine, range.Select(e => rows.ElementAt(e)).ToArray());
    }

    private Dictionary<string, string> GetRequestHeaders(IEnumerable<string> rows)
    {
        if (rows == null || rows.Count() <= 0) return null;
        var target = rows.Select((v, i) => new { Value = v, Index = i }).FirstOrDefault(e => e.Value.Trim() == string.Empty);
        var length = target == null ? rows.Count() - 1 : target.Index;
        if (length <= 1) return null;
        var range = Enumerable.Range(1, length - 1);
        return range.Select(e => rows.ElementAt(e)).ToDictionary(e => e.Split(':')[0], e => e.Split(':')[1].Trim());
    }

    private Dictionary<string, string> GetRequestParameters(string row)
    {
        if (string.IsNullOrEmpty(row)) return null;
        var kvs = Regex.Split(row, "&");
        if (kvs == null || kvs.Count() <= 0) return null;

        return kvs.ToDictionary(e => Regex.Split(e, "=")[0], e => { var p = Regex.Split(e, "="); return p.Length > 1 ? p[1] : ""; });
    }
}

 

