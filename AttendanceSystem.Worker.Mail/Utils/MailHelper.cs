using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Worker.Mail.Utils
{
    public static class MailHelper
    {
        public static StringBuilder LoadWelcomeMailTemplate()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Templates", "WelcomeMailTemplate.html");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Mail template file not found.", filePath);
            }
            return new StringBuilder(File.ReadAllText(filePath));
        }
    }
}
