﻿using System;
using System.Collections.Generic;
using Chromium.Event;
using Chromium.Remote;
using Chromium.Remote.Event;
using Chromium.WebBrowser;
using Chromium.WebBrowser.Event;
using Neutronium.Core;
using Neutronium.Core.WebBrowserEngine.JavascriptObject;
using Neutronium.Core.WebBrowserEngine.Window;

namespace Neutronium.WebBrowserEngine.ChromiumFx.EngineBinding
{
    public class ChromiumFxControlWebBrowserWindow : IModernWebBrowserWindow
    {
        private readonly ChromiumWebBrowser _ChromiumWebBrowser;
        private readonly IDispatcher _dispatcher ;
        private readonly IWebSessionLogger _Logger;
        private CfrBrowser _WebBrowser;
        private bool _FirstLoad = true;
        private bool _SendLoadOnContextCreated = false;

        public IWebView MainFrame { get; private set; }
        public Uri Url => _ChromiumWebBrowser.Url;
        public bool IsLoaded  => !_ChromiumWebBrowser.IsLoading;
        private readonly List<ContextMenuItem> _Commands = new List<ContextMenuItem>();
        private readonly List<int> _MenuSeparatorIndex = new List<int>();

        public ChromiumFxControlWebBrowserWindow(ChromiumWebBrowser chromiumWebBrowser, IDispatcher dispatcher, IWebSessionLogger logger) 
        {
            _Logger = logger;
            _dispatcher = dispatcher;
            _ChromiumWebBrowser = chromiumWebBrowser;
            _ChromiumWebBrowser.LoadHandler.OnLoadEnd += OnLoadEnd;
            _ChromiumWebBrowser.DisplayHandler.OnConsoleMessage += OnConsoleMessage;
            _ChromiumWebBrowser.OnV8ContextCreated += OnV8ContextCreated;
            _ChromiumWebBrowser.RemoteBrowserCreated += OnChromiumWebBrowser_RemoteBrowserCreated;
            _ChromiumWebBrowser.ContextMenuHandler.OnBeforeContextMenu += OnBeforeContextMenu;
            _ChromiumWebBrowser.ContextMenuHandler.OnContextMenuCommand += ContextMenuHandler_OnContextMenuCommand;
            _ChromiumWebBrowser.RequestHandler.OnRenderProcessTerminated += RequestHandler_OnRenderProcessTerminated;
            _ChromiumWebBrowser.LifeSpanHandler.OnBeforePopup += LifeSpanHandler_OnBeforePopup;
            ChromiumWebBrowser.OnBeforeCommandLineProcessing += ChromiumWebBrowser_OnBeforeCommandLineProcessing;
        }

        private void ChromiumWebBrowser_OnBeforeCommandLineProcessing(CfxOnBeforeCommandLineProcessingEventArgs e)
        {
        }

        private void LifeSpanHandler_OnBeforePopup(object sender, CfxOnBeforePopupEventArgs e)
        {
        }

        private void RequestHandler_OnRenderProcessTerminated(object sender, CfxOnRenderProcessTerminatedEventArgs e) 
        {
            var crashed = Crashed;
            if (crashed != null)
                _dispatcher.Dispatch(() => crashed(this, new BrowserCrashedArgs()));
        }

        private void OnBeforeContextMenu(object sender, CfxOnBeforeContextMenuEventArgs e) 
        {
            var model = e.Model;
            for(var index= model.Count-1; index>=0 ; index--) 
            {
                if (!CfxContextMenu.IsEdition(model.GetCommandIdAt(index)))
                    model.RemoveAt(index);
            }

            if (model.Count != 0)
                return;

            var rank = (int) ContextMenuId.MENU_ID_USER_FIRST;
            _Commands.ForEach(command => model.AddItem(rank++, command.Name));
            _MenuSeparatorIndex.ForEach(index => model.InsertSeparatorAt(index));
        }

        public IModernWebBrowserWindow RegisterContextMenuItem(IEnumerable<ContextMenuItem> contextMenuItens)
        {
            _Commands.AddRange(contextMenuItens);
            _MenuSeparatorIndex.Insert(0, _Commands.Count);
            return this;
        }

        private void ContextMenuHandler_OnContextMenuCommand(object sender, CfxOnContextMenuCommandEventArgs e)
        {
            if (!CfxContextMenu.IsUserDefined(e.CommandId))
                return;

            var command = _Commands[e.CommandId - (int)ContextMenuId.MENU_ID_USER_FIRST].Command;
            command.Invoke();
        }

        private void OnChromiumWebBrowser_RemoteBrowserCreated(object sender, RemoteBrowserCreatedEventArgs e) 
        {
            _WebBrowser = e.Browser;
        }

        private void OnV8ContextCreated(object sender, CfrOnContextCreatedEventArgs e)
        {
            MainFrame = new ChromiumFxWebView(e.Browser, _Logger);

            var beforeJavascriptExecuted = BeforeJavascriptExecuted;
            if (beforeJavascriptExecuted == null) 
                return;

            Action<string> execute = (code) => e.Frame.ExecuteJavaScript(code, String.Empty, 0);
            beforeJavascriptExecuted(this, new BeforeJavascriptExcecutionArgs(MainFrame, execute));

            if (_SendLoadOnContextCreated)
                SendLoad();
        }

        private void OnConsoleMessage(object sender, CfxOnConsoleMessageEventArgs e)
        {
            ConsoleMessage?.Invoke(this, new ConsoleMessageArgs(e.Message, e.Source, e.Line));
        }

        private void OnLoadEnd(object sender, CfxOnLoadEndEventArgs e) 
        {
            if (!e.Frame.IsMain)
                return;

            if (_FirstLoad) 
            {
                _FirstLoad = false;
                return;
            }

            SendLoad();
        }        

        private void SendLoad()
        {
            var loadEnd = LoadEnd;
            if (loadEnd == null)
                return;

            if (MainFrame == null)
            {
                _ChromiumWebBrowser.ExecuteJavascript("(function(){})()");
                _SendLoadOnContextCreated = true;
                return;
            }

            _SendLoadOnContextCreated = false;
            _dispatcher.Dispatch(() => loadEnd(this, new LoadEndEventArgs(MainFrame)));
        }

        public void NavigateTo(Uri path)
        {
            var url = path.Scheme == "file" ? path.AbsolutePath : path.ToString();
            _ChromiumWebBrowser.LoadUrl(url);
        } 

        public event EventHandler<LoadEndEventArgs> LoadEnd;

        public event EventHandler<ConsoleMessageArgs> ConsoleMessage;        
        
        public event EventHandler<BeforeJavascriptExcecutionArgs> BeforeJavascriptExecuted;

        public event EventHandler<BrowserCrashedArgs> Crashed;
    }
}
