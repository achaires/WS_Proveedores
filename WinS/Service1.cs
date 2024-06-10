using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinS
{
    public partial class Service1 : ServiceBase
    {
        Program p = new Program();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            p.iniciaBusqueda();
        }

        protected override void OnStop()
        {
        }
    }
}
