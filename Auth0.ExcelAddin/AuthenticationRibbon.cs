using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using Auth0.OidcClient;
using Microsoft.Office.Tools.Ribbon;

namespace Auth0.ExcelAddin
{
    public partial class AuthenticationRibbon
    {
        private void AuthenticationRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, RibbonControlEventArgs e)
        {
            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            }

            var options = new Auth0ClientOptions
            {
                ClientId = ConfigurationManager.AppSettings["Auth0ClientId"],
                Domain = ConfigurationManager.AppSettings["Auth0Domain"]
            };

            var oidClient = new Auth0Client(options);

            var result = await oidClient.LoginAsync();
        }
    }
}
