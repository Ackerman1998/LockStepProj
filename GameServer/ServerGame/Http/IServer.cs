using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IServer
{
    void Post(HttpRequest httpRequest, HttpResponse httpResponse);
    void Get(HttpRequest httpRequest, HttpResponse httpResponse);
}

