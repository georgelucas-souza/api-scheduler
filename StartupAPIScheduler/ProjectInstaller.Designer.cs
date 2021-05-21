namespace StartupAPIScheduler
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.api_scheduler_process_installer = new System.ServiceProcess.ServiceProcessInstaller();
            this.api_scheduler_installer = new System.ServiceProcess.ServiceInstaller();
            // 
            // api_scheduler_process_installer
            // 
            this.api_scheduler_process_installer.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.api_scheduler_process_installer.Password = null;
            this.api_scheduler_process_installer.Username = null;
            // 
            // api_scheduler_installer
            // 
            this.api_scheduler_installer.Description = "[Flex][Startup] Schedule API\'s automatically";
            this.api_scheduler_installer.ServiceName = "startup_api_scheluder";
            this.api_scheduler_installer.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.api_scheduler_process_installer,
            this.api_scheduler_installer});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller api_scheduler_process_installer;
        private System.ServiceProcess.ServiceInstaller api_scheduler_installer;
    }
}