﻿using Tests.ChromiumFX.Infra;
using Tests.Infra.HTMLEngineTesterHelper.HtmlContext;

namespace Tests.ChromiumFX.HTMLEngineTests.Infra
{
    public class ChromiumFXContext : ChromiumFXWindowLessHTMLEngineProvider 
    {
        public ChromiumFXContext() : base(new NullTestHtmlProvider())
        {           
        }
    }
}