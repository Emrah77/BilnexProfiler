using System;
using System.Diagnostics;
using System.IO;
using RedGate.SIPFrameworkShared;

namespace ExpressProfiler.Ecosystem
{
    class ExpressProfiler : ISsmsAddin4
    {
        public string Version { get { return "1.0"; } }
        public string Description { get { return "Bilnex Sql Server Profiler"; } }
        public string Name { get { return "BilnexProfiler"; } }
        public string Author { get { return "BilnexProfiler"; } }
        public string Url { get { return "https://bilnex.com.tr/"; } }

        internal static ISsmsFunctionalityProvider6 m_Provider;
        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            m_Provider = (ISsmsFunctionalityProvider6)provider;
            m_Provider.AddToolbarItem(new ExecuteExpressProfiler());
            var command = new ExecuteExpressProfiler();
            m_Provider.AddToolsMenuItem(command);
        }


        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
        }

        public void OnShutdown(){}
    }

    public class ExecuteExpressProfiler :  ISharedCommand
    {
        public void Execute()
        {
            string param ="";
            IConnectionInfo2 con;
            if(ExpressProfiler.m_Provider.ObjectExplorerWatcher.TryGetSelectedConnection(out con))
            {
                string server = con.Server;
                string user = con.UserName;
                string password = con.Password;
                bool trusted = con.IsUsingIntegratedSecurity;
                param = trusted ? String.Format("-server \"{0}\"",server) : String.Format("-server \"{0}\" -user \"{1}\" -password \"{2}\"", server,user,password);
            }
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string profiler = Path.Combine(root, "ExpressProfiler\\ExpressProfiler.exe");
            Process.Start(profiler, param);
        }

        private readonly ICommandImage m_CommandImage = new CommandImageForEmbeddedResources(typeof(ExecuteExpressProfiler).Assembly, "ExpressProfiler.Ecosystem.Resources.Icon.png");
        public string Name {get { return "BilnexProfilerExecute"; }}
        public string Caption { get { return "BilnexProfiler"; } }
        public string Tooltip { get { return "Execute BilnexProfiler"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new string[] { }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }
    }
}
