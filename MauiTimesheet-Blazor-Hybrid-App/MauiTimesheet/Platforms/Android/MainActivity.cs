using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Widget;
using static Android.Resource;

namespace MauiTimesheet
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            GlobalLayoutUtil.AssistActivity(this);

            Window.DecorView.SystemUiFlags = Android.Views.SystemUiFlags.LightStatusBar;
        }
    }
    // Fixes the keyboard overlaps the Blazor webview
    //https://github.com/dotnet/maui/issues/14197#issuecomment-1535561632
#nullable disable
    public class GlobalLayoutUtil
    {
        private bool isImmersed = false;
        private Android.Views.View mChildOfContent;
        private FrameLayout.LayoutParams frameLayoutParams;
        private int usableHeightPrevious = 0;

        public static void AssistActivity(Activity activity)
        {
            _ = new GlobalLayoutUtil(activity);
        }

        public GlobalLayoutUtil(Activity activity)
        {
            FrameLayout content = (FrameLayout)activity.FindViewById(Id.Content);
            mChildOfContent = content.GetChildAt(0);
            mChildOfContent.ViewTreeObserver.GlobalLayout += (s, o) => PossiblyResizeChildOfContent(activity);
            frameLayoutParams = (FrameLayout.LayoutParams)mChildOfContent.LayoutParameters;
        }

        private void PossiblyResizeChildOfContent(Activity activity)
        {

            int usableHeightNow = ComputeUsableHeight();

            if (usableHeightNow != usableHeightPrevious)
            {

                int usableHeightSansKeyboard = mChildOfContent.RootView.Height;
                int heightDifference = usableHeightSansKeyboard - usableHeightNow;

                if (heightDifference < 0)
                {
                    usableHeightSansKeyboard = mChildOfContent.RootView.Width;
                    heightDifference = usableHeightSansKeyboard - usableHeightNow;
                }
                // keyboard probably just became visible

                if (heightDifference > usableHeightSansKeyboard / 4)
                {

                    frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference;
                }
                //else if (heightDifference >= GetNavigationBarHeight(activity))
                //{
                //    // keyboard probably just became hidden                //
                //    
                //    frameLayoutParams.Height = usableHeightNow - GetNavigationBarHeight(activity);
                //}
                else
                {
                    frameLayoutParams.Height = usableHeightNow;
                }
            }

            mChildOfContent.RequestLayout();

            usableHeightPrevious = usableHeightNow;
        }



        private int ComputeUsableHeight()
        {
            Android.Graphics.Rect rect = new Android.Graphics.Rect();
            mChildOfContent.GetWindowVisibleDisplayFrame(rect);
            if (isImmersed)
                return (int)rect.Bottom;
            else
                return (int)(rect.Bottom - rect.Top);
        }

        /**
         *
         * @param context:
         * @return: 
         */
        private static int GetNavigationBarHeight(Context context)
        {
            int result = 0;
            Resources resources = context.Resources;
            int resourceId =
                    resources.GetIdentifier("navigation_bar_height", "dimen", "android");
            if (resourceId > 0)
                result = resources.GetDimensionPixelSize(resourceId);
            return result;
        }
    }
}
