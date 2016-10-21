using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace 博客园UWP
{

    public sealed partial class Login : ContentDialog
    {
        // 无跳转
        string _login_url = "https://passport.cnblogs.com/user/signin";
        // 跳转到主页
        string _login_url_jumpHome = "https://passport.cnblogs.com/user/signin?ReturnUrl=https%3A%2F%2Fhome.cnblogs.com%2Fu%2Ffrendguo%2F";
        // 个人主页地址，WebView中若出现这个地址，则表示登录成功
        string _login_success = "https://home.cnblogs.com/";

        string loginResult = "";
        public Login()
        {
            this.InitializeComponent();
            LoginWebView.Navigate(new Uri(_login_url_jumpHome));
        }

        /// <summary>
        /// 执行登录操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                // 用户名
                string js = "document.getElementById('input1').setAttribute('value', '" + UserInputTextBox.Text + "');";
                // 密码
                js += "document.getElementById('input2').setAttribute('value', '" + PasswordInptutBox.Password + "');";
                // 点击登录
                js += "document.getElementById('signin').click();";

                // 执行脚本文件
                await LoginWebView.InvokeScriptAsync("eval", new string[] { js });

                CheckLoginStatus();

            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }

        /// <summary>
        /// 检查登录状态
        /// </summary>
        public async void CheckLoginStatus()
        {
            string login_js = "var o = document.getElementById('tip_btn');if(o) o.innerText;";
            loginResult = await LoginWebView.InvokeScriptAsync("eval", new string[] { login_js });

            if (loginResult.Contains("成功"))
            {
                return;
            }

            // 每隔100毫秒检测下状态
            await Task.Delay(100); 

            CheckLoginStatus();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void LoginWebView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith(_login_success))
            {
                // TODO
            }
            
            
        }
    }
}
