using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.DirectInput;

namespace WPF_DancePad_Client
{
    class Joystick
    {
        //במחלקה קיים שימוש בשני ממשקים DirectX 
        //ממשק אשר מכיל בתוכו ספריות זמן ריצה אשר פותחו על ידי Microsoft
        //מטרתו לתת גישה ישירה לחומרה

       //DirectXInput קליטת קלט לא סטנדרטי 



        private Device joystickDevice; ///עצם מסוג Device שעליו ניתן להפעיל פעולות
                                      /// שיסייעו בקבלת האותות משטיח הריקוד.
        private JoystickState state;
        public int Xaxis; // X-axis movement
        public int Yaxis; //Y-axis movement
        private IntPtr hWnd; ///משתנה המצביע על חלון,window
        public bool[] buttons;///מערך בוליאני המציג איזה כפתורים היו בלחיצה-
                               /// true משמע כפתור שנלחץ,false משמע כפתור
                             ///  שלא נלחץ.
        private string systemJoysticks; ///שם הjoystick, ישנם כל מיני סוגים.

        /// <summary>
        ///  הפעולה הבונה של המחלקה.פעולה זו מאתחלת
       /// את ערך הhandle ואת ערך התכונה Xaxis ל1-       
        /// </summary>
        /// <returns></returns>
        public Joystick(IntPtr window_handle)
        {
            hWnd = window_handle;
            Xaxis = -1;
        }
        /// <summary>
        /// פעולה שמטרתה לאתר את שטיח הריקוד מתוך כל 
        /// החומרות שמחוברות למחשב.
        /// </summary>
        /// <returns></returns>
        public string FindJoysticks()
        {
            systemJoysticks = null;

            try
            {
                // Find all the GameControl devices that are attached.
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly); ///קיבלתת רשימה של כל בקרי המשחק המחוברים למערכת .לאחר שנמצאה רשימת מכשירים יכולתי לאתר               
                // check that we have at least one device.
                if (gameControllerList.Count > 0)
                {
                    foreach (DeviceInstance deviceInstance in gameControllerList)
                    {
                        // create a device from this controller so we can retrieve info.
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);

                        systemJoysticks = joystickDevice.DeviceInformation.InstanceName;

                        break;
                    }
                }
            }
            catch
            {
                return null;
            }

            return systemJoysticks;
        }


        /// <summary>
        ///פעולה שמקבלת את שם החומרה אליה אנחנו רוצים להתייחס ומנסה לתפוס אותה.
        ///אם היא תופסת מחזירה אמת וההפך.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AcquireJoystick(string name)
        {
            try
            {
                DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
                int i = 0;
                bool found = false;

                foreach (DeviceInstance deviceInstance in gameControllerList)
                {
                    if (deviceInstance.InstanceName == name)
                    {
                        found = true;
                        joystickDevice = new Device(deviceInstance.InstanceGuid);
                        joystickDevice.SetCooperativeLevel(hWnd, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                        break;
                    }
                    i++;
                }

                if (!found)
                    return false;

                joystickDevice.SetDataFormat(DeviceDataFormat.Joystick);

                joystickDevice.Acquire();

                UpdateStatus();
            }
            catch (Exception err)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// פעולה המשחררת את שטיח הריקוד 
        /// </summary>
        public void ReleaseJoystick()
        {
            joystickDevice.Unacquire();
        }

        /// <summary>
        /// פעולה המעדכנת את מצב שטיח הריקוד
        /// </summary>
        public void UpdateStatus()
        {
            Poll();

            int[] extraAxis = state.GetSlider();

            Xaxis = state.X;
            Yaxis = state.Y;

            byte[] jsButtons = state.GetButtons();
            buttons = new bool[jsButtons.Length];

            int i = 0;
            foreach (byte button in jsButtons)
            {
                buttons[i] = button >= 128;
                i++;
            }
        }



        /// <summary>
        /// בכל פעם שנדרש המצב הנוכחי של שטיח
        /// הריקוד, יש לקרוא לשיטת הסקר כדי לעדכן את
        ///  האובייקט joystickDevice
        ///  מצב הג'ויסטיק מתעדכן ומאפשר למצוא
       /// מיקומי ציר ומצבי כפתור נוכחיים.השיטה
      ///  הפרטית הבאה תסקר את שטיח הריקוד ותעדכן
       /// את המצב.שיטה זו מעדכנת אובייקט
        ///JoystickState בשם state, שהוא שדה פרטי
     ///   במחלקה. 
      /// </summary>
        private void Poll()
        {
            try
            {
                // poll the joystick
                joystickDevice.Poll();
                // update the joystick state field
                state = joystickDevice.CurrentJoystickState;
            }
            catch
            {
                throw (null);
            }
        }
    }
}
